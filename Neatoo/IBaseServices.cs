﻿using Neatoo.Core;
using System.Reflection;

namespace Neatoo
{

    /// <summary>
    /// Wrap the NeatooBase services into an interface so that 
    /// the inheriting classes don't need to list all services
    /// and services can be added
    /// </summary>
    public interface IBaseServices<T>
    {
        IPropertyManager<IProperty> PropertyManager { get; }
        IRegisteredPropertyManager<T> RegisteredPropertyManager { get; }
    }


    public class BaseServices<T> : IBaseServices<T>
        where T : Base<T> // Important - I need the concrete type at least once. This is where I get it
    {
        public BaseServices()
        {
            RegisteredPropertyManager = new RegisteredPropertyManager<T>((PropertyInfo pi) => new RegisteredProperty(pi));
            PropertyManager = new PropertyManager<IProperty>(RegisteredPropertyManager, new DefaultFactory());
        }

        public BaseServices(CreatePropertyManager propertyManager, IRegisteredPropertyManager<T> registeredPropertyManager)
        {
            RegisteredPropertyManager = registeredPropertyManager;
            PropertyManager = propertyManager(RegisteredPropertyManager);
        }


        public IPropertyManager<IProperty> PropertyManager { get; protected set; }
        public IRegisteredPropertyManager<T> RegisteredPropertyManager { get; }

    }
}
