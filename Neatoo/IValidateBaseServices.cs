using Neatoo.AuthorizationRules;
using Neatoo.Core;
using Neatoo.Portal;
using Neatoo.Rules;
using Neatoo.Rules.Rules;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Neatoo
{
    /// <summary>
    /// Wrap the NeatooBase services into an interface so that 
    /// the inheriting classes don't need to list all services
    /// and services can be added
    /// </summary>
    public interface IValidateBaseServices<T> : IBaseServices<T>
        where T : ValidateBase<T>
    {
        IRuleManager<T> CreateRuleManager(T target);

        IValidatePropertyManager<IValidateProperty> ValidatePropertyManager { get; }
    }

    public class ValidateBaseServices<T> : IValidateBaseServices<T>
        where T : ValidateBase<T>
    {
        public IRegisteredPropertyManager<T> RegisteredPropertyManager { get; }
        public IValidatePropertyManager<IValidateProperty> ValidatePropertyManager { get; }

        public RuleManagerFactory<T> ruleManagerFactory { get; }

        public IPropertyManager<IProperty> PropertyManager => ValidatePropertyManager;

        public ValidateBaseServices() : base()
        {
            RegisteredPropertyManager = new RegisteredPropertyManager<T>((System.Reflection.PropertyInfo pi) => new RegisteredProperty(pi));
            this.ValidatePropertyManager = new ValidatePropertyManager<IValidateProperty>(RegisteredPropertyManager, new DefaultFactory());
            this.ruleManagerFactory = new RuleManagerFactory<T>(new AttributeToRule(rp => new RequiredRule(rp)));
        }

        public ValidateBaseServices(IRegisteredPropertyManager<T> registeredPropertyManager, RuleManagerFactory<T> createRuleManager)
        {
            this.ruleManagerFactory = createRuleManager;
        }

        public ValidateBaseServices(CreateValidatePropertyManager  validatePropertyManager, IRegisteredPropertyManager<T> registeredPropertyManager, RuleManagerFactory<T> createRuleManager)
        {
            RegisteredPropertyManager = new RegisteredPropertyManager<T>((System.Reflection.PropertyInfo pi) => new RegisteredProperty(pi));
            ValidatePropertyManager = validatePropertyManager(RegisteredPropertyManager);
            this.ruleManagerFactory = createRuleManager;
        }



        public IRuleManager<T> CreateRuleManager(T target)
        {
            return ruleManagerFactory.CreateRuleManager(target, this.RegisteredPropertyManager);
        }

    }
}
