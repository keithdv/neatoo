using Neatoo.Rules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Neatoo.Core
{

    /// <summary>
    /// You can't register generic delegates in C#
    /// so you make a factory class
    /// </summary>
    public class DefaultFactory : IFactory
    {

        public DefaultFactory()
        {
        }

        public PropertyValue<P> CreatePropertyValue<P>(IRegisteredProperty registeredProperty, P value)
        {
            if(value is IPropertyValue v){
                v.Name = registeredProperty.Name;
                return v as PropertyValue<P>;
            }

            return new PropertyValue<P>(registeredProperty.Name, value);
        }
        public ValidatePropertyValue<P> CreateValidatePropertyValue<P>(IRegisteredProperty registeredProperty, P value)
        {
            if (value is IPropertyValue v)
            {
                v.Name = registeredProperty.Name;
                return v as ValidatePropertyValue<P>;
            }

            return new ValidatePropertyValue<P>(registeredProperty.Name, value);
        }

        public EditPropertyValue<P> CreateEditPropertyValue<P>(IRegisteredProperty registeredProperty, P value)
        {
            // TODO: I think this should throw an exception if value is not an EditPropertyValue<P>

            if(!typeof(P).IsGenericType &&                
                registeredProperty.PropertyInfo.PropertyType.IsGenericType && registeredProperty.PropertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                throw new Exception("Since the property is Nullable, you must define the type of the property in LoadProperty or Set Property, ex LoadProperty<int?>(registeredProperty, value)");
            }

            if (value is IPropertyValue v)
            {
                v.Name = registeredProperty.Name;
                return v as EditPropertyValue<P>;
            }

            return new EditPropertyValue<P>(registeredProperty.Name, value);
        }

    }

    [Serializable]
    public class GlobalFactoryException : Exception
    {
        public GlobalFactoryException() { }
        public GlobalFactoryException(string message) : base(message) { }
        public GlobalFactoryException(string message, Exception inner) : base(message, inner) { }
        protected GlobalFactoryException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

}