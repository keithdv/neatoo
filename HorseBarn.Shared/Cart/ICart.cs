using HorseBarn.lib.Horse;
using Neatoo;
using System.Collections.Specialized;

namespace HorseBarn.lib.Cart;


public interface ICart : IEditBase, INotifyCollectionChanged
{
    internal int? Id { get; }
    string Name { get; set; }
    int NumberOfHorses { get; set; }
    IEnumerable<IHorse> Horses { get; }
    bool CanAddHorse(IHorse horse);
    internal Task RemoveHorse(IHorse horse);
    internal Task AddHorse(IHorse horse);
    internal IHorseList HorseList { get; }
}
