
using Neatoo.Portal;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public interface IRemoteMethodClient
{
    Type DelegateType { get; set; }
}

public class RemoteMethodClient<P, T> : IRemoteMethodClient
{
    private readonly HttpClient httpClient;
    private readonly INeatooJsonSerializer neatooJsonSerializer;

    public RemoteMethodClient(HttpClient httpClient, INeatooJsonSerializer neatooJsonSerializer)
    {
        this.httpClient = httpClient;
        this.neatooJsonSerializer = neatooJsonSerializer;
    }

    public Type DelegateType { get; set; }

    public async Task<T> Call(P p)
    {
        var portalRequest = neatooJsonSerializer.ToRemoteRequest(Neatoo.Portal.DataMapperMethod.Execute, DelegateType, [p]);

        var response = await httpClient.PostAsync("portal", JsonContent.Create(portalRequest, typeof(RemoteRequest)));

        if (!response.IsSuccessStatusCode)
        {
            var issue = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Failed to call portal. Status code: {response.StatusCode} {issue}");
        }

        var result = await response.Content.ReadFromJsonAsync<RemoteResponse>() ?? throw new HttpRequestException($"Successful Code but empty response."); ;

        return (T)neatooJsonSerializer.FromPortalResponse(result);
    }
}