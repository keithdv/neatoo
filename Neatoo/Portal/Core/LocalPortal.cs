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
        where T : IPortalTarget
    {
        public LocalReadPortal(IServiceScope scope)
            : base(scope)
        {
        }

        public async Task<T> Create()
        {
            return await CallReadOperationMethod(PortalOperation.Create, false);
        }
        public async Task<T> Create<C0>(C0 criteria0)
        {
            return await CallReadOperationMethod(PortalOperation.Create, new object[] { criteria0 }, new Type[] { typeof(C0) });
        }
        public async Task<T> Create<C0, C1>(C0 criteria0, C1 criteria1)
        {
            return await CallReadOperationMethod(PortalOperation.Create, new object[] { criteria0, criteria1 }, new Type[] { typeof(C0), typeof(C1) });
        }
        public async Task<T> Create<C0, C1, C2>(C0 criteria0, C1 criteria1, C2 criteria2)
        {
            return await CallReadOperationMethod(PortalOperation.Create, new object[] { criteria0, criteria1, criteria2 }, new Type[] { typeof(C0), typeof(C1), typeof(C2) });
        }
        public async Task<T> Create<C0, C1, C2, C3>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3)
        {
            return await CallReadOperationMethod(PortalOperation.Create, new object[] { criteria0, criteria1, criteria2, criteria3 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3) });
        }
        public async Task<T> Create<C0, C1, C2, C3, C4>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4)
        {
            return await CallReadOperationMethod(PortalOperation.Create, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4) });
        }
        public async Task<T> Create<C0, C1, C2, C3, C4, C5>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5)
        {
            return await CallReadOperationMethod(PortalOperation.Create, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5) });
        }
        public async Task<T> Create<C0, C1, C2, C3, C4, C5, C6>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6)
        {
            return await CallReadOperationMethod(PortalOperation.Create, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6) });
        }
        public async Task<T> Create<C0, C1, C2, C3, C4, C5, C6, C7>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6, C7 criteria7)
        {
            return await CallReadOperationMethod(PortalOperation.Create, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6, criteria7 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6), typeof(C7) });
        }
        public async Task<T> Fetch()
        {
            return await CallReadOperationMethod(PortalOperation.Fetch, true);
        }
        public async Task<T> Fetch<C0>(C0 criteria0)
        {
            return await CallReadOperationMethod(PortalOperation.Fetch, new object[] { criteria0 }, new Type[] { typeof(C0) });
        }
        public async Task<T> Fetch<C0, C1>(C0 criteria0, C1 criteria1)
        {
            return await CallReadOperationMethod(PortalOperation.Fetch, new object[] { criteria0, criteria1 }, new Type[] { typeof(C0), typeof(C1) });
        }
        public async Task<T> Fetch<C0, C1, C2>(C0 criteria0, C1 criteria1, C2 criteria2)
        {
            return await CallReadOperationMethod(PortalOperation.Fetch, new object[] { criteria0, criteria1, criteria2 }, new Type[] { typeof(C0), typeof(C1), typeof(C2) });
        }
        public async Task<T> Fetch<C0, C1, C2, C3>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3)
        {
            return await CallReadOperationMethod(PortalOperation.Fetch, new object[] { criteria0, criteria1, criteria2, criteria3 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3) });
        }
        public async Task<T> Fetch<C0, C1, C2, C3, C4>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4)
        {
            return await CallReadOperationMethod(PortalOperation.Fetch, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4) });
        }
        public async Task<T> Fetch<C0, C1, C2, C3, C4, C5>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5)
        {
            return await CallReadOperationMethod(PortalOperation.Fetch, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5) });
        }
        public async Task<T> Fetch<C0, C1, C2, C3, C4, C5, C6>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6)
        {
            return await CallReadOperationMethod(PortalOperation.Fetch, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6) });
        }
        public async Task<T> Fetch<C0, C1, C2, C3, C4, C5, C6, C7>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6, C7 criteria7)
        {
            return await CallReadOperationMethod(PortalOperation.Fetch, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6, criteria7 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6), typeof(C7) });
        }
        public async Task<T> CreateChild()
        {
            return await CallReadOperationMethod(PortalOperation.CreateChild, false);
        }
        public async Task<T> CreateChild<C0>(C0 criteria0)
        {
            return await CallReadOperationMethod(PortalOperation.CreateChild, new object[] { criteria0 }, new Type[] { typeof(C0) });
        }
        public async Task<T> CreateChild<C0, C1>(C0 criteria0, C1 criteria1)
        {
            return await CallReadOperationMethod(PortalOperation.CreateChild, new object[] { criteria0, criteria1 }, new Type[] { typeof(C0), typeof(C1) });
        }
        public async Task<T> CreateChild<C0, C1, C2>(C0 criteria0, C1 criteria1, C2 criteria2)
        {
            return await CallReadOperationMethod(PortalOperation.CreateChild, new object[] { criteria0, criteria1, criteria2 }, new Type[] { typeof(C0), typeof(C1), typeof(C2) });
        }
        public async Task<T> CreateChild<C0, C1, C2, C3>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3)
        {
            return await CallReadOperationMethod(PortalOperation.CreateChild, new object[] { criteria0, criteria1, criteria2, criteria3 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3) });
        }
        public async Task<T> CreateChild<C0, C1, C2, C3, C4>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4)
        {
            return await CallReadOperationMethod(PortalOperation.CreateChild, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4) });
        }
        public async Task<T> CreateChild<C0, C1, C2, C3, C4, C5>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5)
        {
            return await CallReadOperationMethod(PortalOperation.CreateChild, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5) });
        }
        public async Task<T> CreateChild<C0, C1, C2, C3, C4, C5, C6>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6)
        {
            return await CallReadOperationMethod(PortalOperation.CreateChild, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6) });
        }
        public async Task<T> CreateChild<C0, C1, C2, C3, C4, C5, C6, C7>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6, C7 criteria7)
        {
            return await CallReadOperationMethod(PortalOperation.CreateChild, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6, criteria7 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6), typeof(C7) });
        }
        public async Task<T> FetchChild()
        {
            return await CallReadOperationMethod(PortalOperation.FetchChild, true);
        }
        public async Task<T> FetchChild<C0>(C0 criteria0)
        {
            return await CallReadOperationMethod(PortalOperation.FetchChild, new object[] { criteria0 }, new Type[] { typeof(C0) });
        }
        public async Task<T> FetchChild<C0, C1>(C0 criteria0, C1 criteria1)
        {
            return await CallReadOperationMethod(PortalOperation.FetchChild, new object[] { criteria0, criteria1 }, new Type[] { typeof(C0), typeof(C1) });
        }
        public async Task<T> FetchChild<C0, C1, C2>(C0 criteria0, C1 criteria1, C2 criteria2)
        {
            return await CallReadOperationMethod(PortalOperation.FetchChild, new object[] { criteria0, criteria1, criteria2 }, new Type[] { typeof(C0), typeof(C1), typeof(C2) });
        }
        public async Task<T> FetchChild<C0, C1, C2, C3>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3)
        {
            return await CallReadOperationMethod(PortalOperation.FetchChild, new object[] { criteria0, criteria1, criteria2, criteria3 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3) });
        }
        public async Task<T> FetchChild<C0, C1, C2, C3, C4>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4)
        {
            return await CallReadOperationMethod(PortalOperation.FetchChild, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4) });
        }
        public async Task<T> FetchChild<C0, C1, C2, C3, C4, C5>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5)
        {
            return await CallReadOperationMethod(PortalOperation.FetchChild, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5) });
        }
        public async Task<T> FetchChild<C0, C1, C2, C3, C4, C5, C6>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6)
        {
            return await CallReadOperationMethod(PortalOperation.FetchChild, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6) });
        }
        public async Task<T> FetchChild<C0, C1, C2, C3, C4, C5, C6, C7>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6, C7 criteria7)
        {
            return await CallReadOperationMethod(PortalOperation.FetchChild, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6, criteria7 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6), typeof(C7) });
        }


    }


    public class LocalReadWritePortal<T> : LocalReadPortal<T>, IReadWritePortal<T>, IReadWritePortalChild<T>
        where T : IPortalEditTarget, IEditMetaProperties
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
        protected OperationMethodCallFailedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

}
