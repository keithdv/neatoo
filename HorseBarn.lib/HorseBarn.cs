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

        public async Task<IRacingChariot> AddRacingChariot()
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

        public async Task<IHorse> AddNewHorse(IHorseCriteria horseCriteria)
        {
            IHorse horse;

            if (Horse<LightHorse>.IsLightHorse(horseCriteria.Breed))
            {
                horse = await lightHorsePortal.CreateChild(horseCriteria);
            }
            else if (Horse<HeavyHorse>.IsHeavyHorse(horseCriteria.Breed))
            {
                horse = await heavyHorsePortal.CreateChild(horseCriteria);
            }
            else
            {
                throw new Exception($"Cannot create child horse for breed {horseCriteria.Breed}");
            }

            await horse.CheckAllRules();
            this.Pasture.HorseList.Add(horse);
            return horse;
        }

        public async Task MoveHorseToCart(IHorse horse, ICart cart)
        {
            Pasture.RemoveHorse(horse);
            Carts.RemoveHorse(horse);

            cart.AddHorse(horse);

            await CheckAllRules(); // TODO: This should be automatic
        }

        public async Task MoveHorseToPasture(IHorse horse)
        {
            Carts.RemoveHorse(horse);

            if (!Pasture.HorseList.Contains(horse))
            {
                Pasture.HorseList.Add(horse);
            }

            await CheckAllRules(); // TODO: This should be automatic
        }
    }
}
