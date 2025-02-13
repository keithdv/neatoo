using System;
using System.Linq;

namespace Neatoo.Portal;

public interface INeatooJsonSerializer
{
    string Serialize(object target);
    T Deserialize<T>(string json);
    object Deserialize(string json, Type type);
    RemoteDataMapperRequest ToDataMapperHostRequest(DataMapperMethod portalOperation, Type targetType);
    RemoteDataMapperRequest ToDataMapperHostRequest(DataMapperMethod portalOperation, Type targetType, params object[] criteria);
    RemoteDataMapperRequest ToDataMapperHostRequest(DataMapperMethod portalOperation, object target);
    RemoteDataMapperRequest ToDataMapperHostRequest(DataMapperMethod portalOperation, object target, params object[] criteria);
    (object target, object[] criteria) FromDataMapperRequest(RemoteDataMapperRequest portalRequest);
    object FromPortalResponse(RemoteDataMapperResponse portalResponse);

    public static Type? ToType(string fullName)
    {
        var types = AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType(fullName));
        var type = types.FirstOrDefault(t => t != null);

        return type;
    }
}