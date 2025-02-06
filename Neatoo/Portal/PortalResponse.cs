using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
