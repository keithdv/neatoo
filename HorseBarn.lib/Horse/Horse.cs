﻿using Neatoo;
using Neatoo.Portal;
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
        }

        public Guid? Id { get => Getter<Guid?>(); private set => Setter(value); }

        [Required]
        public string Name { get => Getter<string>(); set => Setter(value); }

        [Required]
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
