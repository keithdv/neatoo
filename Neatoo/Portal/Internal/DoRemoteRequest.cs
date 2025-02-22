using Neatoo.Portal;
using Neatoo.Portal.Internal;
using System.Net.Http.Json;

namespace Neatoo.Portal;

public delegate Task<RemoteResponseDto> SendRemoteRequestToServer(RemoteRequestDto request);

public interface IDoRemoteRequest
{
    Task<T?> ForDelegate<T>(Type delegateType, object[]? parameters);
}

internal class DoRemoteRequest : IDoRemoteRequest
{
    private readonly INeatooJsonSerializer NeatooJsonSerializer;
    private readonly SendRemoteRequestToServer SendRemoteRequestToServer;

    public DoRemoteRequest(INeatooJsonSerializer neatooJsonSerializer, SendRemoteRequestToServer sendRemoteRequestToServer)
    {
        NeatooJsonSerializer = neatooJsonSerializer;
        SendRemoteRequestToServer = sendRemoteRequestToServer;
    }

    public async Task<T?> ForDelegate<T>(Type delegateType, object[]? parameters)
    {
        var remoteRequest = NeatooJsonSerializer.ToRemoteRequest(delegateType, parameters);

        var result = await SendRemoteRequestToServer(remoteRequest);
        
        if(result == null)
        {
            return default;
        }

        return NeatooJsonSerializer.DeserializeRemoteResponse<T>(result);
    }
}