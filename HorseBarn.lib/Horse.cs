using Neatoo;
using Neatoo.Portal;
using Neatoo.Rules.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorseBarn.lib
{
    public class Horse<H> : EditBase<H>, IHorse
        where H: Horse<H>
    {
        public Horse(IEditBaseServices<H> services, CreateRequiredRule createRequiredRule) : base(services)
        {
            RuleManager.AddRule(createRequiredRule(nameof(Name)));
        }

        public Guid? Id { get => Getter<Guid?>(); private set => Setter(value); }

        public string Name { get => Getter<string>(); set => Setter(value); }

        public Breed Breed { get => Getter<Breed>(); protected set => Setter(value); }

        private static IEnumerable<Breed> LightHorses = [Breed.QuarterHorse, Breed.Thoroughbred, Breed.Mustang];

        private static IEnumerable<Breed> HeavyHorses = [Breed.Clydesdale, Breed.Shire];

        internal static bool IsLightHorse(Breed breed)
        {
            return LightHorses.Contains(breed);
        }

        internal static bool IsHeavyHorse(Breed breed)
        {
            return HeavyHorses.Contains(breed);
        }

    }
}
