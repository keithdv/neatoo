using System;
using System.Reflection;

namespace Neatoo.Core
{

    public delegate IPropertyInfo CreatePropertyInfoWrapper(PropertyInfo property);

    public class NeatooPropertyInfoWrapper : IPropertyInfo
    {

        public NeatooPropertyInfoWrapper(PropertyInfo propertyInfo)
        {
            this.PropertyInfo = propertyInfo;
        }

        public PropertyInfo PropertyInfo { get; }

        public string Name => PropertyInfo.Name;

        public Type Type => PropertyInfo.PropertyType;
        public string Key => Name;

    }
}
