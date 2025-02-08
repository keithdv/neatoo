using System;

namespace Neatoo.Portal
{
    public enum PortalOperationType
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

    public enum PortalOperation
    {
        Create = PortalOperationType.Create | PortalOperationType.Read,
        CreateChild = PortalOperationType.Create | PortalOperationType.Read | PortalOperationType.Child,
        Fetch = PortalOperationType.Fetch | PortalOperationType.Read, 
        FetchChild = PortalOperationType.Fetch | PortalOperationType.Read | PortalOperationType.Child,
        Insert = PortalOperationType.Insert | PortalOperationType.Write, 
        InsertChild = PortalOperationType.Insert | PortalOperationType.Write | PortalOperationType.Child,
        Update = PortalOperationType.Update | PortalOperationType.Write, 
        UpdateChild = PortalOperationType.Update | PortalOperationType.Write | PortalOperationType.Child,
        Delete = PortalOperationType.Delete | PortalOperationType.Write, 
        DeleteChild = PortalOperationType.Delete | PortalOperationType.Write | PortalOperationType.Child,
    }

    public static class PortalOperationExtension
    {
        public static AuthorizationRules.AuthorizeOperation ToAuthorizationOperation(this PortalOperation operation)
        {
            switch (operation)
            {
                case PortalOperation.Create:
                case PortalOperation.CreateChild:
                    return AuthorizationRules.AuthorizeOperation.Create;
                case PortalOperation.Fetch:
                case PortalOperation.FetchChild:
                    return AuthorizationRules.AuthorizeOperation.Fetch;
                case PortalOperation.Insert:
                case PortalOperation.InsertChild:
                case PortalOperation.Update:
                case PortalOperation.UpdateChild:
                    return AuthorizationRules.AuthorizeOperation.Update;
                case PortalOperation.Delete:
                case PortalOperation.DeleteChild:
                    return AuthorizationRules.AuthorizeOperation.Delete;
                default:
                    break;
            }

            throw new Exception($"{operation.ToString()} cannot be converted to AuthorizationOperation");

        }
    }

}
