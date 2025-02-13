using HorseBarn.lib.Horse;
using Neatoo;
using Neatoo.Portal;

namespace HorseBarn.lib.Cart;



public interface ICartList : IEditListBase<ICart>

{

    internal Task RemoveHorse(IHorse horse);
}

internal class CartList : EditListBase<CartList, ICart>, ICartList
{
    public CartList(IEditListBaseServices<CartList, ICart> services) : base(services)
    {

    }

    public async Task RemoveHorse(IHorse horse)
    {
        foreach(var c in this)
        {
            await c.RemoveHorse(horse);
        }
    }

#if !CLIENT

    [FetchChild]
    public async Task FetchChild(ICollection<Dal.Ef.Cart> carts, INeatooPortal<IRacingChariot> racingChariotPortal, INeatooPortal<IWagon> wagonPortal)
    {
        foreach (var cart in carts)
        {
            if (cart.CartType == (int)CartType.RacingChariot)
            {
                Add(await racingChariotPortal.FetchChild(cart));
            }
            else if(cart.CartType == (int)CartType.Wagon)
            {
                Add(await wagonPortal.FetchChild(cart));
            }
        }
    }

#endif
}
