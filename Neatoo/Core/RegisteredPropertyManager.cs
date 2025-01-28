using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neatoo.Core
{

    public class RegisteredPropertyManager : IRegisteredPropertyManager
    {

        protected CreateRegisteredProperty CreateRegisteredProperty { get; }
        protected static IDictionary<Type, IDictionary<string, IRegisteredProperty>> RegisteredPropertiesByType { get; } = new ConcurrentDictionary<Type, IDictionary<string, IRegisteredProperty>>();

        protected IDictionary<string, IRegisteredProperty> RegisteredProperties
        {
            get
            {
                RegisterProperties();
                return RegisteredPropertiesByType[Type];
            }
        }

protected static object lockRegisteredProperties = new object();
        protected Type Type { get; set; }

        public RegisteredPropertyManager(CreateRegisteredProperty createRegisteredProperty)
        {
            CreateRegisteredProperty = createRegisteredProperty;

        }

        public void SetType(Type type)
        {
            Type = type;
            RegisterProperties();
        }

        private static Type[] neatooTypes = new Type[] { typeof(Base<>), typeof(ListBase<>), typeof(ValidateBase<>), typeof(ValidateListBase<>), typeof(EditBase<>), typeof(EditListBase<>) };

        protected void RegisterProperties()
        {
            lock (lockRegisteredProperties)
            {
                if (RegisteredPropertiesByType.ContainsKey(Type))
                {
                    return;
                }

                RegisteredPropertiesByType[Type] = new Dictionary<string, IRegisteredProperty>();

                var type = this.Type;

                // If a type does a 'new' on the property you will have duplicate PropertyNames
                // So honor to top-level type that has that propertyName

                // Problem -- this will include All of the properties even ones we don't declare
                do
                {
                    var properties = type.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | BindingFlags.DeclaredOnly).ToList();

                    foreach (var p in properties)
                    {
                        var prop = CreateRegisteredProperty(p);
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
                        RegisteredProperties.Add(nameof(IValidateBase.ObjectInvalid), CreateRegisteredProperty(objProp));
                        break;
                    }

                    type = type.BaseType;
                }
                while (type != null && (!type.IsGenericType || neatooTypes.Contains(type.GetGenericTypeDefinition())));

            }

        }

        public IRegisteredProperty GetRegisteredProperty(string propertyName)
        {
            RegisterProperties();

            if (!RegisteredProperties.TryGetValue(propertyName, out var prop))
            {
                throw new Exception($"{propertyName} missing on {Type.FullName}");
            }

            return prop;
        }

        public IEnumerable<IRegisteredProperty> GetRegisteredProperties()
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
