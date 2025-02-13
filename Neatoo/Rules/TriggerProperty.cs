using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Neatoo.Rules;
public interface ITriggerProperty<in T>
{
    string PropertyName { get; }

    object? GetValue(T target);

    bool IsMatch(T target, string propertyName);
}

public interface ITriggerProperty<in T, P> : ITriggerProperty<T>
{
    P? Get(T target);
}

public class TriggerProperty<T, P> : ITriggerProperty<T>
{
    private readonly Expression<Func<T, P?>> expression;
    private readonly string expressionPropertyName;
    public TriggerProperty(Expression<Func<T, P?>> expression)
    {
        this.expression = expression;
        expressionPropertyName = RecurseMembers(expression.Body, new List<string>());
    }

    public bool IsMatch(T t, string propertyName)
    {
        return string.Equals(expressionPropertyName, propertyName);
    }

    public object? GetValue(T target)
    {
        return Get(target);
    }

    public P? Get(T target)
    {
        return expression.Compile()(target);
    }

    public string PropertyName => expressionPropertyName;

    protected string RecurseMembers(Expression? expression, List<string> properties)
    {
        if (expression is UnaryExpression unaryExpression)
        {
            return RecurseMembers(unaryExpression.Operand, properties);
        }
        if (expression is MemberExpression memberExpression)
        {
            properties.Add(memberExpression.Member.Name);
            return RecurseMembers(memberExpression.Expression, properties);
        }

        properties.Reverse();

        return string.Join('.', properties);
    }

    public static TriggerProperty<T1, P1> FromExpression<T1, P1>(Expression<Func<T1, P1?>> expression)
    {
        return new TriggerProperty<T1, P1>(expression);
    }
}
