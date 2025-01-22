using Neatoo.Portal;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Neatoo.Netwonsoft.Json.Test.EditTests
{
    public interface IEditObject : IEditBase
    {
        Guid ID { get; set; }
        string Name { get; set; }

        IEditObject Child { get; set; }
        void MarkAsChild();

        void MarkNew();

        void MarkOld();

        void MarkUnmodified();

        void MarkDeleted();
    }

    public class EditObject : EditBase<EditObject>, IEditObject
    {
        public EditObject(IEditBaseServices<EditObject> services) : base(services)
        {
        }

        public Guid ID { get => Getter<Guid>(); set => Setter(value); }
        public string Name { get => Getter<string>(); set => Setter(value); }
        public IEditObject Child { get => Getter<IEditObject>(); set => Setter(value); }

        void IEditObject.MarkAsChild()
        {
            this.MarkAsChild();
        }

        void IEditObject.MarkDeleted()
        {
            this.MarkDeleted();
        }

        void IEditObject.MarkNew()
        {
            this.MarkNew();
        }

        void IEditObject.MarkOld()
        {
            this.MarkOld();
        }

        void IEditObject.MarkUnmodified()
        {
            this.MarkUnmodified();
        }

        [Create]
        public void Create(Guid ID, string Name)
        {
            this.ID = ID;
            this.Name = Name;
        }

        [Update]
        [Insert]
        public void Update()
        {
            this.Name = "Updated";
        }
    }

    public interface IEditObjectList : IEditListBase<IEditObject>
    {
        Guid ID { get; set; }
        string Name { get; set; }

        internal void MarkAsChild();

        internal void MarkNew();

        internal void MarkOld();

        internal void MarkUnmodified();

        internal void MarkDeleted();
    }

    public class EditObjectList : EditListBase<EditObjectList, IEditObject>, IEditObjectList
    {
        public EditObjectList(IEditListBaseServices<EditObjectList, IEditObject> services) : base(services)
        {

        }

        public Guid ID { get => Getter<Guid>(); set => Setter(value); }
        public string Name { get => Getter<string>(); set => Setter(value); }

        void IEditObjectList.MarkAsChild()
        {
            this.MarkAsChild();
        }

        void IEditObjectList.MarkDeleted()
        {
            this.MarkDeleted();
        }

        void IEditObjectList.MarkNew()
        {
            this.MarkNew();
        }

        void IEditObjectList.MarkOld()
        {
            this.MarkOld();
        }

        void IEditObjectList.MarkUnmodified()
        {
            this.MarkUnmodified();
        }
    }
}
