using Neatoo.Core;

namespace Neatoo
{
    public interface IFactory
    {
        Property<P> CreateProperty<P>(IRegisteredProperty registeredProperty);
        ValidateProperty<P> CreateValidateProperty<P>(IRegisteredProperty registeredProperty);
        EditProperty<P> CreateEditProperty<P>(IRegisteredProperty registeredProperty);
    }

}