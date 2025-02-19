using Neatoo.Internal;
using System;
using System.Threading.Tasks;

namespace Neatoo.Portal;

public enum DataMapperMethodType
{
    Create = 1,
    Fetch = 2,
    Insert = 4,
    Update = 8,
    Delete = 16,
    Read = 64,
    Write = 128
}

public enum DataMapperMethod
{
    Execute = DataMapperMethodType.Read,
    Create = DataMapperMethodType.Create | DataMapperMethodType.Read,
    Fetch = DataMapperMethodType.Fetch | DataMapperMethodType.Read, 
    Insert = DataMapperMethodType.Insert | DataMapperMethodType.Write, 
    Update = DataMapperMethodType.Update | DataMapperMethodType.Write, 
    Delete = DataMapperMethodType.Delete | DataMapperMethodType.Write, 
}

public static class DataMapperExtension
{
    public static AuthorizationRules.AuthorizeOperation ToAuthorizationOperation(this DataMapperMethod operation)
    {
        switch (operation)
        {
            case DataMapperMethod.Create:
                return AuthorizationRules.AuthorizeOperation.Create;
            case DataMapperMethod.Fetch:
                return AuthorizationRules.AuthorizeOperation.Fetch;
            case DataMapperMethod.Insert:
            case DataMapperMethod.Update:
                return AuthorizationRules.AuthorizeOperation.Update;
            case DataMapperMethod.Delete:
                return AuthorizationRules.AuthorizeOperation.Delete;
            case DataMapperMethod.Execute:
                return AuthorizationRules.AuthorizeOperation.Execute;
            default:
                break;
        }

        throw new Exception($"{operation.ToString()} cannot be converted to AuthorizationOperation");
    }
}
