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

        public Property<P> CreateProperty<P>(IRegisteredProperty registeredProperty)
        {
            return new Property<P>(registeredProperty.Name);
        }
        public ValidateProperty<P> CreateValidateProperty<P>(IRegisteredProperty registeredProperty)
        {
            return new ValidateProperty<P>(registeredProperty.Name);
        }

        public EditProperty<P> CreateEditProperty<P>(IRegisteredProperty registeredProperty)
        {
            return new EditProperty<P>(registeredProperty.Name);
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