using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Neatoo.Core
{

    public class PropertyInfoBag<T> : IPropertyInfoBag<T>
    {

        protected CreatePropertyInfoWrapper CreatePropertyInfo { get; }
        protected static IDictionary<Type, IDictionary<string, IPropertyInfo>> RegisteredPropertiesByType { get; } = new ConcurrentDictionary<Type, IDictionary<string, IPropertyInfo>>();

        protected IDictionary<string, IPropertyInfo> RegisteredProperties
        {
            get
            {
                RegisterProperties();
                return RegisteredPropertiesByType[Type];
            }
        }

protected static object lockRegisteredProperties = new object();
        protected Type Type { get; set; }

        public PropertyInfoBag(CreatePropertyInfoWrapper createPropertyInfoWrapper)
        {

            CreatePropertyInfo = createPropertyInfoWrapper;

            Type = typeof(T);

            RegisterProperties();
        }


        private static Type[] neatooTypes = new Type[] { typeof(Base<>), typeof(ListBase<,>), typeof(ValidateBase<>), typeof(ValidateListBase<,>), typeof(EditBase<>), typeof(EditListBase<,>) };

        protected void RegisterProperties()
        {
            lock (lockRegisteredProperties)
            {
                if (RegisteredPropertiesByType.ContainsKey(Type))
                {
                    return;
                }

                RegisteredPropertiesByType[Type] = new Dictionary<string, IPropertyInfo>();

                var type = this.Type;

                // If a type does a 'new' on the property you will have duplicate PropertyNames
                // So honor to top-level type that has that propertyName

                // Problem -- this will include All of the properties even ones we don't declare
                do
                {
                    var properties = type.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | BindingFlags.DeclaredOnly).ToList();

                    foreach (var p in properties)
                    {
                        var prop = CreatePropertyInfo(p);
                        if (!RegisteredProperties.ContainsKey(p.Name))
                        {
                            RegisteredProperties.Add(p.Name, prop);
                        }
                    }

                    type = type.BaseType;

                } while (type != null && (!type.IsGenericType || !neatooTypes.Contains(type.GetGenericTypeDefinition())));

                do
                {
                    var objProp = type.GetProperty(nameof(IValidateBase.ObjectInvalid), System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | BindingFlags.DeclaredOnly);

                    if (objProp != null)
                    {
                        RegisteredProperties.Add(nameof(IValidateBase.ObjectInvalid), CreatePropertyInfo(objProp));
                        break;
                    }

                    type = type.BaseType;
                }
                while (type != null && (!type.IsGenericType || neatooTypes.Contains(type.GetGenericTypeDefinition())));

            }

        }

        public IPropertyInfo GetPropertyInfo(string propertyName)
        {
            RegisterProperties();

            if (!RegisteredProperties.TryGetValue(propertyName, out var prop))
            {
                throw new Exception($"{propertyName} missing on {Type.FullName}");
            }

            return prop;
        }

        public IEnumerable<IPropertyInfo> Properties()
        {
            RegisterProperties();
            return RegisteredProperties.Values;
        }

        public bool HasProperty(string propertyName)
        {
            RegisterProperties();
            return RegisteredProperties.ContainsKey(propertyName);
        }
    }
}
