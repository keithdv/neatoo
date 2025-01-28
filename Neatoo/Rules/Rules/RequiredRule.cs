using Neatoo.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neatoo.Rules.Rules
{
    public interface IRequiredRule : IRule
    {

    }

    public delegate IRequiredRule CreateRequiredRule(string name);

    internal class RequiredRule : RuleBase<IValidateBase>, IRequiredRule
    {
        public RequiredRule(string propertyName) : base(propertyName) { }
        public RequiredRule(IRegisteredProperty registeredProperty) : base(registeredProperty) { }

        public override PropertyErrors Execute(IValidateBase target)
        {
            var value = ReadProperty(TriggerProperties[0]);

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
                return TriggerProperties.Single().PropertyError($"{TriggerProperties[0].PropertyName} is required.");
            }
            return PropertyErrors.None;
        }

    }
}
