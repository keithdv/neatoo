using Neatoo.Rules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.PersonObjects
{
    public interface IFullNameRule<T> : IRule<T> where T : IPersonBase { int RunCount { get; } }

    public class FullNameRule<T> : RuleBase<T>, IFullNameRule<T>
        where T : IPersonBase
    {
        public int RunCount { get; private set; } = 0;

        public FullNameRule() : base()
        {
            AddTriggerProperties(nameof(IPersonBase.Title));
            AddTriggerProperties(nameof(IPersonBase.ShortName));
        }

        public override PropertyErrors Execute(T target)
        {
            RunCount++;

            target.FullName = $"{target.Title} {target.ShortName}";

            return PropertyErrors.None;
        }
    }
}
