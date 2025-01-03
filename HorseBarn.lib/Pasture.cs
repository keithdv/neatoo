using HorseBarn.lib.Horse;
using Neatoo;
using Neatoo.Portal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorseBarn.lib
{
    public class Pasture : EditBase<Pasture>, IPasture
    {
        public Pasture(IEditBaseServices<Pasture> services) : base(services)
        {
        }

        public Guid? Id { get => Getter<Guid?>(); set => Setter(value); }

        [Required]
        public string Name { get => Getter<string>(); set => Setter(value); }

        public IHorseList HorseList {  get => Getter<IHorseList>(); private set => Setter(value); }

        IEnumerable<IHorse> IPasture.Horses => HorseList;

        public void RemoveHorse(IHorse horse)
        {
            HorseList.RemoveHorse(horse);
        }

        [CreateChild]
        private async Task CreateChild(ISendReceivePortalChild<IHorseList> horseListPortal)
        {
            HorseList = await horseListPortal.CreateChild(); 
            await CheckAllRules();
        }


    }
}
