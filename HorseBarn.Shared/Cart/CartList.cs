#if !CLIENT
using HorseBarn.Dal.Ef;
#endif
using HorseBarn.lib.Horse;
using Neatoo;
using Neatoo.Portal;

namespace HorseBarn.lib.Cart;



public interface ICartList : IEditListBase<ICart>

{

    internal Task RemoveHorse(IHorse horse);
}

[Factory]
internal class CartList : EditListBase<CartList, ICart>, ICartList
{
    public CartList() : base()
    {

    }

    public async Task RemoveHorse(IHorse horse)
    {
        foreach(var c in this)
        {
            await c.RemoveHorse(horse);
        }
    }

    [Create]
    public void Create()
    {
    }

#if !CLIENT

    [Fetch]
    public async Task Fetch(ICollection<Dal.Ef.Cart> carts, [Service] RacingChariotFactory racingChariotPortal,[Service] WagonFactory wagonPortal)
    {
        foreach (var cart in carts)
        {
            if (cart.CartType == (int)CartType.RacingChariot)
            {
                Add(await racingChariotPortal.Fetch(cart));
            }
            else if(cart.CartType == (int)CartType.Wagon)
            {
                Add(await wagonPortal.Fetch(cart));
            }
        }
    }

    [Update]
    public async Task Update(Dal.Ef.HorseBarn horseBarn, [Service] RacingChariotFactory racingChariotPortal, [Service] WagonFactory wagonPortal)
    {
        async Task SaveCart(ICart cart)
        {
            if (cart is RacingChariot racingChariot)
            {
                await racingChariotPortal.Save(racingChariot, horseBarn);
            }
            else if (cart is Wagon wagon)
            {
                await wagonPortal.Save(wagon, horseBarn);
            }
        }

        foreach (var cart in this.DeletedList)
        {
            if (cart.IsDeleted)
            {
                await SaveCart(cart);
            }
        }

        DeletedList.Clear();

        foreach (var cart in this)
        {
            await SaveCart(cart);
        }
    }
#endif
}
