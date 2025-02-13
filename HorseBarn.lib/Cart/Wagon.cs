using HorseBarn.lib.Horse;
using Neatoo;

namespace HorseBarn.lib.Cart;

public interface IWagon : ICart
{

}

internal class Wagon : Cart<Wagon, IHeavyHorse>, IWagon
{
    public Wagon(EditBaseServices<Wagon> services, ICartNumberOfHorsesRule cartNumberOfHorsesRule) : base(services, cartNumberOfHorsesRule)
    {
    }

    protected override CartType CartType => CartType.Wagon;
}
