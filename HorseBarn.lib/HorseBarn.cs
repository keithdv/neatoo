using HorseBarn.lib.Cart;
using HorseBarn.lib.Horse;
using Neatoo;
using Neatoo.Portal;

#if !CLIENT
using Microsoft.EntityFrameworkCore;
using HorseBarn.Dal.Ef;
#endif

namespace HorseBarn.lib;

internal partial class HorseBarn : CustomEditBase<HorseBarn>, IHorseBarn
{
    private readonly LightHorseFactory lightHorsePortal;
    private readonly HeavyHorseFactory heavyHorsePortal;
    private readonly RacingChariotFactory racingChariotPortal;
    private readonly WagonFactory wagonPortal;

    public HorseBarn(IEditBaseServices<HorseBarn> services,
                        LightHorseFactory lightHorsePortal,
                        HeavyHorseFactory heavyHorsePortal,
                        RacingChariotFactory racingChariotPortal,
                        WagonFactory wagonPortal) : base(services)
    {
        this.lightHorsePortal = lightHorsePortal;
        this.heavyHorsePortal = heavyHorsePortal;
        this.racingChariotPortal = racingChariotPortal;
        this.wagonPortal = wagonPortal;
    }

    public IPasture Pasture { get => Getter<IPasture>()!; private set => Setter(value); }
    public ICartList Carts { get => Getter<ICartList>()!; private set => Setter(value); }
    public IEnumerable<IHorse> Horses => Carts.SelectMany(c => c.Horses).Union(Pasture.HorseList);

    IReadOnlyListBase<ICart> IHorseBarn.Carts => this.Carts;

    public async Task<IRacingChariot> AddRacingChariot()
    {
        var newCart = await racingChariotPortal.Create();
        this.Carts.Add(newCart);
        return newCart;
    }

    public async Task<IWagon> AddWagon()
    {
        var newCart = await wagonPortal.Create();
        this.Carts.Add(newCart);
        return newCart;
    }

    public async Task<IHorse> AddNewHorse(IHorseCriteria horseCriteria)
    {
        IHorse horse;

        if (IHorse.IsLightHorse(horseCriteria.Breed))
        {
            horse = await lightHorsePortal.Create(horseCriteria);
        }
        else if (IHorse.IsHeavyHorse(horseCriteria.Breed))
        {
            horse = await heavyHorsePortal.Create(horseCriteria);
        }
        else
        {
            throw new Exception($"Cannot create child horse for breed {horseCriteria.Breed}");
        }

        this.Pasture.HorseList.Add(horse);
        return horse;
    }

    public async Task MoveHorseToCart(IHorse horse, ICart cart)
    {
        Pasture.RemoveHorse(horse);
        await Carts.RemoveHorse(horse);

        await cart.AddHorse(horse);
    }

    public async Task MoveHorseToPasture(IHorse horse)
    {
        await Carts.RemoveHorse(horse);

        if (!Pasture.HorseList.Contains(horse))
        {
            Pasture.HorseList.Add(horse);
        }
    }


#if !CLIENT

    [Create]
    public async Task Create([Service] PastureFactory pasturePortal,[Service] CartListFactory cartListPortal)
    {
        this.Pasture = await pasturePortal.Create();
        this.Carts = await cartListPortal.Create();
    }

    [Fetch]
    public async Task Fetch([Service] IHorseBarnContext horseBarnContext,
                            [Service] PastureFactory pasturePortal,
                            [Service] CartListFactory cartPortal)
    {

        var horseBarn = await horseBarnContext.HorseBarns.FirstOrDefaultAsync();
        if (horseBarn != null)
        {
            this.Id = horseBarn.Id;
            this.Pasture = await pasturePortal.Fetch(horseBarn.Pasture);
            this.Carts = await cartPortal.Fetch(horseBarn.Carts);
        }
    }

    [Insert]
    public async Task Insert([Service] IHorseBarnContext horseBarnContext,
                            [Service] PastureFactory pasturePortal,
                            [Service] CartListFactory cartPortal)
    {
        var horseBarn = new Dal.Ef.HorseBarn();

        horseBarn.PropertyChanged += HandleIdPropertyChanged;

        await pasturePortal.Save(this.Pasture, horseBarn);
        await cartPortal.Save(this.Carts, horseBarn);

        horseBarnContext.HorseBarns.Add(horseBarn);

        await horseBarnContext.SaveChangesAsync();
    }

    [Update]
    public async Task Update([Service] IHorseBarnContext horseBarnContext,
                            [Service] PastureFactory pasturePortal,
                            [Service] CartListFactory cartPortal)
    {
        var horseBarn = await horseBarnContext.HorseBarns.FirstAsync(hb => hb.Id == this.Id);
        if (this.Pasture.IsModified)
        {
            await pasturePortal.Save(this.Pasture, horseBarn);
        }

        if (this.Carts.IsModified)
        {
            await cartPortal.Save(this.Carts, horseBarn);
        }

        await horseBarnContext.SaveChangesAsync();
    }

#endif

#if CLIENT

    [Fetch]
    public void Fetch()
    {
    }

    [Create]
    public void Create(){
    }

    [Update]
    public void Update()
    {
    }
#endif

}
