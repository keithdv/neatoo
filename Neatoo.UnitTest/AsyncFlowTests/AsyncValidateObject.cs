using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neatoo.Core;
using Neatoo.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.AsyncFlowTests
{
    internal class AsyncDelayRule : AsyncRuleBase<AsyncValidateObject>
    {

        public AsyncDelayRule()
        {
            TriggerProperties.Add(nameof(AsyncValidateObject.HasAsyncRules));
        }
        public int RunCount { get; private set; } = 0;

        public override async Task<IRuleResult> Execute(AsyncValidateObject target, CancellationToken token)
        {
            RunCount++;
            await Task.Delay(5);
            return RuleResult.Empty();
        }
    }

    internal class SyncRuleA : RuleBase<AsyncValidateObject>
    {
        public SyncRuleA()
        {
            TriggerProperties.Add(nameof(AsyncValidateObject.SyncA));
        }

        public int RunCount { get; private set; } = 0;

        public override IRuleResult Execute(AsyncValidateObject target)
        {
            target.NestedSyncB = Guid.NewGuid().ToString();
            RunCount++;
            return RuleResult.Empty();
        }
    }
    internal class NestedSyncRuleB : RuleBase<AsyncValidateObject>
    {
        public NestedSyncRuleB()
        {
            TriggerProperties.Add(nameof(AsyncValidateObject.NestedSyncB));
        }
        public int RunCount { get; private set; } = 0;

        public override IRuleResult Execute(AsyncValidateObject target)
        {
            RunCount++;
            return RuleResult.Empty();
        }
    }

    internal class AsyncValidateObject : ValidateBase<AsyncValidateObject>
    {
        public AsyncValidateObject(IValidateBaseServices<AsyncValidateObject> services) : base(services)
        {
            RuleManager.AddRule(AsyncDelayRule = new AsyncDelayRule());
            RuleManager.AddRule(SyncRuleA = new SyncRuleA());
            RuleManager.AddRule(NestedSyncRuleB = new NestedSyncRuleB());
        }

        public AsyncDelayRule AsyncDelayRule { get; private set; }
        public SyncRuleA SyncRuleA { get; private set; }
        public NestedSyncRuleB NestedSyncRuleB { get; private set; }

        public string HasNoRules { get => Getter<string>(); set => Setter(value); }

        public string SyncA { get => Getter<string>(); set => Setter(value); }

        public string NestedSyncB { get => Getter<string>(); set => Setter(value); }

        public string HasAsyncRules { get => Getter<string>(); set => Setter(value); }

    }


}
