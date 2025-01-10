using Neatoo.Rules;
using Neatoo.UnitTest.ValidateBaseTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.PersonObjects
{

    public interface IAsyncRuleThrowsException : IRule<IValidateAsyncObject>
    {
    }

    public class AsyncRuleThrowsException : AsyncRuleBase<IValidateAsyncObject>, IAsyncRuleThrowsException
    {
        public AsyncRuleThrowsException() : base()
        {
            TriggerProperties.Add("ThrowException");
        }

        public override Task<IRuleResult> Execute(IValidateAsyncObject target, CancellationToken token)
        {
            if(target.ThrowException == "Throw")
            {
                throw new Exception("Rule Failed");
            }
            return Task.FromResult<IRuleResult>(RuleResult.Empty());
        }
    }
}
