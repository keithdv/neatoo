
using Neatoo.Rules;
using Neatoo.UnitTest.Objects;
using System;

namespace Neatoo.UnitTest.PersonObjects
{
    public interface IShortNameDependencyRule<T> : IRule<T> where T : IPersonBase { }

    public class ShortNameDependencyRule<T> : RuleBase<T>, IShortNameDependencyRule<T>
        where T : IPersonBase
    {

        public ShortNameDependencyRule(IDisposableDependency dd) : base()
        {
            AddTriggerProperties(_ => _.FirstName);
            AddTriggerProperties(_ => _.LastName);
            DisposableDependency = dd;
        }

        private IDisposableDependency DisposableDependency { get; }

        public override PropertyErrors Execute(T target)
        {

            // System.Diagnostics.Debug.WriteLine($"Run Rule {target.FirstName} {target.LastName}");

            var dd = DisposableDependency ?? throw new ArgumentNullException(nameof(DisposableDependency));

            if (target.FirstName?.StartsWith("Error") ?? false)
            {
                return (nameof(IPersonBase.FirstName), target.FirstName);
            }


            target.ShortName = $"{target.FirstName} {target.LastName}";

            return PropertyErrors.None;
        }

    }
}
