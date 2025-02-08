
using HorseBarn.lib.Horse;
using Neatoo;

namespace HorseBarn.lib
{
    public interface IPasture : IEditBase
    {
        internal int? Id { get; }
        internal IHorseList HorseList { get; }
        public IReadOnlyListBase<IHorse> Horses { get; }

        internal void RemoveHorse(IHorse horse);
    }
}