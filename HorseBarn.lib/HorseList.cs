using Neatoo;
using Neatoo.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorseBarn.lib
{
    public class HorseList : EditListBase<HorseList, IHorse>, IHorseList
    {
        public HorseList(IEditListBaseServices<HorseList, IHorse> services) : base(services)
        {
        }

        [CreateChild]
        private void CreateChild()
        {
        }
    }
}
