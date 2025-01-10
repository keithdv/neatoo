using Neatoo.Rules;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.PersonObjects
{
    public interface IShortNameAsyncRule<T> : IRule<T> where T : IPersonBase { int RunCount { get; } }

    public class ShortNameAsyncRule<T> : AsyncRuleBase<T>, IShortNameAsyncRule<T>
        where T : IPersonBase
    {

        private Guid UniqueId = Guid.NewGuid();

        public ShortNameAsyncRule() : base()
        {
            TriggerProperties.Add(nameof(IPersonBase.FirstName));
            TriggerProperties.Add(nameof(IPersonBase.LastName));
        }

        public int RunCount { get; private set; }

        public override async Task<IRuleResult> Execute(T target, CancellationToken token)
        {
            RunCount++;

            await Task.Delay(10, token);

            Console.WriteLine($"ShortNameAsyncRule: {UniqueId.ToString()} {target.FirstName} {target.LastName}");

            // System.Diagnostics.Debug.WriteLine($"ShortNameAsyncRule {target.FirstName} {target.LastName}");

            if (target.FirstName?.StartsWith("Error") ?? false)
            {
                return RuleResult.PropertyError(nameof(IPersonBase.FirstName), target.FirstName);
            }


            target.ShortName = $"{target.FirstName} {target.LastName}";

            return RuleResult.Empty();
        }

    }
}
