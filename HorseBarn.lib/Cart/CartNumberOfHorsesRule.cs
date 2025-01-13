using HorseBarn.lib.Horse;
using Neatoo.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorseBarn.lib.Cart
{
    internal interface ICartNumberOfHorsesRule : IRule<ICart>
    {
    }

    internal class CartNumberOfHorsesRule : RuleBase<ICart>, ICartNumberOfHorsesRule
    {
        public CartNumberOfHorsesRule()
        {
            TriggerProperties.AddRange([nameof(ICart.NumberOfHorses), nameof(ICart<IHorse>.HorseList)]);
        }
        public override IRuleResult Execute(ICart cart)
        {
            var horseCount = cart.Horses.Count();
            if (cart.Horses.Count() > cart.NumberOfHorses)
            {
                cart.NumberOfHorses = horseCount;
            }
            else if (cart.NumberOfHorses == 0)
            {
                cart.NumberOfHorses = 1;
            }
            else if (horseCount != 0 && cart.NumberOfHorses != horseCount)
            {
                return RuleResult.PropertyError(nameof(ICart.NumberOfHorses), $"There are {horseCount} but there need to be {cart.NumberOfHorses}");
            }
            return RuleResult.Empty();
        }
    }
}
