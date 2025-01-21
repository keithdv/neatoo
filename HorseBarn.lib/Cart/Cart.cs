using HorseBarn.lib.Horse;
using Neatoo;
using Neatoo.Portal;
using Neatoo.Rules;
using Neatoo.Rules.Rules;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HorseBarn.lib.Cart
{
    internal class Cart<C, H> : CustomEditBase<C>, ICart<H>, ICart
        where C : Cart<C, H>
        where H : class, IHorse
    {
        public Cart(IEditBaseServices<C> services,
                    ICartNumberOfHorsesRule cartNumberOfHorsesRule) : base(services)
        {
            RuleManager.AddRule(cartNumberOfHorsesRule);
        }

        event NotifyCollectionChangedEventHandler? INotifyCollectionChanged.CollectionChanged
        {
            add
            {
                HorseList.CollectionChanged += value;
            }

            remove
            {
                HorseList.CollectionChanged -= value;
            }
        }

        [Required]
        public string Name { get => Getter<string>(); set => Setter(value); }

        [Required]
        public int NumberOfHorses { get => Getter<int>(); set => Setter(value); }

        public IHorseList<H> HorseList { get => Getter<IHorseList<H>>(); private set => Setter(value); }

        internal IEnumerable<IHorse> Horses => HorseList.Cast<IHorse>();

        IEnumerable<IHorse> ICart.Horses => HorseList;

        protected override Task PostPortalConstruct()
        {
            HorseList.CollectionChanged += (s, e) => CheckRules(nameof(HorseList));
            return base.PostPortalConstruct();
        }

        public void RemoveHorse(IHorse horse)
        {
            HorseList.RemoveHorse(horse);
        }

        public void AddHorse(IHorse horse)
        {
            if(horse is H h)
            {
                HorseList.Add(h);
            } else
            {
                throw new ArgumentException($"Horse {horse.GetType().FullName} is not of type {typeof(H).FullName}");
            }
        }

        public bool CanAddHorse(IHorse horse)
        {
            if(horse is H && HorseList.Count < NumberOfHorses)
            {
                return true;
            }
            return false;
        }

        protected virtual CartType CartType => throw new NotImplementedException();

#if !CLIENT


        [CreateChild]
        public async void CreateChild(IReadWritePortalChild<IHorseList<H>> horsePortal)
        {
            this.HorseList = await horsePortal.CreateChild();
            this.NumberOfHorses = 1;
            await this.CheckAllSelfRules();
        }

        [FetchChild]
        public async Task FetchChild(Dal.Ef.Cart cart, IReadPortalChild<IHorseList<H>> horsePortal)
        {
            this.Id = cart.Id;
            this.Name = cart.Name;
            this.NumberOfHorses = cart.NumberOfHorses;
            this.HorseList = await horsePortal.FetchChild(cart.Horses);
        }

        [InsertChild]
        protected async Task Insert(Dal.Ef.HorseBarn horseBarn, IReadWritePortalChild<IHorseList<H>> horseList)
        {
            Dal.Ef.Cart cart = new Dal.Ef.Cart();

            cart.Name = this.Name;
            cart.CartType = (int) this.CartType;
            cart.NumberOfHorses = this.NumberOfHorses;
            cart.HorseBarn = horseBarn;
            cart.PropertyChanged += HandleIdPropertyChanged;

            horseBarn.Carts.Add(cart);

            await horseList.UpdateChild(this.HorseList, cart);
        }

        [UpdateChild]
        protected async Task Update(Dal.Ef.HorseBarn horseBarn, IReadWritePortalChild<IHorseList<H>> horseList)
        {
            var cart = horseBarn.Carts.First(c => c.Id == this.Id);

            Debug.Assert(cart.CartType == cart.CartType, "CartType mismatch");

            cart.Name = this.Name;
            cart.NumberOfHorses = this.NumberOfHorses;
            await horseList.UpdateChild(this.HorseList, cart);
        }
#endif
    }
}
