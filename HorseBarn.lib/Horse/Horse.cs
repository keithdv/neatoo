using Neatoo;
using Neatoo.Portal;
using Neatoo.Rules;
using Neatoo.Rules.Rules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorseBarn.lib.Horse
{
    // TODO : Abstract causes portal methods (ex [CreateChild]) to not be recognized by PortalOperationManager and I don't know why
    internal class Horse<H> : CustomEditBase<H>, IHorse
        where H : Horse<H>
    {
        public Horse(IEditBaseServices<H> services) : base(services)
        {
            AddRules(RuleManager);
        }

        private static void AddRules(IRuleManager<H> ruleManager)
        {
            ruleManager.AddRule(hc =>
            {
                if (hc.BirthDate != null && hc.BirthDate.Value > DateOnly.FromDateTime(DateTime.Now))
                {
                    RuleResult.PropertyError(nameof(BirthDate), "Birth date cannot be a future date.");
                }

                return RuleResult.Empty();

            }, nameof(BirthDate));
        }


        [Required]
        public string? Name { get => Getter<string>(); set => Setter(value); }

        public DateOnly? BirthDate { get => Getter<DateOnly?>(); set => Setter(value); }

        [Required]
        public Breed Breed { get => Getter<Breed>(); protected set => Setter(value); }

        public double? Weight { get => Getter<double>(); set => Setter(value); }

        protected void Create(IHorseCriteria horseCriteria)
        {
            this.Breed = horseCriteria.Breed;
            this.BirthDate = horseCriteria.BirthDay;
            this.Name = horseCriteria.Name;
        }

#if !CLIENT

        [FetchChild]
        protected void Fetch(Dal.Ef.Horse horse)
        {
            this.Id = horse.Id;
            this.BirthDate = horse.BirthDate;
            this.Breed = (Breed)horse.Breed;
            this.Name = horse.Name;
        }

        Dal.Ef.Horse horse;

        [InsertChild]
        protected void Insert(Dal.Ef.Pasture pasture)
        {
            horse = new Dal.Ef.Horse();
            horse.BirthDate = this.BirthDate;
            horse.Breed = (int)this.Breed;
            horse.Name = this.Name;

            pasture.Horses.Add(horse);

            horse.PropertyChanged += HandleIdPropertyChanged;
        }

        [InsertChild]
        protected void Insert(Dal.Ef.Cart cart)
        {
            var horse = new Dal.Ef.Horse();
            horse.BirthDate = this.BirthDate;
            horse.Breed = (int)this.Breed;
            horse.Name = this.Name;

            cart.Horses.Add(horse);

            horse.PropertyChanged += HandleIdPropertyChanged;
        }

        [UpdateChild]
        protected void Update(Dal.Ef.Horse horse)
        {
            Debug.Assert(horse.Id == this.Id, "Unexpected Id");
            horse.BirthDate = this.BirthDate;
            horse.Breed = (int)this.Breed;
            horse.Name = this.Name;
        }
#endif
    }
}
