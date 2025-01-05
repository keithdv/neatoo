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

        public IPropertyValue<P> CreatePropertyValue<P>(IRegisteredProperty<P> registeredProperty, P value)
        {
            return new PropertyValue<P>(registeredProperty.Name, value);
        }
        public IValidatePropertyValue<P> CreateValidatePropertyValue<P>(IRegisteredProperty<P> registeredProperty, P value)
        {
            return new ValidatePropertyValue<P>(registeredProperty.Name, value);
        }

        public IEditPropertyValue<P> CreateEditPropertyValue<P>(IRegisteredProperty<P> registeredProperty, P value)
        {
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