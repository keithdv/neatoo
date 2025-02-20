using HorseBarn.lib.Horse;
using Neatoo;
using Neatoo.Portal;
using System.Diagnostics;

#if !CLIENT
using HorseBarn.Dal.Ef;
using Microsoft.EntityFrameworkCore;
#endif

namespace HorseBarn.lib;

public interface IPasture : IEditBase
{
    internal int? Id { get; }
    internal IHorseList HorseList { get; }
    public IReadOnlyListBase<IHorse> Horses { get; }

    internal void RemoveHorse(IHorse horse);
}

[Factory]
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

    [Create]
    public async Task Create([Service] HorseListFactory horseListPortal)
    {
        HorseList = await horseListPortal.Create(); 
        await RunAllRules();
    }

    [Fetch]
    public async Task Fetch(Dal.Ef.Pasture pasture,[Service] HorseListFactory horseListPortal)
    {
        this.Id = pasture.Id;

        this.HorseList = await horseListPortal.Fetch(pasture.Horses);
    }

    [Insert]
    public async Task Insert(Dal.Ef.HorseBarn horseBarn,[Service] HorseListFactory horseListPortal)
    {
        var pasture = new Dal.Ef.Pasture();
        pasture.PropertyChanged += HandleIdPropertyChanged;
        horseBarn.Pasture = pasture;
        await horseListPortal.Save(HorseList, pasture);
    }

    [Update]
    public async Task Update(Dal.Ef.HorseBarn horseBarn, [Service] HorseListFactory horseListPortal)
    {
        var pasture = horseBarn.Pasture;
        Debug.Assert(pasture.Id == this.Id, "Unexpected Id");
        await horseListPortal.Save(HorseList, pasture);
    }

#endif

}
