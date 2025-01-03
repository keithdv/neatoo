using HorseBarn.lib.Horse;
using Neatoo;
using Neatoo.Portal;
using Neatoo.Rules;
using Neatoo.Rules.Rules;
using System;
using System.Collections.Generic;
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

        private static void AddRules(IRuleManager<C> ruleManager)
        {
            // Static method so you don't accidentally reference the instance properties and get an error
            ruleManager.AddRule(c =>
            {
                if (c.NumberOfHorses != 0 && c.NumberOfHorses != c.HorseList.Count)
                {
                    return RuleResult.PropertyError(nameof(NumberOfHorses), $"There are {c.HorseList.Count} but there need to be {c.NumberOfHorses}");
                }
                return RuleResult.Empty();
            }, nameof(NumberOfHorses));
        }

        public Guid? Id { get => Getter<Guid?>(); private set => Setter(value); }

        [Required]
        public string Name { get => Getter<string>(); set => Setter(value); }

        public int NumberOfHorses { get => Getter<int>(); set => Setter(value); }

        public IHorseList<H> HorseList { get => Getter<IHorseList<H>>(); private set => Setter(value); }

        internal IEnumerable<IHorse> Horses => HorseList.Cast<IHorse>();

        IEnumerable<IHorse> ICart.Horses => throw new NotImplementedException();

        [CreateChild]
        public async void CreateChild(ISendReceivePortalChild<IHorseList<H>> horsePortal)
        {
            this.HorseList = await horsePortal.CreateChild();
            await this.CheckAllRules();
        }

        public void RemoveHorse(IHorse horse)
        {
            HorseList.RemoveHorse(horse);
        }
    }
}
