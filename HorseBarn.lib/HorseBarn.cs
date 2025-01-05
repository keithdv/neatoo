using HorseBarn.lib.Cart;
using HorseBarn.lib.Horse;
using Neatoo;
using Neatoo.Portal;
using System.Net.Http.Headers;

namespace HorseBarn.lib
{
    internal class HorseBarn : Neatoo.EditBase<HorseBarn>, IHorseBarn
    {
        private readonly ISendReceivePortalChild<ILightHorse> lightHorsePortal;
        private readonly ISendReceivePortalChild<IHeavyHorse> heavyHorsePortal;
        private readonly ISendReceivePortalChild<IRacingChariot> racingChariotPortal;
        private readonly ISendReceivePortalChild<IWagon> wagonPortal;

        public HorseBarn(IEditBaseServices<HorseBarn> services,
                            ISendReceivePortalChild<ILightHorse> lightHorsePortal,
                            ISendReceivePortalChild<IHeavyHorse> heavyHorsePortal,
                            ISendReceivePortalChild<IRacingChariot> racingChariotPortal,
                            ISendReceivePortalChild<IWagon> wagonPortal) : base(services)
        {
            this.lightHorsePortal = lightHorsePortal;
            this.heavyHorsePortal = heavyHorsePortal;
            this.racingChariotPortal = racingChariotPortal;
            this.wagonPortal = wagonPortal;
        }

        public IPasture Pasture { get => Getter<IPasture>(); private set => Setter(value); }
        public ICartList Carts {  get => Getter<ICartList>(); private set => Setter(value); }

        IReadOnlyListBase<ICart> IHorseBarn.Carts => this.Carts;

        [Create]
        public async Task Create(ISendReceivePortalChild<IPasture> pasturePortal, ISendReceivePortalChild<ICartList> cartListPortal)
        {
            this.Pasture = await pasturePortal.CreateChild();
            this.Carts = await cartListPortal.CreateChild();
        }

        public async Task<IRacingChariot> AddRacingCart()
        {
            var newCart = await racingChariotPortal.CreateChild();
            this.Carts.Add(newCart);
            return newCart;
        }

        public async Task<IWagon> AddWagon()
        {
            var newCart = await wagonPortal.CreateChild();
            this.Carts.Add(newCart);
            return newCart;
        }

        public async Task<IHorse> AddNewHorse(Breed breed)
        {
            IHorse horse;

            if (Horse<LightHorse>.IsLightHorse(breed))
            {
                horse = await lightHorsePortal.CreateChild(breed);
            }
            else if (Horse<HeavyHorse>.IsHeavyHorse(breed))
            {
                horse = await heavyHorsePortal.CreateChild(breed);
            }
            else
            {
                throw new Exception($"Cannot create child horse for breed {breed}");
            }

            await horse.CheckAllRules();
            this.Pasture.HorseList.Add(horse);
            return horse;
        }

        public async Task MoveHorseToCart<H>(H horse, ICart<H> cart) where H : IHorse
        {
            Pasture.RemoveHorse(horse);
            Carts.RemoveHorse(horse);

            cart.HorseList.Add(horse);

            await cart.CheckAllRules(); // TODO: This should be automatic
        }
    }
}
