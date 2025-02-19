﻿using HorseBarn.lib.Horse;
using Neatoo;
using Neatoo.Portal;

namespace HorseBarn.lib.Cart;


public interface IRacingChariot : ICart
{

}

[Factory]
internal class RacingChariot : Cart<RacingChariot, ILightHorse>, IRacingChariot
{
    public RacingChariot(IEditBaseServices<RacingChariot> services, ICartNumberOfHorsesRule cartNumberOfHorsesRule) : base(services, cartNumberOfHorsesRule)
    {
    }

    protected override CartType CartType => CartType.RacingChariot;
}
