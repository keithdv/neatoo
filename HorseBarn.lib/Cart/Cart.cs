using HorseBarn.lib.Horse;
using Neatoo;
using Neatoo.Portal;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace HorseBarn.lib.Cart
{
    internal class Cart<C, H> : CustomEditBase<C>, ICart
        where C : Cart<C, H>
        where H : IHorse
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

        public IHorseList HorseList { get => Getter<IHorseList>(); private set => Setter(value); }

        internal IEnumerable<IHorse> Horses => HorseList.Cast<IHorse>();

        IEnumerable<IHorse> ICart.Horses => HorseList.Cast<IHorse>();


        public async Task RemoveHorse(IHorse horse)
        {
            HorseList.RemoveHorse(horse);
            //await CheckRules(nameof(HorseList));
            //await WaitForTasks();
            await Task.CompletedTask;
        }

        public async Task AddHorse(IHorse horse)
        {
            if (horse is H h)
            {
                HorseList.Add(h);
            }
            else
            {
                throw new ArgumentException($"Horse {horse.GetType().FullName} is not of type {typeof(H).FullName}");
            }
            //await CheckRules(nameof(HorseList));
            //await WaitForTasks();
            await Task.CompletedTask;
        }

        public bool CanAddHorse(IHorse horse)
        {
            if (horse is H && HorseList.Count < NumberOfHorses)
            {
                return true;
            }
            return false;
        }

        protected virtual CartType CartType => throw new NotImplementedException();

#if !CLIENT


        [CreateChild]
        public async void CreateChild(IReadWritePortalChild<IHorseList> horsePortal)
        {
            this.HorseList = await horsePortal.CreateChild();
            this.NumberOfHorses = 1;
            await this.RunSelfRules();
        }

        [FetchChild]
        public async Task FetchChild(Dal.Ef.Cart cart, IReadPortalChild<IHorseList> horsePortal)
        {
            this.Id = cart.Id;
            this.Name = cart.Name;
            this.NumberOfHorses = cart.NumberOfHorses;
            this.HorseList = await horsePortal.FetchChild(cart.Horses);
        }

        [InsertChild]
        protected async Task Insert(Dal.Ef.HorseBarn horseBarn, IReadWritePortalChild<IHorseList> horseList)
        {
            Dal.Ef.Cart cart = new Dal.Ef.Cart();

            cart.Name = this.Name;
            cart.CartType = (int)this.CartType;
            cart.NumberOfHorses = this.NumberOfHorses;
            cart.HorseBarn = horseBarn;
            cart.PropertyChanged += HandleIdPropertyChanged;

            horseBarn.Carts.Add(cart);

            await horseList.UpdateChild(this.HorseList, cart);
        }

        [UpdateChild]
        protected async Task Update(Dal.Ef.HorseBarn horseBarn, IReadWritePortalChild<IHorseList> horseList)
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
