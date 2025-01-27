using Neatoo.Rules;
using Neatoo.UnitTest.EditBaseTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.PersonObjects
{
    internal interface IRecursiveRule : IRule<IPersonBase> { }

    internal class RecursiveRule : RuleBase<IPersonBase>, IRecursiveRule
    {
        public RecursiveRule() : base()
        {
            AddTriggerProperties(nameof(IPersonBase.ShortName));
        }
        public override PropertyErrors Execute(IPersonBase target)
        {
            if (target.ShortName == "Recursive")
            {
                target.ShortName = "Recursive change";
            }
            else if (target.ShortName == "Recursive Error")
            {
                target.FirstName = "Error"; // trigger the ShortNameRule error
            }
            return PropertyErrors.None;
        }
    }
}
