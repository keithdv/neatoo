using Neatoo.Core;
using Neatoo.Rules;

namespace Neatoo
{
    public interface IFactory
    {
        PropertyValue<P> CreatePropertyValue<P>(IRegisteredProperty<P> registeredProperty, P value);
        ValidatePropertyValue<P> CreateValidatePropertyValue<P>(IRegisteredProperty<P> registeredProperty, P value);
        EditPropertyValue<P> CreateEditPropertyValue<P>(IRegisteredProperty<P> registeredProperty, P value);
    }

}