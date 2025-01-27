using Neatoo.Rules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.PersonObjects
{
    public interface IShortNameRule<T> : IRule<T> where T : IPersonBase { int RunCount { get; } }

    public class ShortNameRule<T> : RuleBase<T>, IShortNameRule<T>
        where T : IPersonBase
    {
        public int RunCount { get; private set; } = 0;

        public ShortNameRule() : base()
        {
            AddTriggerProperties(nameof(IPersonBase.FirstName));
            AddTriggerProperties(nameof(IPersonBase.LastName));
        }

        public override PropertyErrors Execute(T target)
        {
            RunCount++;
            // System.Diagnostics.Debug.WriteLine($"Run Rule {target.FirstName} {target.LastName}");

            if (target.FirstName?.StartsWith("Error") ?? false)
            {
                return (nameof(IPersonBase.FirstName), target.FirstName);
            }

            target.ShortName = $"{target.FirstName} {target.LastName}";

            return PropertyErrors.None;
        }

    }
}
