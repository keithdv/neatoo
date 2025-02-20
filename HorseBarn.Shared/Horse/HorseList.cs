#if !CLIENT
using HorseBarn.Dal.Ef;
#endif

using HorseBarn.lib.Cart;
using Neatoo;
using Neatoo.Portal;
using System.Collections.Specialized;

namespace HorseBarn.lib.Horse;

public interface IHorseList : IEditListBase<IHorse>, INotifyCollectionChanged
{

    internal void RemoveHorse(IHorse horse);
}

[Factory]
internal class HorseList : EditListBase<HorseList, IHorse>, IHorseList
{
    public HorseList() : base()
    {
    }

    public void RemoveHorse(IHorse horse)
    {
        if (Contains(horse))
        {
            Remove(horse);
        }
    }

    [Create]
    public void Create()
    {

    }

#if !CLIENT

    [Fetch]
    public async Task Fetch(ICollection<Dal.Ef.Horse> horses,
                                    [Service] LightHorseFactory lightHorsePortal,
                                    [Service] HeavyHorseFactory heavyHorsePortal)
    {
        foreach (var horse in horses)
        {
            if (IHorse.IsLightHorse((Breed)horse.Breed))
            {
                var h = await lightHorsePortal.Fetch(horse);
                Add(h);
            }
            else
            {
                var h = await heavyHorsePortal.Fetch(horse);
                Add(h);
            }
        }
    }

    [Update]
    public async Task Update(Dal.Ef.Cart cart,
                                    [Service] LightHorseFactory lightHorsePortal,
                                    [Service] HeavyHorseFactory heavyHorsePortal)
    {
        foreach (var horse in this.Union(DeletedList))
        {
            if (horse is ILightHorse h)
            {
                await lightHorsePortal.Save(h, cart);
            }
            else if (horse is IHeavyHorse hh)
            {
                await heavyHorsePortal.Save(hh, cart);
            }
        }

        DeletedList.Clear();
    }

    [Update]
    public async Task Update(Dal.Ef.Pasture pasture,
                                    [Service] LightHorseFactory lightHorsePortal,
                                    [Service] HeavyHorseFactory heavyHorsePortal)
    {
        async Task SaveHorse(IHorse horse)
        {
            if (horse is ILightHorse h)
            {
                await lightHorsePortal.Save(h, pasture);
            }
            else if (horse is IHeavyHorse hh)
            {
                await heavyHorsePortal.Save(hh, pasture);
            }
        }
        foreach (var horse in DeletedList) // TODO: Room for improvement here
        {
            if (horse.IsDeleted)
            {
                await SaveHorse(horse);
            }
        }

        DeletedList.Clear();

        foreach (var horse in this)
        {
            await SaveHorse(horse);
        }
    }

#endif
}
