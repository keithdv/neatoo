using Neatoo.Rules;
using System.Threading;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.PersonObjects;


public interface IFullNameAsyncRule<T> : IRule<T> where T : IPersonBase { int RunCount { get; } }

public class FullNameAsyncRule<T> : AsyncRuleBase<T>, IFullNameAsyncRule<T>
    where T : IPersonBase
{

    public FullNameAsyncRule() : base()
    {

        AddTriggerProperties(_ => _.FirstName);
        AddTriggerProperties(_ => _.ShortName);
    }

    public int RunCount { get; private set; }

    public override async Task<PropertyErrors> Execute(T target, CancellationToken token)
    {
        RunCount++;

        await Task.Delay(10, token);

        // System.Diagnostics.Debug.WriteLine($"FullNameAsyncRule {target.Title} {target.ShortName}");

        target.FullName = $"{target.Title} {target.ShortName}";

        return PropertyErrors.None;

    }

}
