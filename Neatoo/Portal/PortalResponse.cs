namespace Neatoo.Portal
{
    public class PortalResponse
    {
        public PortalResponse(string objectJson, string assemblyType)
        {
            ObjectJson = objectJson;
            AssemblyType = assemblyType;
        }

        public string ObjectJson { get; private set; }
        public string AssemblyType { get; private set; }
    }
}
