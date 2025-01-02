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
    internal class HeavyHorse : Horse<HeavyHorse>, IHeavyHorse
    {
        public HeavyHorse(IEditBaseServices<HeavyHorse> services, CreateRequiredRule createRequiredRule) : base(services, createRequiredRule)
        {
        }

        [CreateChild]
        private void createChild(Breed breed)
        {
            if (!Horse<HeavyHorse>.IsHeavyHorse(breed))
            {
                throw new Exception($"Incorred Breed: {breed.ToString()}");
            }

            this.Breed = breed;
        }
    }
}
