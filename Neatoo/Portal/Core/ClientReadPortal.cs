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

    public class ClientReadPortal<T> : IClientReadPortal<T>
    {
        private readonly HttpClient httpClient;
        private readonly IPortalJsonSerializer portalJsonSerializer;

        public ClientReadPortal(HttpClient httpClient, IPortalJsonSerializer portalJsonSerializer)
        {
            this.httpClient = httpClient;
            this.portalJsonSerializer = portalJsonSerializer;
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

        protected Task<T> RequestFromServer(PortalOperation portalOperation, T target)
        {
            var portalRequest = new PortalRequest()
            {
                PortalOperation = portalOperation,
                Target = portalJsonSerializer.ToObjectTypeJson(target)
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

        protected async Task<T> RequestFromServer(PortalRequest request)
        {
            var response = await httpClient.PostAsync("http://localhost:5037/portal", new StringContent(portalJsonSerializer.Serialize(request), Encoding.UTF8, MediaTypeNames.Application.Json));

            if (!response.IsSuccessStatusCode)
            {
                var issue = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to call portal. Status code: {response.StatusCode} {issue}");
            }

            var result = portalJsonSerializer.Deserialize<PortalResponse>(await response.Content.ReadAsStringAsync());

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
        public ClientReadWritePortal(HttpClient httpClient, IPortalJsonSerializer portalJsonSerializer) : base(httpClient, portalJsonSerializer)
        {
        }

        public Task Update(T target)
        {
            return RequestFromServer(PortalOperation.Update, target);
        }
        public Task UpdateChild(T target)
        {
            return RequestFromServer(PortalOperation.UpdateChild, target);
        }
    }
}
