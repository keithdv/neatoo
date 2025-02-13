using System.Collections.Generic;

namespace Neatoo.Portal;

public class RemoteDataMapperRequest
{
    public DataMapperMethod DataMapperOperation { get; set; }
    public ObjectTypeJson Target { get; set; }
    public List<ObjectTypeJson>? Criteria { get; set; }
}
