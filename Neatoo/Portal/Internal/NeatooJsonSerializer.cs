using System;
using System.Linq;
using System.Text.Json;

namespace Neatoo.Portal.Internal;

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
        using var rr = new NeatooReferenceResolver();
        MyReferenceHandler.asyncLocal.Value = rr;

        return JsonSerializer.Serialize(target, Options);
    }

    public T Deserialize<T>(string json)
    {
        using var rr = new NeatooReferenceResolver();
        MyReferenceHandler.asyncLocal.Value = rr;

        return JsonSerializer.Deserialize<T>(json, Options);
    }

    public object Deserialize(string json, Type type)
    {
        using var rr = new NeatooReferenceResolver();
        MyReferenceHandler.asyncLocal.Value = rr;

        return JsonSerializer.Deserialize(json, type, Options);
    }

    public RemoteRequest ToRemoteRequest(DataMapperMethod portalOperation, Type targetType)
    {
        return new RemoteRequest()
        {
            DataMapperOperation = portalOperation,
            Target = new ObjectTypeJson() { AssemblyType = getImplementationType(targetType).FullName },
        };
    }

    public RemoteRequest ToRemoteRequest(DataMapperMethod portalOperation, Type targetType, params object[] criteria)
    {
        var criteriaJson = criteria.Select(c => ToObjectTypeJson(c)).ToList();
        return new RemoteRequest()
        {
            DataMapperOperation = portalOperation,
            Target = new ObjectTypeJson() { AssemblyType = getImplementationType(targetType).FullName },
            Criteria = criteriaJson
        };
    }

    public RemoteRequest ToRemoteRequest(DataMapperMethod portalOperation, object target)
    {
        var targetJson = ToObjectTypeJson(target);
        return new RemoteRequest()
        {
            DataMapperOperation = portalOperation,
            Target = targetJson
        };
    }

    public RemoteRequest ToRemoteRequest(DataMapperMethod portalOperation, object target, params object[] criteria)
    {
        var targetJson = ToObjectTypeJson(target);
        var criteriaJson = criteria.Select(c => ToObjectTypeJson(c)).ToList();
        return new RemoteRequest()
        {
            DataMapperOperation = portalOperation,
            Target = targetJson,
            Criteria = criteriaJson
        };
    }

    public ObjectTypeJson ToObjectTypeJson<T>()
    {
        return new ObjectTypeJson()
        {
            AssemblyType = getImplementationType(typeof(T)).FullName
        };
    }

    public ObjectTypeJson ToObjectTypeJson(object target)
    {
        return new ObjectTypeJson()
        {
            Json = target != null ? Serialize(target) : null,
            AssemblyType = getImplementationType(target.GetType()).FullName
        };
    }

    public (object? target, object[]? criteria) FromDataMapperRequest(RemoteRequest portalRequest)
    {

        object? target = null;
        object[]? criteria = null;

        if (portalRequest.Target != null && !string.IsNullOrEmpty(portalRequest.Target.Json))
        {
            target = FromObjectTypeJson(portalRequest.Target);
        }
        if(portalRequest.Criteria != null)
        {
            criteria = portalRequest.Criteria.Select(c => FromObjectTypeJson(c)).ToArray();
        }

        return (target, criteria);
    }

    public object FromPortalResponse(RemoteResponse portalResponse)
    {
        return Deserialize(portalResponse.ObjectJson, INeatooJsonSerializer.ToType(portalResponse.AssemblyType));
    }

    public object FromObjectTypeJson(ObjectTypeJson objectTypeJson)
    {
        return Deserialize(objectTypeJson.Json, INeatooJsonSerializer.ToType(objectTypeJson.AssemblyType));
    }

}
