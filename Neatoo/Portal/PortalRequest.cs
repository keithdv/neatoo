using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Neatoo.Portal
{
    public class PortalRequest
    {
        public PortalOperation PortalOperation { get; set; }
        public string ObjectJson { get; set; }
        public string Type { get; set; }
        public string CriteriaJson { get; set; }
        public string CriteriaTypes { get; set; }
    }
}
