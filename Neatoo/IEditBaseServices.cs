using Neatoo.Core;
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
    INeatooPortal<T> ReadWritePortal { get; }
}

public class EditBaseServices<T> : ValidateBaseServices<T>, IEditBaseServices<T>
    where T : EditBase<T>
{
    public INeatooPortal<T> ReadWritePortal { get; }

    public IEditPropertyManager EditPropertyManager { get; }

    public new IValidatePropertyManager<IValidateProperty> ValidatePropertyManager => EditPropertyManager;

    public new IPropertyManager<IProperty> PropertyManager => EditPropertyManager;

    public EditBaseServices(INeatooPortal<T> readWritePortal) : base() {

        PropertyInfoList = new PropertyInfoList<T>((System.Reflection.PropertyInfo pi) => new PropertyInfoWrapper(pi));

        EditPropertyManager = new EditPropertyManager(PropertyInfoList, new DefaultFactory());
        ReadWritePortal = readWritePortal;  
    }

    public EditBaseServices(CreateEditPropertyManager propertyManager, IPropertyInfoList<T> propertyInfoList, RuleManagerFactory<T> ruleManager, INeatooPortal<T> readWritePortal)
    {
        PropertyInfoList = propertyInfoList;
        this.ruleManagerFactory = ruleManager;
        EditPropertyManager = propertyManager(propertyInfoList);
        ReadWritePortal = readWritePortal;
    }

}
