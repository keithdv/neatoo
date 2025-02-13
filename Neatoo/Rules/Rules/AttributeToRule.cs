using Neatoo.Core;
using System;
using System.ComponentModel.DataAnnotations;

namespace Neatoo.Rules.Rules;


public interface IAttributeToRule
{
    IRule<T> GetRule<T>(IPropertyInfo r, Type attribute) where T : IValidateBase;
}

public class AttributeToRule : IAttributeToRule
{

    public AttributeToRule()
    {

    }

    public IRule<T> GetRule<T>(IPropertyInfo r, Type attribute) where T : IValidateBase
    {
        if (attribute == typeof(RequiredAttribute))
        {
            return new RequiredRule<T>(r);
        }
        return null;
    }

}
