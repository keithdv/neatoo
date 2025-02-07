using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Neatoo.Rules.Rules
{

    public interface IAttributeToRule
    {
        IRule GetRule(IRegisteredProperty r, Type attribute);
    }

    public class AttributeToRule : IAttributeToRule
    {

        public AttributeToRule(CreateRequiredRule required)
        {
            Required = required;
        }

        public IRule GetRule(IRegisteredProperty r, Type attribute)
        {
            if (attribute == typeof(RequiredAttribute))
            {
                return Required(r);
            }
            return null;
        }


        public CreateRequiredRule Required { get; }
    }
}
