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

        IValidatePropertyManager ValidatePropertyManager => (IValidatePropertyManager)PropertyManager;
    }

    public class ValidateBaseServices<T> : BaseServices<T>, IValidateBaseServices<T>
        where T : ValidateBase<T>
    {
        public ValidateBaseServices() : base()
        {
            this.PropertyManager = new ValidatePropertyManager(RegisteredPropertyManager, new DefaultFactory());
            this.CreateRuleManagerDI = (t, r) => new RuleManager<T>(t, r, new AttributeToRule(rp => new RequiredRule(rp)));
        }

        public ValidateBaseServices(IRegisteredPropertyManager<T> registeredPropertyManager, CreateRuleManager<T> createRuleManager)
    : base(registeredPropertyManager)
        {
            this.CreateRuleManagerDI = createRuleManager;
        }

        public ValidateBaseServices(Func<IRegisteredPropertyManager, IValidatePropertyManager>  validatePropertyManager, IRegisteredPropertyManager<T> registeredPropertyManager, CreateRuleManager<T> createRuleManager)
            : base(registeredPropertyManager)
        {
            this.PropertyManager = validatePropertyManager(RegisteredPropertyManager);
            this.CreateRuleManagerDI = createRuleManager;
        }


        
        public CreateRuleManager<T> CreateRuleManagerDI { get; }

        public IRuleManager<T> CreateRuleManager(T target)
        {
            return CreateRuleManagerDI(target, this.RegisteredPropertyManager);
        }

    }
}
