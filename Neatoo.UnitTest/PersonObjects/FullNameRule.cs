using Neatoo.Rules;

namespace Neatoo.UnitTest.PersonObjects;

public interface IFullNameRule<T> : IRule<T> where T : IPersonBase { int RunCount { get; } }

public class FullNameRule<T> : RuleBase<T>, IFullNameRule<T>
    where T : IPersonBase
{
    public int RunCount { get; private set; } = 0;

    public FullNameRule() : base()
    {
        AddTriggerProperties(_ => _.Title);
        AddTriggerProperties(_ => _.ShortName);
    }

    public override PropertyErrors Execute(T target)
    {
        RunCount++;

        target.FullName = $"{target.Title} {target.ShortName}";

        return PropertyErrors.None;
    }
}
