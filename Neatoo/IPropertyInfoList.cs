using System.Collections.Generic;

namespace Neatoo
{

    /// <summary>
    /// DO NOT REGISTER IN DI CONTAINER
    /// </summary>
    /// <typeparam name="T">Generic to ensure that types can only access their properties</typeparam>
    public interface IPropertyInfoList
    {
        IPropertyInfo GetPropertyInfo(string name);
        IEnumerable<IPropertyInfo> Properties();
        bool HasProperty(string propertyName);
    }

    /// <summary>
    /// REGISTERED IN THE DI CONTAINER
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPropertyInfoBag<T> : IPropertyInfoList
    {

    }
}
