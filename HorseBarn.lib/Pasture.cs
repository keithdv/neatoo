using Neatoo;
using Neatoo.Portal;
using System;
using System.Collections.Generic;
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
        public string Name { get => Getter<string>(); set => Setter(value); }

        public IHorseList Horses {  get => Getter<IHorseList>(); private set => Setter(value); }

        [CreateChild]
        private async Task CreateChild(ISendReceivePortalChild<IHorseList> horseListPortal)
        {
            Horses = await horseListPortal.CreateChild(); 
        }
    }
}
