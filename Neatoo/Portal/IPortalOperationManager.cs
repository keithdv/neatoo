using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Neatoo.Portal
{

    public interface IPortalOperationManager
    {
        void RegisterOperation(PortalOperation operation, string methodName);
        void RegisterOperation(PortalOperation operation, MethodInfo method);
        Task<bool> TryCallOperation(IPortalTarget target, PortalOperation operation);
        Task<bool> TryCallOperation(IPortalTarget target, PortalOperation operation, object[] criteria, Type[] criteriaTypes);
        Task<IPortalTarget> HandlePortalRequest(PortalRequest portalRequest);
    }
    public interface IPortalOperationManager<T> : IPortalOperationManager
    {


    }
}