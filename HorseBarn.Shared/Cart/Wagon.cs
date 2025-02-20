﻿using HorseBarn.lib.Horse;
using Neatoo;
using Neatoo.Portal;

namespace HorseBarn.lib.Cart;

public interface IWagon : ICart
{

}

[Factory]
internal class Wagon : Cart<Wagon, IHeavyHorse>, IWagon
{
    public Wagon(IEditBaseServices<Wagon> services, ICartNumberOfHorsesRule cartNumberOfHorsesRule) : base(services, cartNumberOfHorsesRule)
    {
    }

    protected override CartType CartType => CartType.Wagon;
}
