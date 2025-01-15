﻿using Neatoo.Portal;
using Neatoo.UnitTest.PersonObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.EditBaseTests
{

    public interface IEditPersonList : IEditListBase<IEditPerson>, IPersonBase
    {
        int DeletedCount { get; }

        void MarkAsChild();

        void MarkNew();

        void MarkOld();

        void MarkUnmodified();

        void MarkDeleted();
    }

    public class EditPersonList : EditListBase<EditPersonList, IEditPerson>, IEditPersonList
    {
        public EditPersonList(IEditListBaseServices<EditPersonList, IEditPerson> services,
            IShortNameRule<EditPersonList> shortNameRule,
            IFullNameRule<EditPersonList> fullNameRule) : base(services)
        {
            RuleManager.AddRules(shortNameRule, fullNameRule);
        }

        public int DeletedCount => DeletedList.Count;
        private IRegisteredProperty<Guid> IdProperty => GetRegisteredProperty<Guid>(nameof(Id));
        public Guid Id { get { return Getter<Guid>(); } }

        public string FirstName { get { return Getter<string>(); } set { Setter(value); } }

        public string LastName { get { return Getter<string>(); } set { Setter(value); } }

        public string ShortName { get { return Getter<string>(); } set { Setter(value); } }

        public string Title { get { return Getter<string>(); } set { Setter(value); } }

        public string FullName { get { return Getter<string>(); } set { Setter(value); } }

        public uint? Age { get => Getter<uint?>(); set => Setter(value); }

        public void FillFromDto(PersonDto dto)
        {
            LoadProperty(IdProperty, dto.PersonId);

            // These will not mark IsModified to true
            // as long as within ObjectPortal operation
            FirstName = dto.FirstName;
            LastName = dto.LastName;
            Title = dto.Title;
        }

        [Fetch]
        private async Task FillFromDto(PersonDto dto, IReadOnlyList<PersonDto> personTable)
        {
            LoadProperty(IdProperty, dto.PersonId);

            // These will not mark IsModified to true
            // as long as within ObjectPortal operation
            FirstName = dto.FirstName;
            LastName = dto.LastName;
            Title = dto.Title;

            var children = personTable.Where(p => p.FatherId == Id);

            foreach (var child in children)
            {
                Add(await ItemPortal.FetchChild(child));
            }
        }

        void IEditPersonList.MarkAsChild()
        {
            this.MarkAsChild();
        }

        void IEditPersonList.MarkDeleted()
        {
            this.MarkDeleted();
        }

        void IEditPersonList.MarkNew()
        {
            this.MarkNew();
        }

        void IEditPersonList.MarkOld()
        {
            this.MarkOld();
        }

        void IEditPersonList.MarkUnmodified()
        {
            this.MarkUnmodified();
        }

    }
}
