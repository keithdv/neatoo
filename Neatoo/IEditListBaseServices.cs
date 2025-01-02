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
    public interface IEditListBaseServices<T, I> : IValidateListBaseServices<T, I>
        where T : EditListBase<T, I>
        where I : IEditBase
    {
        IEditPropertyValueManager<T> EditPropertyValueManager { get; }
        ISendReceivePortalChild<I> SendReceivePortalChild { get; }
        ISendReceivePortal<T> SendReceivePortal { get; }
    }

    public class EditListBaseServices<T, I> : ValidateListBaseServices<T, I>, IEditListBaseServices<T, I>
        where T : EditListBase<T, I>
        where I : IEditBase
    {

        public IEditPropertyValueManager<T> EditPropertyValueManager { get; }
        public ISendReceivePortalChild<I> SendReceivePortalChild { get; }
        public ISendReceivePortal<T> SendReceivePortal { get; }

        public EditListBaseServices(IEditPropertyValueManager<T> registeredPropertyManager,
                                        ISendReceivePortalChild<I> sendReceivePortalChild,
                                        ISendReceivePortal<T> sendReceivePortal,
                                        IRuleManager<T> ruleManager)
            : base(registeredPropertyManager, sendReceivePortalChild, ruleManager)
        {
            EditPropertyValueManager = registeredPropertyManager;
            SendReceivePortalChild = sendReceivePortalChild;
            SendReceivePortal = sendReceivePortal;
        }
    }
}
