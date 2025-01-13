using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Neatoo.Portal
{
    // TODO : Make these two an interface so that we car resolve them from DI
    // so that for newtonsoft we can use [JsonConstructor] attribute
    // so that we can do Nullable correctly
    public class PortalRequest
    {
        public PortalOperation PortalOperation { get; set; }
        public ObjectTypeJson Target { get; set; }
        public List<ObjectTypeJson> Criteria { get; set; }
    }

    public class ObjectTypeJson
    {
        public string Json { get; set; }
        public string AssemblyType { get; set; }
    }

    /// <summary>
    /// Can't use Newtonsoft IgnoreJson attribute
    /// </summary>
    public static class ObjectTypeJsonExtensions
    {
        public static Type Type(this ObjectTypeJson objectTypeJson)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType(objectTypeJson.AssemblyType));
            var type = types.FirstOrDefault(t => t != null);

            return type;
        }
    }
}