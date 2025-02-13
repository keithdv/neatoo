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
    Child = 32,
    Read = 64,
    Write = 128
}

public enum DataMapperMethod
{
    Create = DataMapperMethodType.Create | DataMapperMethodType.Read,
    CreateChild = DataMapperMethodType.Create | DataMapperMethodType.Read | DataMapperMethodType.Child,
    Fetch = DataMapperMethodType.Fetch | DataMapperMethodType.Read, 
    FetchChild = DataMapperMethodType.Fetch | DataMapperMethodType.Read | DataMapperMethodType.Child,
    Insert = DataMapperMethodType.Insert | DataMapperMethodType.Write, 
    InsertChild = DataMapperMethodType.Insert | DataMapperMethodType.Write | DataMapperMethodType.Child,
    Update = DataMapperMethodType.Update | DataMapperMethodType.Write, 
    UpdateChild = DataMapperMethodType.Update | DataMapperMethodType.Write | DataMapperMethodType.Child,
    Delete = DataMapperMethodType.Delete | DataMapperMethodType.Write, 
    DeleteChild = DataMapperMethodType.Delete | DataMapperMethodType.Write | DataMapperMethodType.Child,
}

public static class DataMapperExtension
{
    public static DataMapperMethod? GetDataMapperOperation(this IEditMetaProperties target)
    {
        DataMapperMethodType operationType;

        if (target.IsDeleted)
        {
            if (target.IsNew)
            {
                return null;
            }
            operationType = DataMapperMethodType.Delete;
        }
        else if (target.IsNew)
        {
            operationType = DataMapperMethodType.Insert;
        }
        else
        {
            operationType = DataMapperMethodType.Update;
        }

        DataMapperMethod operation;

        if (target.IsChild)
        {
            operation = (DataMapperMethod)(operationType | DataMapperMethodType.Write | DataMapperMethodType.Child);
        }
        else
        {
            operation = (DataMapperMethod)(operationType | DataMapperMethodType.Write);
        }

        return operation;
    }

    public static AuthorizationRules.AuthorizeOperation ToAuthorizationOperation(this DataMapperMethod operation)
    {
        switch (operation)
        {
            case DataMapperMethod.Create:
            case DataMapperMethod.CreateChild:
                return AuthorizationRules.AuthorizeOperation.Create;
            case DataMapperMethod.Fetch:
            case DataMapperMethod.FetchChild:
                return AuthorizationRules.AuthorizeOperation.Fetch;
            case DataMapperMethod.Insert:
            case DataMapperMethod.InsertChild:
            case DataMapperMethod.Update:
            case DataMapperMethod.UpdateChild:
                return AuthorizationRules.AuthorizeOperation.Update;
            case DataMapperMethod.Delete:
            case DataMapperMethod.DeleteChild:
                return AuthorizationRules.AuthorizeOperation.Delete;
            default:
                break;
        }

        throw new Exception($"{operation.ToString()} cannot be converted to AuthorizationOperation");
    }
}
