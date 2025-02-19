using Neatoo.Portal;
using System;
using System.Net.Http.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Neatoo.Portal.Internal;

namespace Neatoo;


public abstract class MethodPortal<T> : IRemoteMethodPortal<T>
{

    public MethodPortal()
    {
    }

    public abstract Task<T> Execute();
}


public abstract class MethodPortal<P, T> : IRemoteMethodPortal<P, T>
{

    public MethodPortal(INeatooJsonSerializer neatooJsonSerializer)
    {
        NeatooJsonSerializer = neatooJsonSerializer;
    }

    public INeatooJsonSerializer NeatooJsonSerializer { get; }

    public virtual Task<T> Execute(P parameters)
    {

        throw new NotImplementedException();

        //var response = await httpClient.PostAsync("portal", JsonContent.Create(portalRequest, typeof(RemoteRequest)));

        //if (!response.IsSuccessStatusCode)
        //{
        //    var issue = await response.Content.ReadAsStringAsync();
        //    throw new HttpRequestException($"Failed to call portal. Status code: {response.StatusCode} {issue}");
        //}

        //var result = await response.Content.ReadFromJsonAsync<RemoteResponse>() ?? throw new HttpRequestException($"Successful Code but empty response."); ;

        //return result;

    }
}
