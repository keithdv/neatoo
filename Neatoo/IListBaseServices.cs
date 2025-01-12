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
        IReadPortalChild<I> ReadPortal { get; }
    }

    public class ListBaseServices<T, I> : IListBaseServices<T, I>
        where T : ListBase<T, I>
        where I : IBase
    {

        public ListBaseServices(IPropertyValueManager<T> registeredPropertyDataManager, IReadPortalChild<I> readPortal)
        {
            PropertyValueManager = registeredPropertyDataManager;
            ReadPortal = readPortal;
        }

        public IPropertyValueManager<T> PropertyValueManager { get; }
        public IReadPortalChild<I> ReadPortal { get; }

    }
}
