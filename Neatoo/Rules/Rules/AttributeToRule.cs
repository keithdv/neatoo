using System;
using System.ComponentModel.DataAnnotations;

namespace Neatoo.Rules.Rules
{

    public interface IAttributeToRule
    {
        IRule<T> GetRule<T>(IRegisteredProperty r, Type attribute) where T : IValidateBase;
    }

    public class AttributeToRule : IAttributeToRule
    {

        public AttributeToRule()
        {

        }

        public IRule<T> GetRule<T>(IRegisteredProperty r, Type attribute) where T : IValidateBase
        {
            if (attribute == typeof(RequiredAttribute))
            {
                return new RequiredRule<T>(r);
            }
            return null;
        }

    }
}
