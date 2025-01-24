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

        IEditPropertyValueManager<T> EditPropertyValueManager { get; }
        IReadWritePortal<T> ReadWritePortal { get; }
    }

    public class EditBaseServices<T> : ValidateBaseServices<T>, IEditBaseServices<T>
        where T : EditBase<T>
    {

        public IEditPropertyValueManager<T> EditPropertyValueManager { get; }
        public IReadWritePortal<T> ReadWritePortal { get; }

        public EditBaseServices(IReadWritePortal<T> readWritePortal) : base(){
            EditPropertyValueManager = new EditPropertyValueManager<T>(RegisteredPropertyManager, new DefaultFactory());
            ReadWritePortal = readWritePortal;  
        }

        public EditBaseServices(IEditPropertyValueManager<T> registeredPropertyValueManager, IRegisteredPropertyManager<T> registeredPropertyManager, IRuleManager<T> ruleManager, IReadWritePortal<T> readWritePortal)
            : base(registeredPropertyValueManager, registeredPropertyManager, ruleManager)
        {
            EditPropertyValueManager = registeredPropertyValueManager;
            ReadWritePortal = readWritePortal;
        }
    }
}
