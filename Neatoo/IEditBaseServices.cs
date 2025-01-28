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

        IEditPropertyManager EditPropertyManager => (IEditPropertyManager)PropertyManager;
        IReadWritePortal<T> ReadWritePortal { get; }
    }

    public class EditBaseServices<T> : ValidateBaseServices<T>, IEditBaseServices<T>
        where T : EditBase<T>
    {

        public IReadWritePortal<T> ReadWritePortal { get; }

        public EditBaseServices(IReadWritePortal<T> readWritePortal) : base(){
            PropertyManager = new EditPropertyManager(RegisteredPropertyManager, new DefaultFactory());
            ReadWritePortal = readWritePortal;  
        }

        public EditBaseServices(Func<IRegisteredPropertyManager, IEditPropertyManager> propertyManager, IRegisteredPropertyManager<T> registeredPropertyManager, CreateRuleManager<T> ruleManager, IReadWritePortal<T> readWritePortal)
            : base(registeredPropertyManager, ruleManager)
        {
            PropertyManager = propertyManager(registeredPropertyManager);
            ReadWritePortal = readWritePortal;
        }
    }
}
