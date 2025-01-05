﻿using Neatoo.Portal;
using Neatoo.UnitTest.PersonObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.EditBaseTests
{

    public interface IEditPerson : IPersonBase, IEditBase
    {
        IEditPerson Child { get; set; }

        List<int> InitiallyNull { get; set; }
        List<int> InitiallyDefined { get; set; }
        void MarkAsChild();

        void MarkNew();

        void MarkOld();

        void MarkUnmodified();

        void MarkDeleted();

    }

    public class EditPerson : PersonEditBase<EditPerson>, IEditPerson
    {
        public EditPerson(IEditBaseServices<EditPerson> services,
            IShortNameRule<EditPerson> shortNameRule,
            IFullNameRule<EditPerson> fullNameRule) : base(services)
        {
            RuleManager.AddRules(shortNameRule, fullNameRule);
            InitiallyDefined = new List<int>() { 1, 2, 3 };
        }

        public List<int> InitiallyNull { get => Getter<List<int>>(); set => Setter(value); }
        public List<int> InitiallyDefined { get => Getter<List<int>>(); set => Setter(value); }

        public IEditPerson Child { get => Getter<IEditPerson>(); set => Setter(value); }

        void IEditPerson.MarkAsChild()
        {
            this.MarkAsChild();
        }
        
        void IEditPerson.MarkDeleted()
        {
            this.MarkDeleted();
        }

        void IEditPerson.MarkNew()
        {
            this.MarkNew();
        }

        void IEditPerson.MarkOld()
        {
            this.MarkOld();
        }

        void IEditPerson.MarkUnmodified()
        {
            this.MarkUnmodified();
        }
    }
}
