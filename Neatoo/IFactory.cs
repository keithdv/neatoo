using Neatoo.Core;
using Neatoo.Rules;

namespace Neatoo
{
    public interface IFactory
    {
        PropertyValue<P> CreatePropertyValue<P>(IRegisteredProperty registeredProperty, IBase parent);
        ValidatePropertyValue<P> CreateValidatePropertyValue<P>(IRegisteredProperty registeredProperty, IBase parent);
        EditPropertyValue<P> CreateEditPropertyValue<P>(IRegisteredProperty registeredProperty, IBase parent);
    }

}