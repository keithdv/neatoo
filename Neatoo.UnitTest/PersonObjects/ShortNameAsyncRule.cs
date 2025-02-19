using Neatoo.Rules;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.PersonObjects;

public interface IShortNameAsyncRule<T> : IRule<T> where T : IPersonBase { int RunCount { get; } }

public class ShortNameAsyncRule<T> : AsyncRuleBase<T>, IShortNameAsyncRule<T>
    where T : IPersonBase
{

    private Guid UniqueId = Guid.NewGuid();

    public ShortNameAsyncRule() : base()
    {
        AddTriggerProperties(_ => _.FirstName);
        AddTriggerProperties(_ => _.LastName);
    }

    public int RunCount { get; private set; }

    public override async Task<PropertyErrors> Execute(T target, CancellationToken token)
    {
        Console.WriteLine($"ShortNameAsyncRule: {UniqueId.ToString()} BEGIN");

        RunCount++;

        await Task.Delay(10, token);


        // System.Diagnostics.Debug.WriteLine($"ShortNameAsyncRule {target.FirstName} {target.LastName}");

        if (target.FirstName?.StartsWith("Error") ?? false)
        {
            Console.WriteLine($"ShortNameAsyncRule: {UniqueId.ToString()} {target.FirstName} {target.LastName} Error!!");
            return nameof(IPersonBase.FirstName).PropertyError(target.FirstName);
        }

        Console.WriteLine($"ShortNameAsyncRule: {UniqueId.ToString()} {target.FirstName} {target.LastName}");

        target.ShortName = $"{target.FirstName} {target.LastName}";

        Console.WriteLine($"ShortNameAsyncRule: {UniqueId.ToString()} Done");

        return PropertyErrors.None;
    }

}
