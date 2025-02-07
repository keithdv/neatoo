using Neatoo.AuthorizationRules;
using Neatoo.Core;
using Neatoo.Portal;
using Neatoo.Rules;
using System;
using System.Collections.Generic;
using System.Text;

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

        public IRegisteredPropertyManager<T> RegisteredPropertyManager { get; }

        public EditBaseServices(IReadWritePortal<T> readWritePortal) : base() {

            RegisteredPropertyManager = new RegisteredPropertyManager<T>((System.Reflection.PropertyInfo pi) => new RegisteredProperty(pi));

            EditPropertyManager = new EditPropertyManager(RegisteredPropertyManager, new DefaultFactory());
            ReadWritePortal = readWritePortal;  
        }

        public EditBaseServices(CreateEditPropertyManager propertyManager, IRegisteredPropertyManager<T> registeredPropertyManager, RuleManagerFactory<T> ruleManager, IReadWritePortal<T> readWritePortal)
        {
            RegisteredPropertyManager = registeredPropertyManager;
            this.ruleManager = ruleManager;
            EditPropertyManager = propertyManager(registeredPropertyManager);
            ReadWritePortal = readWritePortal;
        }

        public IRuleManager<T> CreateRuleManager(T target)
        {
            return ruleManager.CreateRuleManager(target, this.RegisteredPropertyManager);
        }
    }
}
