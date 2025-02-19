namespace Neatoo.Portal;

public class ObjectTypeJson
{
    public string Json { get; }
    public string AssemblyType { get; }

    public ObjectTypeJson(string json, string assemblyType)
    {
        Json = json;
        AssemblyType = assemblyType;
    }
}