using HorseBarn.lib.Horse;
using Neatoo;

namespace HorseBarn.lib.Cart
{

    public interface IRacingChariot : ICart
    {

    }

    internal class RacingChariot : Cart<RacingChariot, ILightHorse>, IRacingChariot
    {
        public RacingChariot(IEditBaseServices<RacingChariot> services, ICartNumberOfHorsesRule cartNumberOfHorsesRule) : base(services, cartNumberOfHorsesRule)
        {
        }

        protected override CartType CartType => CartType.RacingChariot;
    }
}
