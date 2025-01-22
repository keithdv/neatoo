using HorseBarn.lib.Cart;
using HorseBarn.lib.Horse;
using Neatoo;

namespace HorseBarn.lib
{
    public interface IHorseBarn : IEditBase
    {
        internal int? Id { get; }
        IPasture Pasture { get; }
        IReadOnlyListBase<ICart> Carts { get; }
        IEnumerable<IHorse> Horses { get; }
        Task<IHorse> AddNewHorse(IHorseCriteria horseCriteria);
        Task<IRacingChariot> AddRacingChariot();
        Task<IWagon> AddWagon();
        void MoveHorseToCart(IHorse horse, ICart cart);
        void MoveHorseToPasture(IHorse horse);
    }
}