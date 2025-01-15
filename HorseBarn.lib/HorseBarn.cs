using HorseBarn.lib.Cart;
using HorseBarn.lib.Horse;
using Neatoo;
using Neatoo.Portal;

#if !CLIENT
using HorseBarn.Dal.Ef;
#endif

namespace HorseBarn.lib
{
    internal partial class HorseBarn : CustomEditBase<HorseBarn>, IHorseBarn
    {
        private readonly IReadWritePortalChild<ILightHorse> lightHorsePortal;
        private readonly IReadWritePortalChild<IHeavyHorse> heavyHorsePortal;
        private readonly IReadWritePortalChild<IRacingChariot> racingChariotPortal;
        private readonly IReadWritePortalChild<IWagon> wagonPortal;

        public HorseBarn(IEditBaseServices<HorseBarn> services,
                            IReadWritePortalChild<ILightHorse> lightHorsePortal,
                            IReadWritePortalChild<IHeavyHorse> heavyHorsePortal,
                            IReadWritePortalChild<IRacingChariot> racingChariotPortal,
                            IReadWritePortalChild<IWagon> wagonPortal) : base(services)
        {
            this.lightHorsePortal = lightHorsePortal;
            this.heavyHorsePortal = heavyHorsePortal;
            this.racingChariotPortal = racingChariotPortal;
            this.wagonPortal = wagonPortal;
        }

        public IPasture Pasture { get => Getter<IPasture>(); private set => Setter(value); }
        public ICartList Carts {  get => Getter<ICartList>(); private set => Setter(value); }
        public IEnumerable<IHorse> Horses => Carts.SelectMany(c => c.Horses).Union(Pasture.HorseList);

        IReadOnlyListBase<ICart> IHorseBarn.Carts => this.Carts;

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

            await CheckAllRules(); // TODO: This should be automatically done by the Neatoo framework
        }

        public async Task MoveHorseToPasture(IHorse horse)
        {
            Carts.RemoveHorse(horse);

            if (!Pasture.HorseList.Contains(horse))
            {
                Pasture.HorseList.Add(horse);
            }

            await CheckAllRules(); // TODO: This should be automatically done by the Neatoo framework
        }


#if !CLIENT

        [Create]
        public async Task Create(IReadWritePortalChild<IPasture> pasturePortal, IReadWritePortalChild<ICartList> cartListPortal)
        {
            this.Pasture = await pasturePortal.CreateChild();
            this.Carts = await cartListPortal.CreateChild();
        }

        [Insert]
        public async Task Update(HorseBarnContext horseBarnContext,
                                IReadWritePortalChild<IPasture> pasturePortal,
                                IReadWritePortalChild<ICartList> cartPortal)
        {
            var horseBarn = new Dal.Ef.HorseBarn();

            horseBarn.PropertyChanged += HandleIdPropertyChanged;

            await pasturePortal.UpdateChild(this.Pasture, horseBarn);
            await cartPortal.UpdateChild(this.Carts, horseBarn);

            horseBarnContext.HorseBarns.Add(horseBarn);

            await horseBarnContext.SaveChangesAsync();
        }

#endif

    }
}
