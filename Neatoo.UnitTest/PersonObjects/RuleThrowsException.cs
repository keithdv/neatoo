﻿using Neatoo.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.PersonObjects
{
    public interface IRuleThrowsException : IRule<IPersonBase>
    {
    }

    public class RuleThrowsException : RuleBase<IPersonBase>, IRuleThrowsException
    {
        public RuleThrowsException() : base()
        {
            TriggerProperties.Add(nameof(IPersonBase.FirstName));
        }

        public override IRuleResult Execute(IPersonBase target)
        {
            if (target.FirstName == "Throw")
            {
                throw new Exception("Rule Failed");
            }
            return RuleResult.Empty();
        }
    }
}
