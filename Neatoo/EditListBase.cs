using Neatoo.Core;
using Neatoo.Portal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Neatoo
{

    public interface IEditListBase : IValidateListBase, IEditMetaProperties, IPortalEditTarget
    {
    }

    public interface IEditListBase<I> : IEditListBase, IValidateListBase<I>, IEditMetaProperties, IPortalEditTarget
        where I : IEditBase
    {
        new void RemoveAt(int index);
    }

    public abstract class EditListBase<I> : ValidateListBase<I>, INeatooObject, IEditListBase<I>, IEditListBase
        where I : IEditBase
    {

        protected new IReadWritePortalChild<I> ItemPortal { get; }

        public EditListBase(EditListBaseServices<I> services) : base(services)
        {
            this.ItemPortal = services.ReadWritePortalChild;
        }

        public bool IsMarkedModified { get; protected set; } = false;
        public bool IsModified => IsNew || this.Any(c => c.IsModified) || IsDeleted || DeletedList.Any() || IsSelfModified;
        public bool IsSelfModified => IsDeleted || IsMarkedModified;
        public bool IsSavable => false;
        public bool IsNew { get; protected set; }
        public bool IsDeleted { get; protected set; }
        public bool IsChild { get; protected set; }
        protected List<I> DeletedList { get; } = new List<I>();

        protected (bool IsModified, bool IsSelfModified, bool IsSavable) EditMetaState { get; private set; }

        protected override void RaiseMetaPropertiesChanged(bool resetBusy = false)
        {
            base.RaiseMetaPropertiesChanged(resetBusy);

            if (EditMetaState.IsModified != IsModified)
            {
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsModified)));
            }
            if (EditMetaState.IsSelfModified != IsSelfModified)
            {
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsSelfModified)));
            }
            if (EditMetaState.IsSavable != IsSavable)
            {
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsSavable)));
            }

            ResetMetaState();
        }

        protected override void ResetMetaState()
        {
            base.ResetMetaState();
            EditMetaState = (IsModified, IsSelfModified, IsSavable);
        }

        protected virtual void MarkAsChild()
        {
            IsChild = true;
        }

        void IPortalEditTarget.MarkAsChild()
        {
            MarkAsChild();
        }

        protected virtual void MarkUnmodified()
        {
            IsNew = false;
        }

        void IPortalEditTarget.MarkUnmodified()
        {
            MarkUnmodified();
        }

        protected virtual void MarkModified()
        {
            IsMarkedModified = true;
        }

        void IPortalEditTarget.MarkModified()
        {
            MarkModified();
        }

        protected virtual void MarkNew()
        {
            IsNew = true;
        }

        void IPortalEditTarget.MarkNew()
        {
            MarkNew();
        }

        protected virtual void MarkOld()
        {
            IsNew = false;
        }

        void IPortalEditTarget.MarkOld()
        {
            MarkOld();
        }

        protected virtual void MarkDeleted()
        {
            // TODO
            // THis concept is a little blurry
            // I suppose I should delete all of my children??
            IsDeleted = true;
        }

        void IPortalEditTarget.MarkDeleted()
        {
            MarkDeleted();
        }

        public void Delete()
        {
            MarkDeleted();
        }

        public void UnDelete()
        {
            // TODO : Blurry here too...should I delete all of my children?
            IsDeleted = false;
        }

        protected override void InsertItem(int index, I item)
        {
            if (item.IsDeleted)
            {
                item.UnDelete();
            }

            if (!IsStopped && !item.IsNew)
            {
                ((IPortalEditTarget)item).MarkModified();
            }

            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            var item = this[index];
            if (!item.IsNew)
            {
                item.Delete();
                DeletedList.Add(item);
            }

            base.RemoveItem(index);
        }

        [Update]
        [UpdateChild]
        protected virtual async Task Update()
        {
            if (IsSelfModified)
            {
                throw new Exception($"{this.GetType().FullName} is modified you must override and define Update().");
            }
            await UpdateList();
        }

        [Update]
        [UpdateChild]
        protected virtual async Task Update(object[] criteria)
        {
            if (IsSelfModified)
            {
                throw new Exception($"{this.GetType().FullName} is modified you must override and define Update().");
            }
            await UpdateList(criteria);
        }

        protected async Task UpdateList()
        {
            foreach (var d in DeletedList)
            {
                if (d.IsDeleted) // May have been moved to a different parent
                {
                    await ItemPortal.UpdateChild(d);
                }
            }

            DeletedList.Clear();

            foreach (var i in this.Where(i => i.IsModified).ToList())
            {
                await ItemPortal.UpdateChild(i);
            }
        }

        protected async Task UpdateList(object[] criteria)
        {
            foreach (var d in DeletedList)
            {
                await ItemPortal.UpdateChild(d, criteria);
            }

            DeletedList.Clear();

            foreach (var i in this.Where(i => i.IsModified).ToList())
            {
                await ItemPortal.UpdateChild(i, criteria);
            }
        }
    }



}
