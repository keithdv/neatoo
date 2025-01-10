using Neatoo;
using Neatoo.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorseBarn.lib.Horse
{
    internal class HorseList : HorseList<HorseList, IHorse>, IHorseList
    {
        public HorseList(IEditListBaseServices<HorseList, IHorse> services) : base(services)
        {

        }

    }

    internal class HorseList<H> : HorseList<HorseList<H>, H>, IHorseList<H>
        where H : class, IHorse
    {
        public HorseList(IEditListBaseServices<HorseList<H>, H> services) : base(services)
        {
        }
    }

    internal class HorseList<L, I> : EditListBase<L, I>, IHorseList<I>
        where L : HorseList<L, I>
        where I : class, IHorse
    {
        public HorseList(IEditListBaseServices<L, I> services) : base(services)
        {
        }

        public void RemoveHorse(IHorse horse)
        {
            if (horse is I myhorse)
            {
                if (Contains(myhorse))
                {
                    Remove(myhorse);
                }
            } else
            {
                throw new Exception($"Horse ${horse.GetType().FullName} is not of type {typeof(I).FullName}");
            }
        }
    }
}
