using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neatoo.Portal
{
    public interface IPortalJsonSerializer
    {
        string Serialize(object target);
        ObjectTypeJson ToObjectTypeJson<T>();
        ObjectTypeJson ToObjectTypeJson(object target);
        T Deserialize<T>(string json);
        object Deserialize(string json, Type type);
        object FromObjectTypeJson(ObjectTypeJson objectTypeJson);
    }
}
