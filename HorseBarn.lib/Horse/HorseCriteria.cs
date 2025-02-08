using Neatoo;
using System.ComponentModel.DataAnnotations;

namespace HorseBarn.lib.Horse
{
    public interface IHorseCriteria : IValidateBase
    {
        DateOnly? BirthDay { get; set; }
        Breed Breed { get; set; }
        string Name { get; set; }
    }

    internal class HorseCriteria : ValidateBase<HorseCriteria>, IHorseCriteria
    {
        public HorseCriteria(IValidateBaseServices<HorseCriteria> services) : base(services)
        {
        }

        [Required]
        public string Name { get => Getter<string>(); set => Setter(value); }

        [Required]
        public Breed Breed { get => Getter<Breed>(); set => Setter(value); }

        [Required]
        public DateOnly? BirthDay { get => Getter<DateOnly?>(); set => Setter(value); }

    }
}
