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

        public PropertyValue<P> CreatePropertyValue<P>(IRegisteredProperty registeredProperty, IBase parent)
        {
            return new PropertyValue<P>(registeredProperty.Name) { Parent = parent };
        }
        public ValidatePropertyValue<P> CreateValidatePropertyValue<P>(IRegisteredProperty registeredProperty, IBase parent)
        {
            return new ValidatePropertyValue<P>(registeredProperty.Name) { Parent = parent };
        }

        public EditPropertyValue<P> CreateEditPropertyValue<P>(IRegisteredProperty registeredProperty, IBase parent)
        {
            return new EditPropertyValue<P>(registeredProperty.Name) { Parent = parent };
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