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
    internal class LightHorse : Horse<LightHorse>, ILightHorse
    {
        public LightHorse(IEditBaseServices<LightHorse> services, CreateRequiredRule createRequiredRule) : base(services, createRequiredRule)
        {
        }

        [CreateChild]
        private void createChild(Breed breed)
        {
            if (!Horse<HeavyHorse>.IsLightHorse(breed))
            {
                throw new Exception($"Incorred Breed: {breed.ToString()}");
            }

            this.Breed = breed;
        }
    }
}
