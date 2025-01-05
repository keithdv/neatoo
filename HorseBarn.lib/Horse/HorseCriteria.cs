using Neatoo;
using Neatoo.Rules;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorseBarn.lib.Horse
{
    public class HorseCriteria : ValidateBase<HorseCriteria>
    {
        public HorseCriteria(IValidateBaseServices<HorseCriteria> services) : base(services)
        {
            AddRules(this.RuleManager);
        }

        private static void AddRules(IRuleManager<HorseCriteria> ruleManager)
        {

        }



    }
}
