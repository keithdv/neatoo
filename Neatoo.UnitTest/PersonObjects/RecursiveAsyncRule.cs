using Neatoo.Rules;
using Neatoo.UnitTest.EditBaseTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.PersonObjects
{
    internal interface IRecursiveAsyncRule : IRule<IPersonBase> { }

    internal class RecursiveAsyncRule : AsyncRuleBase<IPersonBase>, IRecursiveAsyncRule
    {
        public RecursiveAsyncRule() : base()
        {
            AddTriggerProperties(nameof(IPersonBase.ShortName));
        }
        public override async Task<PropertyErrors> Execute(IPersonBase target, CancellationToken token)
        {

            await Task.Delay(10, token);

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
