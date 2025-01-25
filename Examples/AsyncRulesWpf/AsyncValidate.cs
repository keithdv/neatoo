using Neatoo;
using Neatoo.Core;
using Neatoo.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AsyncRulesWpf
{
    public class AsyncValidate : ValidateBase<AsyncValidate>
    {
        public AsyncValidate() : base(new ValidateBaseServices<AsyncValidate>())
        {
            AddRules(RuleManager);
        }

        private static void AddRules(IRuleManager<AsyncValidate> ruleManager)
        {
            ruleManager.AddRule(async (AsyncValidate t) =>
            {
                await Task.Delay(1000);
                t.AsyncPropertyB = t.AsyncPropertyA * 2;
                return RuleResult.Empty();
            }, [ nameof(AsyncPropertyA) ]);

            ruleManager.AddRule(async (AsyncValidate t) =>
            {
                await Task.Delay(1000);
                t.AsyncPropertyC = t.AsyncPropertyB * 2;
                return RuleResult.Empty();
            }, [nameof(AsyncPropertyB)]);

            ruleManager.AddRule(async (AsyncValidate t) =>
            {
                await Task.Delay(1000);
                t.AsyncPropertyD = t.AsyncPropertyC /  2;
                return RuleResult.Empty();
            }, [nameof(AsyncPropertyC)]);

            ruleManager.AddRule(async (AsyncValidate t) =>
            {
                await Task.Delay(1000);
                if(t.AsyncPropertyA == 100)
                {
                    return RuleResult.PropertyError(nameof(t.AsyncPropertyA), "AsyncPropertyA cannot be 100");
                }
                return RuleResult.Empty();
            }, [nameof(AsyncPropertyA)]);

            ruleManager.AddRule(async (AsyncValidate t) =>
            {
                await Task.Delay(1000);
                if (t.AsyncPropertyB == 100)
                {
                    return RuleResult.PropertyError(nameof(t.AsyncPropertyB), "AsyncPropertyB cannot be 100");
                }
                return RuleResult.Empty();
            }, [nameof(AsyncPropertyB)]);

            ruleManager.AddRule(async (AsyncValidate t) =>
            {
                await Task.Delay(1000);
                if (t.AsyncPropertyC == 100)
                {
                    return RuleResult.PropertyError(nameof(t.AsyncPropertyC), "AsyncPropertyC cannot be 100");
                }
                return RuleResult.Empty();
            }, [nameof(AsyncPropertyC)]);

            ruleManager.AddRule(async (AsyncValidate t) =>
            {
                await Task.Delay(1000);
                if (t.AsyncPropertyD == 100)
                {
                    return RuleResult.PropertyError(nameof(t.AsyncPropertyC), "AsyncPropertyD cannot be 100");
                }
                return RuleResult.Empty();
            }, [nameof(AsyncPropertyD)]);
        }

        new public IValidatePropertyValue this[string propertyName] => base[propertyName];

        public uint AsyncPropertyA        {            get => Getter<uint>();            set => Setter(value);        }

        public uint AsyncPropertyB { get => Getter<uint>(); set => Setter(value); }

        public uint AsyncPropertyC { get => Getter<uint>(); set => Setter(value); }

        public uint AsyncPropertyD { get => Getter<uint>(); set => Setter(value); }

    }
}
