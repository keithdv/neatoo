using Neatoo.Rules;
using Neatoo.UnitTest.Objects;
using System;


namespace Neatoo.UnitTest.PersonObjects;

public interface IFullNameDependencyRule<T> : IRule<T> where T : IPersonBase { }

public class FullNameDependencyRule<T> : RuleBase<T>, IFullNameDependencyRule<T>
    where T : IPersonBase
{

    public FullNameDependencyRule(IDisposableDependency dd) : base()
    {
        AddTriggerProperties(_ => _.Title);
        AddTriggerProperties(_ => _.ShortName);

        this.DisposableDependency = dd;
    }

    private IDisposableDependency DisposableDependency { get; }

    public override PropertyErrors Execute(T target)
    {

        var dd = DisposableDependency ?? throw new ArgumentNullException(nameof(DisposableDependency));

        target.FullName = $"{target.Title} {target.ShortName}";

        return PropertyErrors.None;

    }

}
