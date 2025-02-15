using System;
using System.Linq;

namespace Neatoo.Portal;

public interface INeatooJsonSerializer
{
    string Serialize(object target);
    T Deserialize<T>(string json);
    object Deserialize(string json, Type type);
    RemoteRequest ToRemoteRequest(DataMapperMethod portalOperation, Type targetType);
    RemoteRequest ToRemoteRequest(DataMapperMethod portalOperation, Type targetType, params object[] criteria);
    RemoteRequest ToRemoteRequest(DataMapperMethod portalOperation, object target);
    RemoteRequest ToRemoteRequest(DataMapperMethod portalOperation, object target, params object[] criteria);
    (object? target, object[]? criteria) FromDataMapperRequest(RemoteRequest portalRequest);
    object FromPortalResponse(RemoteResponse portalResponse);

    public static Type? ToType(string fullName)
    {
        var types = AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType(fullName));
        var type = types.FirstOrDefault(t => t != null);

        return type;
    }
}