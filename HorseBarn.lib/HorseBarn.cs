using HorseBarn.lib.Cart;
using HorseBarn.lib.Horse;
using Neatoo;
using Neatoo.Portal;

#if !CLIENT
using Microsoft.EntityFrameworkCore;
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

            if (IHorse.IsLightHorse(horseCriteria.Breed))
            {
                horse = await lightHorsePortal.CreateChild(horseCriteria);
            }
            else if (IHorse.IsHeavyHorse(horseCriteria.Breed))
            {
                horse = await heavyHorsePortal.CreateChild(horseCriteria);
            }
            else
            {
                throw new Exception($"Cannot create child horse for breed {horseCriteria.Breed}");
            }
            
            this.Pasture.HorseList.Add(horse);
            await horse.CheckAllRules();
            return horse;
        }

        public async Task MoveHorseToCart(IHorse horse, ICart cart)
        {
            Pasture.RemoveHorse(horse);
            await Carts.RemoveHorse(horse);

            await cart.AddHorse(horse);
        }

        public async Task MoveHorseToPasture(IHorse horse)
        {
            await Carts.RemoveHorse(horse);

            if (!Pasture.HorseList.Contains(horse))
            {
                Pasture.HorseList.Add(horse);
            }
        }


#if !CLIENT

        [Create]
        public async Task Create(IReadWritePortalChild<IPasture> pasturePortal, IReadWritePortalChild<ICartList> cartListPortal)
        {
            this.Pasture = await pasturePortal.CreateChild();
            this.Carts = await cartListPortal.CreateChild();
        }

        [Fetch]
        public async Task Fetch(HorseBarnContext horseBarnContext,
                                IReadPortalChild<IPasture> pasturePortal,
                                IReadPortalChild<ICartList> cartPortal)
        {

            var horseBarn = await horseBarnContext.HorseBarns.FirstOrDefaultAsync();
            if (horseBarn != null)
            {
                this.Id = horseBarn.Id;
                this.Pasture = await pasturePortal.FetchChild(horseBarn.Pasture);
                this.Carts = await cartPortal.FetchChild(horseBarn.Carts);
            }
        }

        [Insert]
        public async Task Insert(HorseBarnContext horseBarnContext,
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

        [Update]
        public async Task Update(HorseBarnContext horseBarnContext,
                                IReadWritePortalChild<IPasture> pasturePortal,
                                IReadWritePortalChild<ICartList> cartPortal)
        {
            var horseBarn = await horseBarnContext.HorseBarns.FirstAsync(hb => hb.Id == this.Id);
            if (this.Pasture.IsModified)
            {
                await pasturePortal.UpdateChild(this.Pasture, horseBarn.Pasture);
            }

            if (this.Carts.IsModified)
            {
                await cartPortal.UpdateChild(this.Carts, horseBarn);
            }

            await horseBarnContext.SaveChangesAsync();
        }

#endif

    }
}
