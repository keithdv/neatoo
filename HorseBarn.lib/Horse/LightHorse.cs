using Neatoo;
using Neatoo.Portal;

namespace HorseBarn.lib.Horse;


internal class LightHorse : Horse<LightHorse>, ILightHorse
{
    public LightHorse(IEditBaseServices<LightHorse> services) : base(services)
    {
    }

    public double TopSpeed { get => Getter<double>(); set => Setter(value); }

    [Create]
    public void Create(IHorseCriteria criteria)
    {
        if (!IHorse.IsLightHorse(criteria.Breed))
        {
            throw new Exception($"Incorrect Breed: {criteria.Breed.ToString()}");
        }

        base.Create(criteria);
    }
}
