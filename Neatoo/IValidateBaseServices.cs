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
    /// </summary>
    public interface IValidateBaseServices<T> : IBaseServices<T>
        where T : ValidateBase<T>
    {
        IValidatePropertyValueManager<T> ValidatePropertyValueManager { get; }

        IRuleManager<T> RuleManager { get; }

    }


    public class ValidateBaseServices<T> : BaseServices<T>, IValidateBaseServices<T>
        where T : ValidateBase<T>
    {
        public ValidateBaseServices() : base()
        {
            this.ValidatePropertyValueManager = new ValidatePropertyValueManager<T>(RegisteredPropertyManager, new DefaultFactory(), new ValuesDiffer());
            this.RuleManager = new RuleManager<T>(RegisteredPropertyManager);
        }

        public ValidateBaseServices(IValidatePropertyValueManager<T> registeredPropertyValueManager, IRegisteredPropertyManager<T> registeredPropertyManager, IRuleManager<T> ruleManager)
            : base(registeredPropertyValueManager, registeredPropertyManager)
        {
            this.ValidatePropertyValueManager = registeredPropertyValueManager;
            RuleManager = ruleManager;

        }

        public IValidatePropertyValueManager<T> ValidatePropertyValueManager { get; }
        public IRuleManager<T> RuleManager { get; }

        IPropertyValueManager<T> IBaseServices<T>.PropertyValueManager
        {
            get { return ValidatePropertyValueManager; }
        }

    }
}
