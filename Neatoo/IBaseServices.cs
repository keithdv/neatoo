using Neatoo.AuthorizationRules;
using Neatoo.Core;
using Neatoo.Portal;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Neatoo
{

    /// <summary>
    /// Wrap the NeatooBase services into an interface so that 
    /// the inheriting classes don't need to list all services
    /// and services can be added
    /// </summary>
    //public interface IBaseServices<out T>
    //{
    //    IPropertyManager PropertyManager { get; }
    //    IRegisteredPropertyManager RegisteredPropertyManager { get; }
    //}


    public class BaseServices<T>
    {
        public BaseServices()
        {
            RegisteredPropertyManager = new RegisteredPropertyManager((PropertyInfo pi) => new RegisteredProperty(pi));
            PropertyManager = new PropertyManager(RegisteredPropertyManager, new DefaultFactory());
        }

        public BaseServices(IRegisteredPropertyManager<T> registeredPropertyManager)
        {
            RegisteredPropertyManager = registeredPropertyManager;
        }

        public BaseServices(Func<IRegisteredPropertyManager, IPropertyManager> propertyManager, IRegisteredPropertyManager<T> registeredPropertyManager)
        {
            RegisteredPropertyManager = registeredPropertyManager;
            PropertyManager = propertyManager(RegisteredPropertyManager);
        }


        public IPropertyManager PropertyManager { get; protected set; }
        public IRegisteredPropertyManager RegisteredPropertyManager { get; }

    }
}
