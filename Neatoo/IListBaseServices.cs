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
    //public interface ListBaseServices<I>
    //    where I : IBase
    //{
    //    IReadPortalChild<I> ReadPortal { get; }
    //}

    public class ListBaseServices<I>
        where I : IBase
    {

        public ListBaseServices()        {        }

        public ListBaseServices(IReadPortalChild<I> readPortal)
        {
            ReadPortal = readPortal;
        }

        public IReadPortalChild<I> ReadPortal { get; }
    }
}
