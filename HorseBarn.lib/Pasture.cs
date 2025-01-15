using HorseBarn.lib.Horse;
using Neatoo;
using Neatoo.Portal;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            await CheckAllRules();
        }

        [InsertChild]
        public async Task InsertChild(Dal.Ef.HorseBarn horseBarn, IReadWritePortalChild<IHorseList> horseListPortal)
        {
            var pasture = new Dal.Ef.Pasture();
            pasture.PropertyChanged += HandleIdPropertyChanged;
            horseBarn.Pasture = pasture;
            await horseListPortal.UpdateChild(HorseList, pasture);
        }



#endif

    }
}
