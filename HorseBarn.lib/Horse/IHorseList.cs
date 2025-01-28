using Neatoo;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorseBarn.lib.Horse
{

    public interface IHorseList : IEditListBase<IHorse>, INotifyCollectionChanged
    {

        internal void RemoveHorse(IHorse horse);
    }

}
