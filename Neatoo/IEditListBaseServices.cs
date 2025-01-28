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
    //public interface EditListBaseServices<I> : IValidateListBaseServices<I>
    //    where I : IEditBase
    //{
    //    IReadWritePortalChild<I> ReadWritePortalChild { get; }
    //}

    public class EditListBaseServices<I> : ValidateListBaseServices<I>
        where I : IEditBase
    {

        public IReadWritePortalChild<I> ReadWritePortalChild { get; }

        public EditListBaseServices(IReadWritePortalChild<I> readWritePortalChild)
            : base(readWritePortalChild)
        {
            ReadWritePortalChild = readWritePortalChild;
        }
    }
}
