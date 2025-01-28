using Neatoo;
using Neatoo.Rules;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public HorseCriteria(ValidateBaseServices<HorseCriteria> services) : base(services)
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
