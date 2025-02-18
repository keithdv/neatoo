
//using Neatoo.Portal;
//using System;
//using System.Net.Http;
//using System.Net.Http.Json;
//using System.Threading.Tasks;

//public interface IRemoteMethodClient
//{
//    Type DelegateType { get; set; }
//}

//public interface IRemoteMethodClient<T> : IRemoteMethodClient
//    where T : notnull
//{
//    Task<T> Call();
//}

//public interface IRemoteMethodClient<P, T> : IRemoteMethodClient
//    where T : notnull
//    where P : notnull
//{
//    Task<T> Call(P p);
//}

//public interface IRemoteMethodClient<P1, P2, T> : IRemoteMethodClient
//    where T : notnull
//    where P1 : notnull
//    where P2 : notnull
//{
//    Task<T> Call(P1 p1, P2 p2);
//}

//public interface IRemoteMethodClient<P1, P2, P3, T> : IRemoteMethodClient
//    where T : notnull
//    where P1 : notnull
//    where P2 : notnull
//    where P3 : notnull
//{
//    Task<T> Call(P1 p1, P2 p2, P3 p3);
//}

//public interface IRemoteMethodClient<P1, P2, P3, P4, T> : IRemoteMethodClient
//    where T : notnull
//    where P1 : notnull
//    where P2 : notnull
//    where P3 : notnull
//    where P4 : notnull
//{
//    Task<T> Call(P1 p1, P2 p2, P3 p3, P4 p4);
//}

//public class RemoteMethodClient<P, T> : RemoteMethodClient<T>, IRemoteMethodClient<P, T>
//    where T : notnull
//    where P : notnull
//{
//    public RemoteMethodClient(HttpClient httpClient, INeatooJsonSerializer neatooJsonSerializer) : base(httpClient, neatooJsonSerializer)
//    {
//    }

//    public Task<T> Call(P p)
//    {
//        return base.Call([p]);
//    }
//}

//public class RemoteMethodClient<P1, P2, T> : RemoteMethodClient<T>, IRemoteMethodClient<P1, P2, T>
//    where T : notnull
//    where P1 : notnull
//    where P2 : notnull
//{
//    public RemoteMethodClient(HttpClient httpClient, INeatooJsonSerializer neatooJsonSerializer) : base(httpClient, neatooJsonSerializer)
//    {
//    }

//    public Task<T> Call(P1 p1, P2 p2)
//    {
//        return base.Call([p1, p2]);
//    }
//}

//public class RemoteMethodClient<P1, P2, P3, T> : RemoteMethodClient<T>, IRemoteMethodClient<P1, P2, P3, T>
//    where T : notnull
//    where P1 : notnull
//    where P2 : notnull
//    where P3 : notnull
//{
//    public RemoteMethodClient(HttpClient httpClient, INeatooJsonSerializer neatooJsonSerializer) : base(httpClient, neatooJsonSerializer)
//    {
//    }

//    public Task<T> Call(P1 p1, P2 p2, P3 p3)
//    {
//        return base.Call([p1, p2, p3]);
//    }
//}


//public class RemoteMethodClient<P1, P2, P3, P4, T> : RemoteMethodClient<T>, IRemoteMethodClient<P1, P2, P3, P4, T>
//    where T : notnull
//    where P1 : notnull
//    where P2 : notnull
//    where P3 : notnull
//    where P4 : notnull
//{
//    public RemoteMethodClient(HttpClient httpClient, INeatooJsonSerializer neatooJsonSerializer) : base(httpClient, neatooJsonSerializer)
//    {
//    }

//    public Task<T> Call(P1 p1, P2 p2, P3 p3, P4 p4)
//    {
//        return base.Call([p1, p2, p3, p4]);
//    }
//}

using Neatoo.Portal;
using Neatoo.Portal.Internal;
using System.Net.Http.Json;

namespace Neatoo.Portal;

public delegate Task<object?> RemoteRead(Type delegateType, object[]? parameters);
//public delegate Task<object?> RemoteSave(Type delegateType, object target, object[]? parameters);

internal static class RemoteMethod
{
    public static async Task<object?> Read(Type delegateType, object[]? parameters, INeatooJsonSerializer neatooJsonSerializer, RequestFromServerDelegate requestFromServer)
    {
        var portalRequest = neatooJsonSerializer.ToRemoteRequest(Neatoo.Portal.DataMapperMethod.Execute, delegateType, parameters);

        var result = await requestFromServer(portalRequest);

        return neatooJsonSerializer.FromPortalResponse(result);
    }

    //public static async Task<object?> Save(Type delegateType, object target, object[]? parameters, INeatooJsonSerializer neatooJsonSerializer, RequestFromServerDelegate requestFromServer)
    //{
    //    var portalRequest = neatooJsonSerializer.ToRemoteRequest(Neatoo.Portal.DataMapperMethod.Execute, delegateType,  target, parameters);

    //    var result = await requestFromServer(portalRequest);

    //    return neatooJsonSerializer.FromPortalResponse(result);
    //}
}

//public class RemoteMethodClient<T> : IRemoteMethodClient<T>
//    where T : notnull
//{
//    private readonly HttpClient httpClient;
//    private readonly INeatooJsonSerializer neatooJsonSerializer;

//    public RemoteMethodClient(HttpClient httpClient, INeatooJsonSerializer neatooJsonSerializer)
//    {
//        this.httpClient = httpClient;
//        this.neatooJsonSerializer = neatooJsonSerializer;
//    }

//    public Type DelegateType { get; set; }

//    public Task<T> Call()
//    {
//        return Call(null);
//    }

//    public async Task<T> Call(object[]? p)
//    {
//        var portalRequest = neatooJsonSerializer.ToRemoteRequest(Neatoo.Portal.DataMapperMethod.Execute, DelegateType, p);

//        var response = await httpClient.PostAsync("portal", JsonContent.Create(portalRequest, typeof(RemoteRequest)));

//        if (!response.IsSuccessStatusCode)
//        {
//            var issue = await response.Content.ReadAsStringAsync();
//            throw new HttpRequestException($"Failed to call portal. Status code: {response.StatusCode} {issue}");
//        }

//        var result = await response.Content.ReadFromJsonAsync<RemoteResponse>() ?? throw new HttpRequestException($"Successful Code but empty response."); ;

//        return (T)neatooJsonSerializer.FromPortalResponse(result);
//    }
//}