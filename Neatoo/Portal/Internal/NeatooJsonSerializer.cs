using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Neatoo.Portal.Internal;

public interface INeatooJsonSerializer
{
    string? Serialize(object? target);
    string? Serialize(object? target, Type targetType);
    T? Deserialize<T>(string json);
    object? Deserialize(string json, Type type);
    RemoteRequestDto ToRemoteRequest(Type delegateType, params object[]? parameters);
    RemoteRequestDto ToRemoteRequest(Type delegateType, object saveTarget, params object[]? parameters);
    RemoteRequest DeserializeRemoteRequest(RemoteRequestDto remoteRequest);
    T? DeserializeRemoteResponse<T>(RemoteResponseDto remoteResponse);
}

public class NeatooJsonSerializer : INeatooJsonSerializer
{
    private readonly GetServiceImplementationType getImplementationType;
    private readonly ILocalAssemblies localAssemblies;

    JsonSerializerOptions Options { get; }

    private NeatooReferenceHandler MyReferenceHandler { get; } = new NeatooReferenceHandler();
    public NeatooJsonSerializer(NeatooJsonConverterFactory neatooJsonConverterFactory, GetServiceImplementationType getImplementationType, ILocalAssemblies localAssemblies)
    {
        Options = new JsonSerializerOptions
        {
            ReferenceHandler = MyReferenceHandler,
            Converters = { neatooJsonConverterFactory },
            WriteIndented = true,
            IncludeFields = true
        };
        this.getImplementationType = getImplementationType;
        this.localAssemblies = localAssemblies;
    }


    public string? Serialize(object? target)
    {
        if (target == null)
        {
            return null;
        }

        using var rr = new NeatooReferenceResolver();
        MyReferenceHandler.asyncLocal.Value = rr;

        return JsonSerializer.Serialize(target, Options);
    }

    public string? Serialize(object? target, Type targetType)
    {
        if (target == null)
        {
            return null;
        }

        using var rr = new NeatooReferenceResolver();
        MyReferenceHandler.asyncLocal.Value = rr;

        return JsonSerializer.Serialize(target, targetType, Options);
    }

    public T? Deserialize<T>(string? json)
    {
        if (string.IsNullOrEmpty(json))
        {
            return default;
        }

        using var rr = new NeatooReferenceResolver();
        MyReferenceHandler.asyncLocal.Value = rr;

        return JsonSerializer.Deserialize<T>(json, Options);
    }

    public object? Deserialize(string? json, Type type)
    {
        if (string.IsNullOrEmpty(json))
        {
            return null;
        }

        using var rr = new NeatooReferenceResolver();
        MyReferenceHandler.asyncLocal.Value = rr;

        return JsonSerializer.Deserialize(json, type, Options);
    }

    public RemoteRequestDto ToRemoteRequest(Type delegateType, params object[]? parameters)
    {
        List<ObjectTypeJson?>? parameterJson = null;

        if (parameters != null)
        {
            parameterJson = parameters.Select(c => ToObjectTypeJson(c)).ToList();
        }

        return new RemoteRequestDto
        {
            DelegateAssemblyType = delegateType.FullName,
            Parameters = parameterJson
        };
    }

    public RemoteRequestDto ToRemoteRequest(Type delegateType, object saveTarget, params object[]? parameters)
    {
        ArgumentNullException.ThrowIfNull(delegateType, nameof(delegateType));
        ArgumentNullException.ThrowIfNull(saveTarget, nameof(saveTarget));


        List<ObjectTypeJson?>? parameterJson = null;

        if (parameters != null)
        {
            if (parameters.Any(p => p == null))
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            parameterJson = parameters.Select(c => ToObjectTypeJson(c)).ToList();
        }
        return new RemoteRequestDto
        {
            DelegateAssemblyType = delegateType.FullName,
            Parameters = parameterJson,
            SaveTarget = ToObjectTypeJson(saveTarget)
        };
    }

    public ObjectTypeJson? ToObjectTypeJson(object? target)
    {
        if (target == null)
        {
            return null;
        }
        ArgumentNullException.ThrowIfNull(target, nameof(target));
        ArgumentNullException.ThrowIfNull(target.GetType().FullName, nameof(Type.FullName));
        return new ObjectTypeJson(Serialize(target)!, target.GetType().FullName!);
    }

    public RemoteRequest DeserializeRemoteRequest(RemoteRequestDto remoteRequest)
    {
        ArgumentNullException.ThrowIfNull(remoteRequest, nameof(remoteRequest));

        object? target = null;
        object[]? parameters = null;

        if (remoteRequest.SaveTarget != null && !string.IsNullOrEmpty(remoteRequest.SaveTarget.Json))
        {
            target = FromObjectTypeJson(remoteRequest.SaveTarget);
        }
        if (remoteRequest.Parameters != null)
        {
            parameters = remoteRequest.Parameters.Select(c => FromObjectTypeJson(c)).ToArray();
        }

        var result = new RemoteRequest()
        {
            DelegateType = localAssemblies.FindType(remoteRequest.DelegateAssemblyType),
            Parameters = parameters,
            SaveTarget = target
        };

        return result;
    }

    public T? DeserializeRemoteResponse<T>(RemoteResponseDto portalResponse)
    {
        if (portalResponse?.ObjectJson == null)
        {
            return default;
        }

        return Deserialize<T>(portalResponse.ObjectJson);
    }

    public object? FromObjectTypeJson(ObjectTypeJson? objectTypeJson)
    {
        if (objectTypeJson == null)
        {
            return null;
        }

        return Deserialize(objectTypeJson.Json, localAssemblies.FindType(objectTypeJson.AssemblyType));
    }

}
