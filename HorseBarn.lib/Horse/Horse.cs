using Neatoo;
using Neatoo.Portal;
using Neatoo.Rules;
using Neatoo.Rules.Rules;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorseBarn.lib.Horse
{
    // TODO : Abstract causes portal methods (ex [CreateChild]) to not be recognized by PortalOperationManager
    internal class Horse<H> : EditBase<H>, IHorse
        where H : Horse<H>
    {
        public Horse(IEditBaseServices<H> services) : base(services)
        {
            AddRules(RuleManager);
        }

        private static void AddRules(IRuleManager<H> ruleManager)
        {
            ruleManager.AddRule(hc =>
            {
                if (hc.BirthDate != null && hc.BirthDate.Value > DateOnly.FromDateTime(DateTime.Now))
                {
                    RuleResult.PropertyError(nameof(BirthDate), "Birth date cannot be a future date.");
                }

                return RuleResult.Empty();

            }, nameof(BirthDate));
        }

        public Guid? Id { get => Getter<Guid?>(); private set => Setter(value); }

        [Required]
        public string? Name { get => Getter<string>(); set => Setter(value); }

        public DateOnly? BirthDate { get => Getter<DateOnly?>(); set => Setter(value); }

        [Required]
        public Breed Breed { get => Getter<Breed>(); protected set => Setter(value); }

        public double? Weight { get => Getter<double>(); set => Setter(value); }

        private static IEnumerable<Breed> LightHorses = [Horse.Breed.QuarterHorse, Horse.Breed.Thoroughbred, Horse.Breed.Mustang];

        private static IEnumerable<Breed> HeavyHorses = [Horse.Breed.Clydesdale, Horse.Breed.Shire];

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
