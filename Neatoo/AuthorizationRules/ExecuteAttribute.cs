using Neatoo.Portal;
using System;

namespace Neatoo.AuthorizationRules;


[System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class AuthorizeAttribute<T> : Attribute
{
    public AuthorizeAttribute()
    {

    }
}

[System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class AuthorizeAttribute : Attribute
{
    public DataMapperMethodType DataMapperMethodType { get; }
    public AuthorizeAttribute(DataMapperMethodType operation)
    {
        this.DataMapperMethodType = operation;
    }
}
