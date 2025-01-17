﻿using HorseBarn.lib.Horse;
using Neatoo;
using System.Collections.Specialized;

namespace HorseBarn.lib.Cart
{

    public interface ICart : IEditBase, INotifyCollectionChanged
    {
        internal int? Id { get; }
        string Name { get; set;  }
        int NumberOfHorses { get; set;  }
        IEnumerable<IHorse> Horses { get; }
        bool CanAddHorse(IHorse horse);
        internal void RemoveHorse(IHorse horse);
        internal void AddHorse(IHorse horse);
    }

    public interface ICart<H> : ICart, INotifyCollectionChanged
        where H : IHorse
    {
        internal IHorseList<H> HorseList { get; }

    }
}