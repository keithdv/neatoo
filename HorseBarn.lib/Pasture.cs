using HorseBarn.lib.Horse;
using Neatoo;
using Neatoo.Portal;
using System.Diagnostics;

#if !CLIENT
using HorseBarn.Dal.Ef;
using Microsoft.EntityFrameworkCore;
#endif

namespace HorseBarn.lib
{
    internal class Pasture : CustomEditBase<Pasture>, IPasture
    {
        public Pasture(IEditBaseServices<Pasture> services) : base(services)
        {

        }

        public IHorseList HorseList {  get => Getter<IHorseList>(); private set => Setter(value); }

        public IReadOnlyListBase<IHorse> Horses => HorseList;

        public void RemoveHorse(IHorse horse)
        {
            HorseList.RemoveHorse(horse);
        }

#if !CLIENT

        [CreateChild]
        public async Task CreateChild(IReadWritePortalChild<IHorseList> horseListPortal)
        {
            HorseList = await horseListPortal.CreateChild(); 
            await RunAllRules();
        }

        [FetchChild]
        public async Task FetchChild(Dal.Ef.Pasture pasture, IReadPortalChild<IHorseList> horseListPortal)
        {
            this.Id = pasture.Id;

            this.HorseList = await horseListPortal.FetchChild(pasture.Horses);
        }

        [InsertChild]
        public async Task InsertChild(Dal.Ef.HorseBarn horseBarn, IReadWritePortalChild<IHorseList> horseListPortal)
        {
            var pasture = new Dal.Ef.Pasture();
            pasture.PropertyChanged += HandleIdPropertyChanged;
            horseBarn.Pasture = pasture;
            await horseListPortal.UpdateChild(HorseList, pasture);
        }

        [UpdateChild]
        public async Task UpdateChild(Dal.Ef.Pasture pasture, IReadWritePortalChild<IHorseList> horseListPortal)
        {
            Debug.Assert(pasture.Id == this.Id, "Unexpected Id");
            await horseListPortal.UpdateChild(HorseList, pasture);
        }

#endif

    }
}
