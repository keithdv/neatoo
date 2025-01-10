﻿using HorseBarn.lib.Horse;
using Neatoo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorseBarn.lib.Cart
{

    public interface ICartList : ICartList<ICart>
    {

    }

    public interface ICartList<C> : IEditListBase<C>
        where C : ICart

    {

        internal void RemoveHorse(IHorse horse);
    }

    internal class CartList : CartList<CartList, ICart>, ICartList
    {
        public CartList(IEditListBaseServices<CartList, ICart> services) : base(services)
        {
        }
    }

    internal class CartList<L, C> : EditListBase<L, C>, ICartList<C>
        where L : CartList<L, C>
        where C : ICart
    {
        public CartList(IEditListBaseServices<L, C> services) : base(services)
        {
        }

        public void RemoveHorse(IHorse horse)
        {
            foreach(var c in this)
            {
                c.RemoveHorse(horse);
            }
        }
    }

}
