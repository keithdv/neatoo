
using HorseBarn.lib.Horse;
using Neatoo;

namespace HorseBarn.lib
{
    public interface IPasture : IEditBase
    {
        Guid? Id { get; }
        string Name { get; set; }
        internal IHorseList HorseList { get; }
        public IEnumerable<IHorse> Horses { get; }
        internal void RemoveHorse(IHorse horse);

    }
}