using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neatoo.Portal
{
    public interface IPortalJsonSerializer
    {
        string Serialize(object target);
        T Deserialize<T>(string json);
        object Deserialize(string json, Type type);
    }
}
