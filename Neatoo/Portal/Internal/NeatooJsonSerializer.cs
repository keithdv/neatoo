using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Neatoo.Portal.Internal;

public interface INeatooJsonSerializer
{
    string Serialize(object target);
    T? Deserialize<T>(string json);
    object? Deserialize(string json, Type type);
    RemoteRequestDto ToRemoteRequest(Type delegateType, params object[]? parameters);
    RemoteRequestDto ToRemoteRequest(Type delegateType, object saveTarget, params object[]? parameters);
    (object? saveTarget, object[]? parameters) DeserializeRemoteRequest(RemoteRequestDto remoteRequest);
    object DeserializeRemoteResponse(RemoteResponseDto remoteResponse);

    public static Type? FindType(string fullName)
    {
        var types = AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType(fullName));
        var type = types.FirstOrDefault(t => t != null);

        return type;
    }
}

public class NeatooJsonSerializer : INeatooJsonSerializer
{
    private readonly GetImplementationType getImplementationType;

    JsonSerializerOptions Options { get; }

    private NeatooReferenceHandler MyReferenceHandler { get; } = new NeatooReferenceHandler();
    public NeatooJsonSerializer(NeatooJsonConverterFactory neatooJsonConverterFactory, GetImplementationType getImplementationType)
    {
        Options = new JsonSerializerOptions
        {
            ReferenceHandler = MyReferenceHandler,
            Converters = { neatooJsonConverterFactory },
            WriteIndented = true,
            IncludeFields = true
        };
        this.getImplementationType = getImplementationType;
    }


    public string Serialize(object target)
    {
        ArgumentNullException.ThrowIfNull(target, nameof(target));

        using var rr = new NeatooReferenceResolver();
        MyReferenceHandler.asyncLocal.Value = rr;

        return JsonSerializer.Serialize(target, Options);
    }

    public T? Deserialize<T>(string json)
    {
        using var rr = new NeatooReferenceResolver();
        MyReferenceHandler.asyncLocal.Value = rr;

        return JsonSerializer.Deserialize<T>(json, Options);
    }

    public object? Deserialize(string json, Type type)
    {
        using var rr = new NeatooReferenceResolver();
        MyReferenceHandler.asyncLocal.Value = rr;

        return JsonSerializer.Deserialize(json, type, Options);
    }

    public RemoteRequestDto ToRemoteRequest(Type delegateType, params object[]? parameters)
    {
        List<ObjectTypeJson>? parameterJson = null;

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

        List<ObjectTypeJson>? parameterJson = null;

        if (parameters != null)
        {
            parameterJson = parameters.Select(c => ToObjectTypeJson(c)).ToList();
        }
        return new RemoteRequestDto
        {
            DelegateAssemblyType = delegateType.FullName,
            Parameters = parameterJson,
            SaveTarget = ToObjectTypeJson(saveTarget)
        };
    }

    public ObjectTypeJson ToObjectTypeJson(object target)
    {
        ArgumentNullException.ThrowIfNull(target, nameof(target));
        ArgumentNullException.ThrowIfNull(target.GetType().FullName, nameof(Type.FullName));
        return new ObjectTypeJson(Serialize(target), target.GetType().FullName!);
    }

    public (object? saveTarget, object[]? parameters) DeserializeRemoteRequest(RemoteRequestDto remoteRequest)
    {
        ArgumentNullException.ThrowIfNull(remoteRequest, nameof(remoteRequest));

        object? target = null;
        object[]? parameters = null;

        if (remoteRequest.SaveTarget != null && !string.IsNullOrEmpty(remoteRequest.SaveTarget.Json))
        {
            target = FromObjectTypeJson(remoteRequest.SaveTarget);
        }
        if(remoteRequest.Parameters != null)
        {
            parameters = remoteRequest.Parameters.Select(c => FromObjectTypeJson(c)).ToArray();
        }

        return (target, parameters);
    }

    public object DeserializeRemoteResponse(RemoteResponseDto portalResponse)
    {
        return Deserialize(portalResponse.ObjectJson, INeatooJsonSerializer.FindType(portalResponse.AssemblyType));
    }

    public object FromObjectTypeJson(ObjectTypeJson objectTypeJson)
    {
        return Deserialize(objectTypeJson.Json, INeatooJsonSerializer.FindType(objectTypeJson.AssemblyType));
    }

}
