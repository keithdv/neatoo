
using HorseBarn.lib.Horse;
using Neatoo;
using System.Collections.Specialized;

namespace HorseBarn.lib
{
    public interface IPasture : IEditBase
    {
        internal IHorseList HorseList { get; }
        public IReadOnlyListBase<IHorse> Horses { get; }

        internal void RemoveHorse(IHorse horse);
    }
}