using Neatoo.Core;
using Neatoo.Portal;
using Neatoo.Rules;

namespace Neatoo
{
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
        IReadWritePortal<T> ReadWritePortal { get; }
    }

    public class EditBaseServices<T> : IEditBaseServices<T>
        where T : EditBase<T>
    {
        private readonly RuleManagerFactory<T> ruleManager;

        public IReadWritePortal<T> ReadWritePortal { get; }

        public IEditPropertyManager EditPropertyManager { get; }

        public IValidatePropertyManager<IValidateProperty> ValidatePropertyManager => EditPropertyManager;

        public IPropertyManager<IProperty> PropertyManager => EditPropertyManager;

        public IPropertyInfoBag<T> PropertyInfoList { get; }

        public EditBaseServices(IReadWritePortal<T> readWritePortal) : base() {

            PropertyInfoList = new PropertyInfoBag<T>((System.Reflection.PropertyInfo pi) => new NeatooPropertyInfoWrapper(pi));

            EditPropertyManager = new EditPropertyManager(PropertyInfoList, new DefaultFactory());
            ReadWritePortal = readWritePortal;  
        }

        public EditBaseServices(CreateEditPropertyManager propertyManager, IPropertyInfoBag<T> propertyInfoList, RuleManagerFactory<T> ruleManager, IReadWritePortal<T> readWritePortal)
        {
            PropertyInfoList = propertyInfoList;
            this.ruleManager = ruleManager;
            EditPropertyManager = propertyManager(propertyInfoList);
            ReadWritePortal = readWritePortal;
        }

        public IRuleManager<T> CreateRuleManager(T target)
        {
            return ruleManager.CreateRuleManager(target, this.PropertyInfoList);
        }
    }
}
