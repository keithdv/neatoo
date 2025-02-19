using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Neatoo.Portal;

public class RemoteRequestDto
{
    public string DelegateAssemblyType { get; set; } = null!;
    public IReadOnlyCollection<ObjectTypeJson>? Parameters { get; set; }
    public ObjectTypeJson? SaveTarget { get; set; }
}
