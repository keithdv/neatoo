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
    //public interface IValidateListBaseServices<I> : ListBaseServices<I>
    //    where I : IValidateBase
    //{
    //}



    public class ValidateListBaseServices<I> : ListBaseServices<I>
        where I : IValidateBase
    {

        public ValidateListBaseServices(IReadPortalChild<I> portal) : base(portal)
        {
        }
    }
}
