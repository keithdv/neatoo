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
        ISendReceivePortal<T> SendReceivePortal { get; }
    }

    public class EditBaseServices<T> : ValidateBaseServices<T>, IEditBaseServices<T>
        where T : EditBase<T>
    {

        public IEditPropertyValueManager<T> EditPropertyValueManager { get; }
        public ISendReceivePortal<T> SendReceivePortal { get; }

        public EditBaseServices(ISendReceivePortal<T> sendReceivePortal) : base(){
            EditPropertyValueManager = new EditPropertyValueManager<T>(RegisteredPropertyManager, new DefaultFactory(), new ValuesDiffer());
            SendReceivePortal = sendReceivePortal;  
        }

        public EditBaseServices(IEditPropertyValueManager<T> registeredPropertyValueManager, IRegisteredPropertyManager<T> registeredPropertyManager, IRuleManager<T> ruleManager, ISendReceivePortal<T> sendReceivePortal)
            : base(registeredPropertyValueManager, registeredPropertyManager, ruleManager)
        {
            EditPropertyValueManager = registeredPropertyValueManager;
            SendReceivePortal = sendReceivePortal;
        }
    }
}
