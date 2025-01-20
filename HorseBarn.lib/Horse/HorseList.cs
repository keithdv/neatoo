using HorseBarn.lib.Cart;
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
            }
        }

#if !CLIENT

        [InsertChild]
        [UpdateChild]
        public async Task InsertChild(Dal.Ef.Pasture pasture, IReadWritePortalChild<IHorse> horsePortal)
        {
            foreach (var horse in this)
            {
                await horsePortal.UpdateChild(horse, pasture);
            }
        }

        [InsertChild]
        [UpdateChild]
        public async Task InsertChild(Dal.Ef.Cart cart, IReadWritePortalChild<IHorse> horsePortal)
        {
            foreach (var horse in this)
            {
                await horsePortal.UpdateChild(horse, cart);
            }
        }

        [FetchChild] 
        public async Task FetchChild(ICollection<Dal.Ef.Horse> horses,
                                        IReadPortalChild<ILightHorse> lightHorsePortal,
                                        IReadPortalChild<IHeavyHorse> heavyHorsePortal)
        {
            foreach (var horse in horses)
            {
                if(IHorse.IsLightHorse((Breed) horse.Breed))
                {
                    var h = (I) await lightHorsePortal.FetchChild(horse);
                    Add(h);
                }
                else
                {
                    var h = (I) await heavyHorsePortal.FetchChild(horse);
                    Add(h);
                }
            }
        }


        
#endif
    }
}
