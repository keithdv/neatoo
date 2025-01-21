namespace Neatoo.Core;

public interface IRegisteredPropertyAccess
{
    IPropertyValue ReadPropertyValue(string propertyName);
    IPropertyValue ReadPropertyValue(IRegisteredProperty registeredProperty);
    P ReadProperty<P>(IRegisteredProperty registeredProperty);
    void SetProperty<P>(IRegisteredProperty registeredProperty, P value);
    void LoadProperty<P>(IRegisteredProperty registeredProperty, P value);
}
