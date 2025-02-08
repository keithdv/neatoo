using Neatoo;
using Neatoo.Portal;

namespace HorseBarn.lib.Horse
{
    internal class HeavyHorse : Horse<HeavyHorse>, IHeavyHorse
    {
        public HeavyHorse(IEditBaseServices<HeavyHorse> services) : base(services)
        {
        }

        [Create]
        [CreateChild]
        public void createChild(IHorseCriteria horseCriteria)
        {

            if (!IHorse.IsHeavyHorse(horseCriteria.Breed))
            {
                throw new Exception($"Incorrect Breed: {horseCriteria.Breed.ToString()}");
            }

            Create(horseCriteria);
        }
    }
}
