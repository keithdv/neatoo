using HorseBarn.lib.Cart;
using HorseBarn.lib.Horse;
using Neatoo;

namespace HorseBarn.lib
{
    public interface IHorseBarn : IEditBase
    {
        IPasture Pasture { get; }
        IReadOnlyListBase<ICart> Carts { get; }
        Task<IHorse> AddNewHorse(Breed breed);
        Task<IRacingChariot> AddRacingCart();
        Task<IWagon> AddWagon();
        Task MoveHorseToCart<H>(H horse, ICart<H> cart) where H : IHorse;
    }
}