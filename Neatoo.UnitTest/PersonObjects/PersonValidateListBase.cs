//using System;
//using System.Collections.Generic;
//using System.Text;
//using Neatoo.Rules;

//namespace Neatoo.UnitTest.PersonObjects

//{
//    public abstract class PersonValidateListBase<T> : ValidateListBase<T>, IPersonBase
//        where T : IPersonBase
//    {

//        public PersonValidateListBase(IValidateListBaseServices<T> services) : base(services)
//        {
//        }

//        private IRegisteredProperty IdProperty => GetRegisteredProperty(nameof(Id));
//        public Guid Id { get { return Getter<Guid>(); } }

//        public string FirstName
//        {
//            get { return Getter<string>(); }
//            set { Setter(value); }
//        }

//        public string LastName
//        {
//            get { return Getter<string>(); }
//            set { Setter(value); }
//        }

//        public string ShortName
//        {
//            get { return Getter<string>(); }
//            set { Setter(value); }
//        }

//        public string Title
//        {
//            get { return Getter<string>(); }
//            set { Setter(value); }
//        }

//        public string FullName
//        {
//            get { return Getter<string>(); }
//            set { Setter(value); }
//        }

//        public uint? Age
//        {
//            get => Getter<uint?>(); set => Setter(value);
//        }

//        public void FillFromDto(PersonDto dto)
//        {
//            this[IdProperty].SetValue(dto.PersonId);
//            FirstName = dto.FirstName;
//            LastName = dto.LastName;
//            Title = dto.Title;
//        }
//    }
//}
