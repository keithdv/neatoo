using Neatoo.Portal;
using Neatoo.Portal.Internal;
using System.Net.Http.Json;

namespace Neatoo.Portal;

public delegate Task<object?> DoRemoteRequest(Type delegateType, object[]? parameters);
public delegate Task<RemoteResponseDto> SendRemoteRequestToServer(RemoteRequestDto request);

internal static class RemoteMethod
{
    public static async Task<object?> DoRemoteRequest(Type delegateType, object[]? parameters, INeatooJsonSerializer neatooJsonSerializer, SendRemoteRequestToServer sendRemoteRequestToServer)
    {
        var remoteRequest = neatooJsonSerializer.ToRemoteRequest(delegateType, parameters);

        var result = await sendRemoteRequestToServer(remoteRequest);

        return neatooJsonSerializer.DeserializeRemoteResponse(result);
    }
}