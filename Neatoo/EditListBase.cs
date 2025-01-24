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

    public interface IEditListBase : IValidateListBase, IEditBase, IEditMetaProperties, IPortalEditTarget
    {
    }

    public interface IEditListBase<I> : IValidateListBase<I>, IEditBase, IEditMetaProperties, IPortalEditTarget
        where I : IEditBase
    {
        new void RemoveAt(int index);

    }

    public abstract class EditListBase<T, I> : ValidateListBase<T, I>, INeatooObject, IEditListBase<I>, IEditListBase
        where T : EditListBase<T, I>
        where I : IEditBase
    {

        protected new IEditPropertyValueManager<T> PropertyValueManager => (IEditPropertyValueManager<T>)base.PropertyValueManager;

        protected new IReadWritePortalChild<I> ItemPortal { get; }
        public IReadWritePortal<T> ReadWritePortal { get; }

        public EditListBase(IEditListBaseServices<T, I> services) : base(services)
        {
            this.ItemPortal = services.ReadWritePortalChild;
            this.ReadWritePortal = services.ReadWritePortal;
        }

        protected bool SetModified { get; set; } = false;
        public bool IsModified => PropertyValueManager.IsModified || IsNew || this.Any(c => c.IsModified) || IsDeleted || DeletedList.Any() || IsSelfModified;
        public bool IsSelfModified => PropertyValueManager.IsSelfModified || IsDeleted || SetModified;
        public bool IsSavable => IsModified && IsValid && !IsBusy && !IsChild;
        public bool IsNew { get; protected set; }
        public bool IsDeleted { get; protected set; }
        public IEnumerable<string> ModifiedProperties => PropertyValueManager.ModifiedProperties;
        public bool IsChild { get; protected set; }
        protected List<I> DeletedList { get; } = new List<I>();

        bool IEditBase.SetModified => SetModified;

        protected (bool IsModified, bool IsSelfModified, bool IsSavable) EditMetaState { get; private set; }

        protected override void RaiseMetaPropertiesChanged()
        {
            base.RaiseMetaPropertiesChanged();

            if (EditMetaState.IsModified != IsModified)
            {
                PropertyHasChanged(nameof(IsModified));
            }
            if (EditMetaState.IsSelfModified != IsSelfModified)
            {
                PropertyHasChanged(nameof(IsSelfModified));
            }
            if (EditMetaState.IsSavable != IsSavable)
            {
                PropertyHasChanged(nameof(IsSavable));
            }

            ResetMetaState();
        }

        protected override void ResetMetaState()
        {
            base.ResetMetaState();
            EditMetaState = (IsModified, IsSelfModified, IsSavable);
        }

        new protected IEditPropertyValue GetProperty(string propertyName)
        {
            return PropertyValueManager[propertyName];
        }

        new protected IEditPropertyValue GetProperty(IRegisteredProperty registeredProperty)
        {
            return PropertyValueManager[registeredProperty];
        }

        new protected IEditPropertyValue this[string propertyName] { get => GetProperty(propertyName); }
        new protected IEditPropertyValue this[IRegisteredProperty registeredProperty] { get => GetProperty(registeredProperty); }

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
            PropertyValueManager.MarkSelfUnmodified();
        }

        void IPortalEditTarget.MarkUnmodified()
        {
            MarkUnmodified();
        }

        protected virtual void MarkModified()
        {
            SetModified = true;
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

        async Task<I> IEditBase.SaveRetrieve<I>()
        {
            return await Task.FromResult((await DoSave()) as I);
        }

        public Task Save()
        {
            return DoSave();
        }

        public virtual async Task<T> DoSave()
        {
            if (!IsSavable)
            {
                if (IsChild)
                {
                    throw new Exception("Child objects cannot be saved");
                }
                if (!IsValid)
                {
                    throw new Exception("Object is not valid and cannot be saved.");
                }
                if (!IsModified)
                {
                    throw new Exception("Object has not been modified.");
                }
            }

            return await ReadWritePortal.Update((T)this);
        }

        [Update]
        [UpdateChild]
        protected virtual async Task Update()
        {
            if (IsSelfModified)
            {
                throw new Exception($"{typeof(T).FullName} is modified you must override and define Update().");
            }
            await UpdateList();
        }

        [Update]
        [UpdateChild]
        protected virtual async Task Update(object[] criteria)
        {
            if (IsSelfModified)
            {
                throw new Exception($"{typeof(T).FullName} is modified you must override and define Update().");
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
