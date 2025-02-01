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
        T Deserialize<T>(string json);
        object Deserialize(string json, Type type);

        public ObjectTypeJson ToObjectTypeJson<T>()
        {
            return new ObjectTypeJson()
            {
                AssemblyType = typeof(T).FullName
            };
        }

        public ObjectTypeJson ToObjectTypeJson(object target)
        {
            return new ObjectTypeJson()
            {
                Json = target != null ? Serialize(target) : null,
                AssemblyType = target.GetType().FullName
            };
        }

        public object FromObjectTypeJson(ObjectTypeJson objectTypeJson)
        {
            return Deserialize(objectTypeJson.Json, ToType(objectTypeJson.AssemblyType));
        }
        public static Type ToType(string fullName)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType(fullName));
            var type = types.FirstOrDefault(t => t != null);

            return type;
        }

    }
}
