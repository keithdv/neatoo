﻿using System;
using System.Collections.Generic;
using System.Text;
using Neatoo;
using Neatoo.Core;

namespace Neatoo.UnitTest.PersonObjects

{
    public abstract class PersonEditBase<T> : EditBase<T>, IPersonBase
        where T : IPersonEdit
    {

        public PersonEditBase(EditBaseServices<T> services) : base(services)
        {
        }

        private IRegisteredProperty IdProperty => GetRegisteredProperty(nameof(Id));
        public Guid Id { get { return Getter<Guid>(); } }

        public string FirstName { get { return Getter<string>(); } set { Setter(value); } }

        public string LastName { get { return Getter<string>(); } set { Setter(value); } }

        public string ShortName { get { return Getter<string>(); } set { Setter(value); } }

        public string Title { get { return Getter<string>(); } set { Setter(value); } }

        public string FullName { get { return Getter<string>(); } set { Setter(value); } }

        public uint? Age { get => Getter<uint?>(); set => Setter(value); }
        string IPersonBase.FirstName { get => FirstName; set => FirstName = value; }

        public void FillFromDto(PersonDto dto)
        {
             this[IdProperty].LoadProperty(dto.PersonId);

            // These will not mark IsModified to true
            // as long as within ObjectPortal operation
            FirstName = dto.FirstName;
            LastName = dto.LastName;
            Title = dto.Title;
        }
    }
}
