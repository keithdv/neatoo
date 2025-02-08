using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Schema;

namespace Neatoo.Rules
{
    public interface ITriggerProperty<in T>
    {
        string PropertyName { get; }

        bool IsMatch(T target, string propertyName);
        object? GetValue(T target);
    }

    //internal class TriggerProperty : ITriggerProperty
    //{
    //    private readonly Expression expression;

    //    public string PropertyName { get; protected set; }

    //    public TriggerProperty(Expression expression)
    //    {
    //        this.expression = expression;
    //    }

    //    public TriggerProperty(string propertyName)
    //    {
    //        this.PropertyName = propertyName;
    //    }

    //    public PropertyError PropertyError(string message)
    //    {
    //        return new PropertyError(PropertyName, message);
    //    }

    //    public bool IsMatch<T>(T t, string propertyName)
    //    {
    //        if (!string.IsNullOrWhiteSpace(PropertyName))
    //        {
    //            return propertyName == PropertyName;
    //        }
    //        else if (expression is Expression<Expression<Func<T, Object>>> funcExpression)
    //        {
    //        }

    //        throw new Exception("Invalid TriggerProperty: expression and propertyName not defined");
    //    }

    //    protected string RecurseMembers(MemberExpression memberExpression)
    //    {
    //        if (memberExpression.Expression is MemberExpression)
    //        {
    //            return RecurseMembers(memberExpression.Expression as MemberExpression) + "." + memberExpression.Member.Name;
    //        }
    //        return memberExpression.Member.Name;
    //    }

    //    public static implicit operator TriggerProperty(string propertyName)
    //    {
    //        return new TriggerProperty(propertyName);
    //    }
    //}

    public class TriggerProperty<T> : ITriggerProperty<T>
    {
        private readonly Expression<Func<T, object?>> expression;
        private readonly string expressionPropertyName;
        public TriggerProperty(Expression<Func<T, object?>> expression)
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

        public static implicit operator TriggerProperty<T>(Expression<Func<T, object?>> expression)
        {
            return new TriggerProperty<T>(expression);
        }
    }
}
