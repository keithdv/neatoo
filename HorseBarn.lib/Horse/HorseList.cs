using Neatoo;
using Neatoo.Portal;

namespace HorseBarn.lib.Horse
{
    //internal class HorseList<H> : HorseList<HorseList, Horse<H>>, IHorseList
    //{
    //    public HorseList(IEditListBaseServices<HorseList, IHorse> services) : base(services)
    //    {

    //    }

    //}

    internal class HorseList : EditListBase<HorseList, IHorse>, IHorseList
    {
        public HorseList(IEditListBaseServices<HorseList, IHorse> services) : base(services)
        {
        }

        public void RemoveHorse(IHorse horse)
        {
            if (Contains(horse))
            {
                Remove(horse);
            }
        }

#if !CLIENT

        [FetchChild] 
        public async Task FetchChild(ICollection<Dal.Ef.Horse> horses,
                                        IReadPortalChild<ILightHorse> lightHorsePortal,
                                        IReadPortalChild<IHeavyHorse> heavyHorsePortal)
        {
            foreach (var horse in horses)
            {
                if(IHorse.IsLightHorse((Breed) horse.Breed))
                {
                    var h = await lightHorsePortal.FetchChild(horse);
                    Add(h);
                }
                else
                {
                    var h = await heavyHorsePortal.FetchChild(horse);
                    Add(h);
                }
            }
        }


        
#endif
    }
}
