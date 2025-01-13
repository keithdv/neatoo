using HorseBarn.lib.Horse;
using Neatoo;
using Neatoo.Rules.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorseBarn.lib.Cart
{
    public interface IWagon : ICart<IHeavyHorse>
    {

    }

    internal class Wagon : Cart<Wagon, IHeavyHorse>, IWagon
    {
        public Wagon(IEditBaseServices<Wagon> services, ICartNumberOfHorsesRule cartNumberOfHorsesRule) : base(services, cartNumberOfHorsesRule)
        {
        }
    }
}
