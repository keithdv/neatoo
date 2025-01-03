using Neatoo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorseBarn.lib.Horse
{
    public interface IHorseList : IHorseList<IHorse>
    {
    }

    public interface IHorseList<I> : IEditListBase<I>
        where I : IHorse
    {
        internal void RemoveHorse(IHorse horse);
    }
}
