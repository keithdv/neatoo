using Neatoo.AuthorizationRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Neatoo.Portal.Core
{


    public class LocalReadPortal<T> : Portal<T>, IReadPortal<T>, IReadPortalChild<T>
    {
        public LocalReadPortal(IServiceScope scope)
            : base(scope)
        {
        }

        public Task<T> Create()
        {
            return CallReadOperationMethod(PortalOperation.Create, false);
        }
        public Task<T> Fetch()
        {
            return CallReadOperationMethod(PortalOperation.Fetch, true);
        }
        public Task<T> CreateChild()
        {
            return CallReadOperationMethod(PortalOperation.CreateChild, false);
        }
        public Task<T> FetchChild()
        {
            return CallReadOperationMethod(PortalOperation.FetchChild, true);
        }

        public Task<T> Create(object[] criteria)
        {
            return CallReadOperationMethod(PortalOperation.Create, criteria);
        }

        public Task<T> Fetch(object[] criteria)
        {
            return CallReadOperationMethod(PortalOperation.Fetch, criteria);
        }

        public Task<T> CreateChild(object[] criteria)
        {
            return CallReadOperationMethod(PortalOperation.CreateChild, criteria);
        }

        public Task<T> FetchChild(object[] criteria)
        {
            return CallReadOperationMethod(PortalOperation.FetchChild, criteria);
        }
    }


    public class LocalReadWritePortal<T> : LocalReadPortal<T>, IReadWritePortal<T>, IReadWritePortalChild<T>
        where T : IEditMetaProperties
    {

        public LocalReadWritePortal(IServiceScope scope)
            : base(scope)
        {
        }

        public async Task Update(T target)
        {
            await CallWriteOperationMethod(target);
        }

        public async Task UpdateChild(T target)
        {
            await CallWriteChildOperationMethod(target);
        }
    }



    [Serializable]
    public class OperationMethodCallFailedException : Exception
    {
        public OperationMethodCallFailedException() { }
        public OperationMethodCallFailedException(string message) : base(message) { }
        public OperationMethodCallFailedException(string message, Exception inner) : base(message, inner) { }
    }

}
