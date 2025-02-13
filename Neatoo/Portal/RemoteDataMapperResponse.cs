namespace Neatoo.Portal;

public class RemoteDataMapperResponse
{
    public RemoteDataMapperResponse(string objectJson, string assemblyType)
    {
        ObjectJson = objectJson;
        AssemblyType = assemblyType;
    }

    public string ObjectJson { get; private set; }
    public string AssemblyType { get; private set; }
}
