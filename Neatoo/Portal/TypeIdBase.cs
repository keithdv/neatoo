using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neatoo.Portal
{

    public class SerializationTypeId
    {
        public SerializationTypeId(long id, string typeName, string? interfaceFullName)
        {
            Id = id;
            InterfaceFullName = interfaceFullName;
            TypeName = typeName;
        }

        public long Id { get; }
        public string? InterfaceFullName { get; }
        public string TypeName { get; }
    }
}
