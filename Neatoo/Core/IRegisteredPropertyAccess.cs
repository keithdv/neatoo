namespace Neatoo.Core;

public interface IRegisteredPropertyAccess
{
    IPropertyValue ReadPropertyValue(string propertyName);
    IPropertyValue ReadPropertyValue(IRegisteredProperty registeredProperty);
    P ReadProperty<P>(IRegisteredProperty<P> registeredProperty);
    void SetProperty<P>(IRegisteredProperty<P> registeredProperty, P value);
    void LoadProperty<P>(IRegisteredProperty<P> registeredProperty, P value);
}
