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
            AddTriggerProperties(nameof(AsyncValidateObject.AsyncDelayRuleValue));
        }
        public int RunCount { get; private set; } = 0;

        public override async Task<PropertyErrors> Execute(AsyncValidateObject target, CancellationToken token)
        {
            RunCount++;
            await Task.Delay(5);
            return None;
        }
    }

    internal class SyncRuleA : RuleBase<AsyncValidateObject>
    {
        public SyncRuleA()
        {
            AddTriggerProperties(nameof(AsyncValidateObject.SyncA));
        }

        public int RunCount { get; private set; } = 0;

        public override PropertyErrors Execute(AsyncValidateObject target)
        {
            target.NestedSyncB = Guid.NewGuid().ToString();
            RunCount++;
            return PropertyErrors.None;
        }
    }
    internal class NestedSyncRuleB : RuleBase<AsyncValidateObject>
    {
        public NestedSyncRuleB()
        {
            AddTriggerProperties(nameof(AsyncValidateObject.NestedSyncB));
        }
        public int RunCount { get; private set; } = 0;

        public override PropertyErrors Execute(AsyncValidateObject target)
        {
            RunCount++;
            return PropertyErrors.None;
        }
    }

    internal class AsyncRuleCanWait : AsyncRuleBase<AsyncValidateObject>
    {
        public AsyncRuleCanWait()
        {
            AddTriggerProperties(nameof(AsyncValidateObject.AsyncRulesCanWait));
        }
        public int RunCount { get; private set; } = 0;
        public override async Task<PropertyErrors> Execute(AsyncValidateObject target, CancellationToken token)
        {
            RunCount++;
            await Task.Delay(5);
            target.AsyncRulesCanWaitNested = "Value";
            var prop = base.ReadProperty(nameof(AsyncValidateObject.AsyncRulesCanWait));

            // Deadlock
            //if(target.AsyncRulesCanWait == "Wait")
            //{
            //    await prop;
            //    Assert.AreEqual("Ran", prop.Value);
            //}

            return PropertyErrors.None;
        }
    }

    internal class AsyncRuleCanWaitNested : AsyncRuleBase<AsyncValidateObject>
    {
        public AsyncRuleCanWaitNested()
        {
            AddTriggerProperties(nameof(AsyncValidateObject.AsyncRulesCanWait));
        }
        public int RunCount { get; private set; } = 0;
        public override async Task<PropertyErrors> Execute(AsyncValidateObject target, CancellationToken token)
        {
            RunCount++;
            await Task.Delay(5);
            target.AsyncRulesCanWaitNested = "Ran";
            target.AsyncDelayRuleValue = Guid.NewGuid().ToString();
            return PropertyErrors.None;
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
