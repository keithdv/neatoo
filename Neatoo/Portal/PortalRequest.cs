using System.Collections.Generic;

namespace Neatoo.Portal;

// TODO : Make these two an interface so that we car resolve them from DI
// so that for newtonsoft we can use [JsonConstructor] attribute
// so that we can do Nullable correctly


public class PortalRequest
{
    public PortalOperation PortalOperation { get; set; }
    public ObjectTypeJson Target { get; set; }
    public List<ObjectTypeJson>? Criteria { get; set; }
}

public class ObjectTypeJson
{
    public string? Json { get; set; }
    public string AssemblyType { get; set; }
}