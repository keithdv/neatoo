using System;
using System.Reflection;

namespace Neatoo
{
    public interface IPropertyInfo
    {
        PropertyInfo PropertyInfo { get; }
        string Name { get; }
        Type Type { get; }
        string Key { get; }
    }
}