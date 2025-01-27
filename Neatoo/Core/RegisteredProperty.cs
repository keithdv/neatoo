using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Neatoo.Core
{

    public delegate IRegisteredProperty CreateRegisteredProperty(PropertyInfo property);

    public class RegisteredProperty : IRegisteredProperty
    {

        public RegisteredProperty(PropertyInfo propertyInfo)
        {
            this.PropertyInfo = propertyInfo;
        }

        public PropertyInfo PropertyInfo { get; }

        public string Name => PropertyInfo.Name;

        public Type Type => PropertyInfo.PropertyType;
        public string Key => Name;

    }
}
