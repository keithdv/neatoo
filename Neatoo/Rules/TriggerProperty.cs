using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Neatoo.Rules
{
    public interface ITriggerProperty
    {
        string PropertyName { get; }

        PropertyError PropertyError(string message);

        bool IsMatch<T>(T target, string propertyName);

    }

    internal class TriggerProperty : ITriggerProperty
    {
        private readonly Expression expression;

        public string PropertyName { get; protected set; }

        public TriggerProperty(Expression expression)
        {
            this.expression = expression;
        }

        public TriggerProperty(string propertyName)
        {
            this.PropertyName = propertyName;
        }

        public PropertyError PropertyError(string message)
        {
            return new PropertyError(PropertyName, message);
        }

        public bool IsMatch<T>(T t, string propertyName)
        {
            if (!string.IsNullOrWhiteSpace(PropertyName))
            {
                return propertyName == PropertyName;
            }
            else if (expression != null)
            {
                var memberExpression = expression as MemberExpression;
                if (memberExpression != null)
                {
                    return memberExpression.Member.Name == propertyName;
                }
            }

            throw new Exception("Invalid TriggerProperty: expression and propertyName not defined");
        }

        public static implicit operator TriggerProperty(string propertyName)
        {
            return new TriggerProperty(propertyName);
        }
    }

}
