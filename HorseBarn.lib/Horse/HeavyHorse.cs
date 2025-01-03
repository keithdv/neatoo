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
    internal class HeavyHorse : Horse<HeavyHorse>, IHeavyHorse
    {
        public HeavyHorse(IEditBaseServices<HeavyHorse> services) : base(services)
        {
        }

        [CreateChild]
        private void createChild(Breed breed)
        {
            if (!IsHeavyHorse(breed))
            {
                throw new Exception($"Incorrect Breed: {breed.ToString()}");
            }

            Breed = breed;
        }
    }
}
