using Neatoo.Core;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Neatoo.Rules.Rules;

public interface IRequiredRule<T> : IRule<T>
    where T : IValidateBase
{

}

    internal class RequiredRule<T> : RuleBase<T>, IRequiredRule<T>
    where T : IValidateBase
{
    public RequiredRule(IPropertyInfo propertyInfo) : base() {



        var parameter = Expression.Parameter(typeof(T));
        var property = Expression.Property(parameter, propertyInfo.Name);
        var lambda = Expression.Lambda<Func<T, object?>>(Expression.Convert(property, typeof(object)), parameter);
        var tr = new TriggerProperty<T, object>(lambda);

        TriggerProperties.Add(tr);
    }

    public override PropertyErrors Execute(T target)
    {
        var value = TriggerProperties[0].GetValue(target);

        bool isError = false;

        if (value is string s)
        {
            isError = string.IsNullOrWhiteSpace(s);
        }
        else if (value?.GetType().IsValueType ?? false)
        {
            isError = value == Activator.CreateInstance(value.GetType());
        }
        else
        {
            isError = value == null;
        }

        if (isError)
        {
            return new PropertyError(TriggerProperties.Single().PropertyName, $"{TriggerProperties[0].PropertyName} is required.");
        }
        return PropertyErrors.None;
    }

}
