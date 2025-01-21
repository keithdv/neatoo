using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Neatoo.Portal.Core
{

    public interface IClientReadPortal<T> : IReadPortal<T>, IReadPortalChild<T>
    {
    }

    public interface IClientReadWritePortal<T> : IReadWritePortal<T>, IReadWritePortalChild<T>
        where T : IEditMetaProperties
    {
    }


    public delegate Task<PortalResponse> RequestFromServerDelegate(PortalRequest request);

    public class ClientReadPortal<T> : IClientReadPortal<T>
    {
        private readonly IPortalJsonSerializer portalJsonSerializer;
        private readonly RequestFromServerDelegate requestFromServerDelegate;

        public ClientReadPortal(IPortalJsonSerializer portalJsonSerializer, RequestFromServerDelegate requestFromServerDelegate)
        {
            this.portalJsonSerializer = portalJsonSerializer;
            this.requestFromServerDelegate = requestFromServerDelegate;
        }

        public async Task<T> Create()
        {
            return await RequestFromServer(PortalOperation.Create);
        }
        public async Task<T> Fetch()
        {
            return await RequestFromServer(PortalOperation.Fetch);
        }
        public async Task<T> CreateChild()
        {
            return await RequestFromServer(PortalOperation.CreateChild);
        }
        public async Task<T> FetchChild()
        {
            return await RequestFromServer(PortalOperation.FetchChild);
        }

        protected Task<T> RequestFromServer(PortalOperation portalOperation)
        {
            var portalRequest = new PortalRequest()
            {
                PortalOperation = portalOperation,
                Target = new ObjectTypeJson() { AssemblyType = typeof(T).FullName }
            };

            return RequestFromServer(portalRequest);
        }



        protected Task<T> RequestFromServer(PortalOperation portalOperation, object[] criteria)
        {
            var portalRequest = new PortalRequest()
            {
                PortalOperation = portalOperation,
                Target = portalJsonSerializer.ToObjectTypeJson<T>(),
                Criteria = criteria.Select(x => portalJsonSerializer.ToObjectTypeJson(x)).ToList()
            };

            return RequestFromServer(portalRequest);
        }

        protected Task<T> RequestFromServer(PortalOperation portalOperation, T target)
        {
            var portalRequest = new PortalRequest()
            {
                PortalOperation = portalOperation,
                Target = portalJsonSerializer.ToObjectTypeJson(target)
            };

            return RequestFromServer(portalRequest);
        }

        protected Task<T> RequestFromServer(PortalOperation portalOperation, T target, object[] criteria)
        {
            var portalRequest = new PortalRequest()
            {
                PortalOperation = portalOperation,
                Target = portalJsonSerializer.ToObjectTypeJson(target),
                Criteria = criteria.Select(x => portalJsonSerializer.ToObjectTypeJson(x)).ToList()
            };

            return RequestFromServer(portalRequest);
        }

        protected async Task<T> RequestFromServer(PortalRequest request)
        {
            var result = await this.requestFromServerDelegate(request);

            return portalJsonSerializer.Deserialize<T>(result.ObjectJson);
        }

        public Task<T> Create(object[] criteria)
        {
            return RequestFromServer(PortalOperation.Create, criteria);
        }

        public Task<T> Fetch(object[] criteria)
        {
            return RequestFromServer(PortalOperation.Fetch, criteria);
        }

        public Task<T> CreateChild(object[] criteria0)
        {
            return RequestFromServer(PortalOperation.CreateChild, criteria0);
        }

        public Task<T> FetchChild(object[] criteria0)
        {
            return RequestFromServer(PortalOperation.FetchChild, criteria0);
        }
    }

    public class ClientReadWritePortal<T> : ClientReadPortal<T>, IClientReadWritePortal<T>
        where T : IEditMetaProperties
    {
        public ClientReadWritePortal(IPortalJsonSerializer portalJsonSerializer, RequestFromServerDelegate requestFromServerDelegate) : base(portalJsonSerializer, requestFromServerDelegate)
        {
        }

        protected PortalOperation GetOperation(T target)
        {
            if (target.IsChild)
            {
                if (target.IsNew)
                {
                    return PortalOperation.InsertChild;
                }
                else
                {
                    return PortalOperation.UpdateChild;
                }
            }
            else
            {
                if (target.IsNew)
                {
                    return PortalOperation.Insert;
                }
                else
                {
                    return PortalOperation.Update;
                }
            }
        }

        public Task<T> Update(T target)
        {
            return RequestFromServer(GetOperation(target), target);
        }
        public Task<T> UpdateChild(T target)
        {
            return RequestFromServer(GetOperation(target), target);
        }
        public Task<T> Update(T target, params object[] criteria)
        {
            return RequestFromServer(GetOperation(target), target, criteria);
        }
        public Task<T> UpdateChild(T target, params object[] criteria)
        {
            return RequestFromServer(GetOperation(target), target, criteria);
        }
    }
}
