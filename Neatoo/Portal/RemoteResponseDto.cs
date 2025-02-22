using System.Text.Json.Serialization;

namespace Neatoo.Portal;

public class RemoteResponseDto
{
    [JsonConstructor]
    public RemoteResponseDto(string? objectJson)
    {
        ObjectJson = objectJson;
    }

    public string? ObjectJson { get; private set; }
}
