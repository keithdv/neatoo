using Neatoo;
using Neatoo.Portal;
using Neatoo.Rules.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorseBarn.lib.Horse
{

    internal class LightHorse : Horse<LightHorse>, ILightHorse
    {
        public LightHorse(IEditBaseServices<LightHorse> services) : base(services)
        {
        }

        public double TopSpeed { get => Getter<double>(); set => Setter(value); }

        [CreateChild]
        public void createChild(Breed breed)
        {
            if (!Horse<HeavyHorse>.IsLightHorse(breed))
            {
                throw new Exception($"Incorrect Breed: {breed.ToString()}");
            }

            Breed = breed;
        }
    }
}
