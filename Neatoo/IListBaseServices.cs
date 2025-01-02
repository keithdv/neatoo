using Neatoo.AuthorizationRules;
using Neatoo.Core;
using Neatoo.Portal;
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
    public interface IListBaseServices<T, I>
        where T : ListBase<T, I>
        where I : IBase
    {
        IPropertyValueManager<T> PropertyValueManager { get; }
        IReceivePortalChild<I> ReceivePortal { get; }
    }

    public class ListBaseServices<T, I> : IListBaseServices<T, I>
        where T : ListBase<T, I>
        where I : IBase
    {

        public ListBaseServices(IPropertyValueManager<T> registeredPropertyDataManager, IReceivePortalChild<I> receivePortal)
        {
            PropertyValueManager = registeredPropertyDataManager;
            ReceivePortal = receivePortal;
        }

        public IPropertyValueManager<T> PropertyValueManager { get; }
        public IReceivePortalChild<I> ReceivePortal { get; }

    }
}
