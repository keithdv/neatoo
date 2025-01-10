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
        public void createChild(IHorseCriteria horseCriteria)
        {

            if (!IsHeavyHorse(horseCriteria.Breed))
            {
                throw new Exception($"Incorrect Breed: {horseCriteria.Breed.ToString()}");
            }

            Create(horseCriteria);
        }
    }
}
