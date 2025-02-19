using Neatoo.Core;
using Neatoo.Portal.Internal;
using Neatoo.Rules;

namespace Neatoo;

/// <summary>
/// Wrap the NeatooBase services into an interface so that 
/// the inheriting classes don't need to list all services
/// and services can be added
/// REGISTERED IN DI CONTAINER
/// </summary>
public interface IEditBaseServices<T> : IValidateBaseServices<T>
    where T : EditBase<T>
{

    IEditPropertyManager EditPropertyManager { get; }
    Save<T> Save { get; }
}

public class EditBaseServices<T> : ValidateBaseServices<T>, IEditBaseServices<T>
    where T : EditBase<T>
{
    public Save<T> Save { get; }

    public IEditPropertyManager EditPropertyManager { get; }

    public new IValidatePropertyManager<IValidateProperty> ValidatePropertyManager => EditPropertyManager;

    public new IPropertyManager<IProperty> PropertyManager => EditPropertyManager;

    public EditBaseServices(Save<T> save) : base() {

        PropertyInfoList = new PropertyInfoList<T>((System.Reflection.PropertyInfo pi) => new PropertyInfoWrapper(pi));

        EditPropertyManager = new EditPropertyManager(PropertyInfoList, new DefaultFactory());
        Save = save;  
    }

    public EditBaseServices(CreateEditPropertyManager propertyManager, IPropertyInfoList<T> propertyInfoList, RuleManagerFactory<T> ruleManager, Save<T> save)
    {
        PropertyInfoList = propertyInfoList;
        this.ruleManagerFactory = ruleManager;
        EditPropertyManager = propertyManager(propertyInfoList);
        Save = save;
    }

}
