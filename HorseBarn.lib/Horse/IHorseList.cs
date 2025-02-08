using Neatoo;
using System.Collections.Specialized;

namespace HorseBarn.lib.Horse
{

    public interface IHorseList : IEditListBase<IHorse>, INotifyCollectionChanged
    {

        internal void RemoveHorse(IHorse horse);
    }

}
