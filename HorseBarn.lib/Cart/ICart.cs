using HorseBarn.lib.Horse;
using Neatoo;

namespace HorseBarn.lib.Cart
{

    public interface ICart : IEditBase
    {
        string Name { get; set;  }
        int NumberOfHorses { get; set;  }

        IEnumerable<IHorse> Horses { get; }
        internal void RemoveHorse(IHorse horse);
    }

    public interface ICart<H> : ICart
        where H : IHorse
    {
        internal IHorseList<H> HorseList { get; }

    }
}