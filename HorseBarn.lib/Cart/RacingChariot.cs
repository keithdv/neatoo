using HorseBarn.lib.Horse;
using Neatoo;
using Neatoo.Portal;
using Neatoo.Rules.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorseBarn.lib.Cart
{

    public interface IRacingChariot : ICart
    {

    }

    internal class RacingChariot : Cart<RacingChariot, ILightHorse>, IRacingChariot
    {
        public RacingChariot(EditBaseServices<RacingChariot> services, ICartNumberOfHorsesRule cartNumberOfHorsesRule) : base(services, cartNumberOfHorsesRule)
        {
        }

        protected override CartType CartType => CartType.RacingChariot;
    }
}
