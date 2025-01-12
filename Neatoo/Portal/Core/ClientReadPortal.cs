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
        where T : IPortalTarget
    {
    }

    public interface IClientReadWritePortal<T> : IReadWritePortal<T>, IReadWritePortalChild<T>
        where T : IPortalEditTarget, IEditMetaProperties
    {
    }

    public class ClientReadPortal<T> : IClientReadPortal<T>
        where T : IPortalTarget
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
        public async Task<T> Create<C0>(C0 criteria0)
        {
            return await RequestFromServer(PortalOperation.Create, new object[] { criteria0 }, new Type[] { typeof(C0) });
        }
        public async Task<T> Create<C0, C1>(C0 criteria0, C1 criteria1)
        {
            return await RequestFromServer(PortalOperation.Create, new object[] { criteria0, criteria1 }, new Type[] { typeof(C0), typeof(C1) });
        }
        public async Task<T> Create<C0, C1, C2>(C0 criteria0, C1 criteria1, C2 criteria2)
        {
            return await RequestFromServer(PortalOperation.Create, new object[] { criteria0, criteria1, criteria2 }, new Type[] { typeof(C0), typeof(C1), typeof(C2) });
        }
        public async Task<T> Create<C0, C1, C2, C3>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3)
        {
            return await RequestFromServer(PortalOperation.Create, new object[] { criteria0, criteria1, criteria2, criteria3 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3) });
        }
        public async Task<T> Create<C0, C1, C2, C3, C4>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4)
        {
            return await RequestFromServer(PortalOperation.Create, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4) });
        }
        public async Task<T> Create<C0, C1, C2, C3, C4, C5>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5)
        {
            return await RequestFromServer(PortalOperation.Create, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5) });
        }
        public async Task<T> Create<C0, C1, C2, C3, C4, C5, C6>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6)
        {
            return await RequestFromServer(PortalOperation.Create, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6) });
        }
        public async Task<T> Create<C0, C1, C2, C3, C4, C5, C6, C7>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6, C7 criteria7)
        {
            return await RequestFromServer(PortalOperation.Create, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6, criteria7 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6), typeof(C7) });
        }
        public async Task<T> Fetch()
        {
            return await RequestFromServer(PortalOperation.Fetch);
        }
        public async Task<T> Fetch<C0>(C0 criteria0)
        {
            return await RequestFromServer(PortalOperation.Fetch, new object[] { criteria0 }, new Type[] { typeof(C0) });
        }
        public async Task<T> Fetch<C0, C1>(C0 criteria0, C1 criteria1)
        {
            return await RequestFromServer(PortalOperation.Fetch, new object[] { criteria0, criteria1 }, new Type[] { typeof(C0), typeof(C1) });
        }
        public async Task<T> Fetch<C0, C1, C2>(C0 criteria0, C1 criteria1, C2 criteria2)
        {
            return await RequestFromServer(PortalOperation.Fetch, new object[] { criteria0, criteria1, criteria2 }, new Type[] { typeof(C0), typeof(C1), typeof(C2) });
        }
        public async Task<T> Fetch<C0, C1, C2, C3>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3)
        {
            return await RequestFromServer(PortalOperation.Fetch, new object[] { criteria0, criteria1, criteria2, criteria3 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3) });
        }
        public async Task<T> Fetch<C0, C1, C2, C3, C4>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4)
        {
            return await RequestFromServer(PortalOperation.Fetch, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4) });
        }
        public async Task<T> Fetch<C0, C1, C2, C3, C4, C5>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5)
        {
            return await RequestFromServer(PortalOperation.Fetch, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5) });
        }
        public async Task<T> Fetch<C0, C1, C2, C3, C4, C5, C6>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6)
        {
            return await RequestFromServer(PortalOperation.Fetch, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6) });
        }
        public async Task<T> Fetch<C0, C1, C2, C3, C4, C5, C6, C7>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6, C7 criteria7)
        {
            return await RequestFromServer(PortalOperation.Fetch, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6, criteria7 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6), typeof(C7) });
        }
        public async Task<T> CreateChild()
        {
            return await RequestFromServer(PortalOperation.CreateChild);
        }
        public async Task<T> CreateChild<C0>(C0 criteria0)
        {
            return await RequestFromServer(PortalOperation.CreateChild, new object[] { criteria0 }, new Type[] { typeof(C0) });
        }
        public async Task<T> CreateChild<C0, C1>(C0 criteria0, C1 criteria1)
        {
            return await RequestFromServer(PortalOperation.CreateChild, new object[] { criteria0, criteria1 }, new Type[] { typeof(C0), typeof(C1) });
        }
        public async Task<T> CreateChild<C0, C1, C2>(C0 criteria0, C1 criteria1, C2 criteria2)
        {
            return await RequestFromServer(PortalOperation.CreateChild, new object[] { criteria0, criteria1, criteria2 }, new Type[] { typeof(C0), typeof(C1), typeof(C2) });
        }
        public async Task<T> CreateChild<C0, C1, C2, C3>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3)
        {
            return await RequestFromServer(PortalOperation.CreateChild, new object[] { criteria0, criteria1, criteria2, criteria3 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3) });
        }
        public async Task<T> CreateChild<C0, C1, C2, C3, C4>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4)
        {
            return await RequestFromServer(PortalOperation.CreateChild, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4) });
        }
        public async Task<T> CreateChild<C0, C1, C2, C3, C4, C5>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5)
        {
            return await RequestFromServer(PortalOperation.CreateChild, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5) });
        }
        public async Task<T> CreateChild<C0, C1, C2, C3, C4, C5, C6>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6)
        {
            return await RequestFromServer(PortalOperation.CreateChild, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6) });
        }
        public async Task<T> CreateChild<C0, C1, C2, C3, C4, C5, C6, C7>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6, C7 criteria7)
        {
            return await RequestFromServer(PortalOperation.CreateChild, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6, criteria7 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6), typeof(C7) });
        }
        public async Task<T> FetchChild()
        {
            return await RequestFromServer(PortalOperation.FetchChild);
        }
        public async Task<T> FetchChild<C0>(C0 criteria0)
        {
            return await RequestFromServer(PortalOperation.FetchChild, new object[] { criteria0 }, new Type[] { typeof(C0) });
        }
        public async Task<T> FetchChild<C0, C1>(C0 criteria0, C1 criteria1)
        {
            return await RequestFromServer(PortalOperation.FetchChild, new object[] { criteria0, criteria1 }, new Type[] { typeof(C0), typeof(C1) });
        }
        public async Task<T> FetchChild<C0, C1, C2>(C0 criteria0, C1 criteria1, C2 criteria2)
        {
            return await RequestFromServer(PortalOperation.FetchChild, new object[] { criteria0, criteria1, criteria2 }, new Type[] { typeof(C0), typeof(C1), typeof(C2) });
        }
        public async Task<T> FetchChild<C0, C1, C2, C3>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3)
        {
            return await RequestFromServer(PortalOperation.FetchChild, new object[] { criteria0, criteria1, criteria2, criteria3 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3) });
        }
        public async Task<T> FetchChild<C0, C1, C2, C3, C4>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4)
        {
            return await RequestFromServer(PortalOperation.FetchChild, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4) });
        }
        public async Task<T> FetchChild<C0, C1, C2, C3, C4, C5>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5)
        {
            return await RequestFromServer(PortalOperation.FetchChild, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5) });
        }
        public async Task<T> FetchChild<C0, C1, C2, C3, C4, C5, C6>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6)
        {
            return await RequestFromServer(PortalOperation.FetchChild, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6) });
        }
        public async Task<T> FetchChild<C0, C1, C2, C3, C4, C5, C6, C7>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6, C7 criteria7)
        {
            return await RequestFromServer(PortalOperation.FetchChild, new object[] { criteria0, criteria1, criteria2, criteria3, criteria4, criteria5, criteria6, criteria7 }, new Type[] { typeof(C0), typeof(C1), typeof(C2), typeof(C3), typeof(C4), typeof(C5), typeof(C6), typeof(C7) });
        }

        protected Task<T> RequestFromServer(PortalOperation portalOperation)
        {
            var portalRequest = new PortalRequest()
            {
                PortalOperation = portalOperation,
                Type = typeof(T).AssemblyQualifiedName
            };

            return RequestFromServer(portalRequest);
        }

        protected Task<T> RequestFromServer(PortalOperation portalOperation, T target)
        {
            var portalRequest = new PortalRequest()
            {
                PortalOperation = portalOperation,
                ObjectJson = portalJsonSerializer.Serialize(target),
                Type = typeof(T).AssemblyQualifiedName
            };

            return RequestFromServer(portalRequest);
        }

        protected Task<T> RequestFromServer(PortalOperation portalOperation, object[] criteria, Type[] criteriaTypes)
        {
            var portalRequest = new PortalRequest()
            {
                PortalOperation = portalOperation,
                Type = typeof(T).AssemblyQualifiedName,
                CriteriaJson = portalJsonSerializer.Serialize(criteria.Select(x => portalJsonSerializer.Serialize(x)).ToList()),
                CriteriaTypes = portalJsonSerializer.Serialize(criteriaTypes.Select(x => x.AssemblyQualifiedName).ToList())
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
    }

    public class ClientReadWritePortal<T> : ClientReadPortal<T>, IClientReadWritePortal<T>
        where T : IPortalEditTarget, IEditMetaProperties
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
