using System;
using System.Collections.Generic;

namespace Neatoo
{

    public delegate IRegisteredPropertyManager CreateRegisteredPropertyManager(Type type);

    /// <summary>
    /// DO NOT REGISTER IN DI CONTAINER
    /// </summary>
    /// <typeparam name="T">Generic to ensure that types can only access their properties</typeparam>
    public interface IRegisteredPropertyManager
    {
        IRegisteredProperty GetRegisteredProperty(string name);
        IEnumerable<IRegisteredProperty> GetRegisteredProperties();
        bool HasProperty(string propertyName);

        void SetType(Type type);
    }

    /// <summary>
    /// REGISTERED IN THE DI CONTAINER
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRegisteredPropertyManager<T> : IRegisteredPropertyManager
    {

    }
}
