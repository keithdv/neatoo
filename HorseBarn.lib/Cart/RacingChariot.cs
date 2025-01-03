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

    public interface IRacingChariot : ICart<ILightHorse>, ICart
    {

    }

    internal class RacingChariot : Cart<RacingChariot, ILightHorse>, IRacingChariot
    {
        public RacingChariot(IEditBaseServices<RacingChariot> services) : base(services)
        {
        }
    }

    public interface IRacingChariotList : ICartList<IRacingChariot>
    {

    }

    internal class RacingChariotList : CartList<RacingChariotList, IRacingChariot>, IRacingChariotList
    {
        public RacingChariotList(IEditListBaseServices<RacingChariotList, IRacingChariot> services) : base(services)
        {
        }
    }
}
