using System.Text.Json.Serialization;

namespace Neatoo.Portal;

public class RemoteResponseDto
{
    [JsonConstructor]
    public RemoteResponseDto(string objectJson, string assemblyType)
    {
        ObjectJson = objectJson;
        AssemblyType = assemblyType;
    }

    public string ObjectJson { get; private set; }
    public string AssemblyType { get; private set; }
}
