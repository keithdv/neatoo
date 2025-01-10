using HorseBarn.lib.Horse;
using Neatoo;
using Neatoo.Portal;
using Neatoo.Rules;
using Neatoo.Rules.Rules;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HorseBarn.lib.Cart
{
    internal class Cart<C, H> : EditBase<C>, ICart<H>, ICart
        where C : Cart<C, H>
        where H : class, IHorse
    {
        public Cart(IEditBaseServices<C> services) : base(services)
        {
            AddRules(this.RuleManager);
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

        private static void AddRules(IRuleManager<C> ruleManager)
        {
            // Static method so you don't accidentally reference the instance properties and get an error
            ruleManager.AddRule(c =>
            {
                if(c.HorseList.Count > c.NumberOfHorses)
                {
                    c.NumberOfHorses = c.HorseList.Count;
                } else if (c.NumberOfHorses == 0)
                {
                    c.NumberOfHorses = 1;
                }
                else if (c.HorseList.Count != 0 && c.NumberOfHorses != c.HorseList.Count)
                {
                    return RuleResult.PropertyError(nameof(NumberOfHorses), $"There are {c.HorseList.Count} but there need to be {c.NumberOfHorses}");
                }
                return RuleResult.Empty();
            }, nameof(NumberOfHorses), nameof(HorseList));
        }

        public Guid? Id { get => Getter<Guid?>(); private set => Setter(value); }

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

        [CreateChild]
        public async void CreateChild(ISendReceivePortalChild<IHorseList<H>> horsePortal)
        {
            this.HorseList = await horsePortal.CreateChild();
            this.NumberOfHorses = 1;
            await this.CheckAllSelfRules();
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
    }
}
