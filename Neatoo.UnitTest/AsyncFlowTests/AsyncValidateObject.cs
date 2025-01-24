using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neatoo.Core;
using Neatoo.Rules;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            TriggerProperties.Add(nameof(AsyncValidateObject.AsyncDelayRuleValue));
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

    internal class AsyncRuleCanWait : AsyncRuleBase<AsyncValidateObject>
    {
        public AsyncRuleCanWait()
        {
            TriggerProperties.Add(nameof(AsyncValidateObject.AsyncRulesCanWait));
        }
        public int RunCount { get; private set; } = 0;
        public override async Task<IRuleResult> Execute(AsyncValidateObject target, CancellationToken token)
        {
            RunCount++;
            await Task.Delay(5);
            target.AsyncRulesCanWaitNested = "Value";
            var prop = ReadPropertyValue(target, nameof(AsyncValidateObject.AsyncRulesCanWait));

            // Deadlock
            //if(target.AsyncRulesCanWait == "Wait")
            //{
            //    await prop;
            //    Assert.AreEqual("Ran", prop.Value);
            //}

            return RuleResult.Empty();
        }
    }

    internal class AsyncRuleCanWaitNested : AsyncRuleBase<AsyncValidateObject>
    {
        public AsyncRuleCanWaitNested()
        {
            TriggerProperties.Add(nameof(AsyncValidateObject.AsyncRulesCanWait));
        }
        public int RunCount { get; private set; } = 0;
        public override async Task<IRuleResult> Execute(AsyncValidateObject target, CancellationToken token)
        {
            RunCount++;
            await Task.Delay(5);
            target.AsyncRulesCanWaitNested = "Ran";
            target.AsyncDelayRuleValue = Guid.NewGuid().ToString();
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
            RuleManager.AddRule(new AsyncRuleCanWait());
            RuleManager.AddRule(new AsyncRuleCanWaitNested());
        }

        public AsyncDelayRule AsyncDelayRule { get; private set; }
        public SyncRuleA SyncRuleA { get; private set; }
        public NestedSyncRuleB NestedSyncRuleB { get; private set; }

        public string HasNoRules { get => Getter<string>(); set => Setter(value); }

        public string SyncA { get => Getter<string>(); set => Setter(value); }

        public string NestedSyncB { get => Getter<string>(); set => Setter(value); }

        public string AsyncDelayRuleValue { get => Getter<string>(); set => Setter(value); }

        public IPropertyValue AsyncRuleCanWaitPropertyValue => this[nameof(AsyncRulesCanWait)];

        public string AsyncRulesCanWait { get => Getter<string>(); set => Setter(value); }
        public string AsyncRulesCanWaitNested { get => Getter<string>(); set => Setter(value); }

    }


}
