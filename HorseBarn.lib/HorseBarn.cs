using Neatoo;
using Neatoo.Portal;
using System.Net.Http.Headers;

namespace HorseBarn.lib
{
    internal class HorseBarn : Neatoo.EditBase<HorseBarn>, IHorseBarn
    {
        private readonly ISendReceivePortalChild<ILightHorse> lightHorsePortal;
        private readonly ISendReceivePortalChild<IHeavyHorse> heavyHorsePortal;

        public HorseBarn(IEditBaseServices<HorseBarn> services,
                            ISendReceivePortalChild<ILightHorse> lightHorsePortal,
                            ISendReceivePortalChild<IHeavyHorse> heavyHorsePortal) : base(services)
        {
            this.lightHorsePortal = lightHorsePortal;
            this.heavyHorsePortal = heavyHorsePortal;
        }

        public IPasture Pasture { get => Getter<IPasture>(); private set => Setter(value); }

        [Create]
        private async Task Create(ISendReceivePortalChild<IPasture> pasturePortal)
        {
            Pasture = await pasturePortal.CreateChild();
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
            this.Pasture.Horses.Add(horse);
            return horse;
        }
    }
}
