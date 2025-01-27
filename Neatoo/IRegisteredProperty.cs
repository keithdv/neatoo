using System;
using System.Reflection;

namespace Neatoo
{
    public interface IRegisteredProperty
    {
        PropertyInfo PropertyInfo { get; }
        string Name { get; }
        Type Type { get; }
        string Key { get; }
    }
}