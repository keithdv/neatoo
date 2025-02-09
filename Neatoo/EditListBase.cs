using Neatoo.Portal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Neatoo
{

    public interface IEditListBase : IValidateListBase, IEditMetaProperties
    {
    }

    public interface IEditListBase<I> : IEditListBase, IValidateListBase<I>, IEditMetaProperties
        where I : IEditBase
    {
        new void RemoveAt(int index);
    }

    public abstract class EditListBase<T, I> : ValidateListBase<T, I>, INeatooObject, IEditListBase<I>, IEditListBase
        where T : EditListBase<T, I>
        where I : IEditBase
    {

        protected new IReadWritePortalChild<I> ItemPortal { get; }

        public EditListBase(IEditListBaseServices<T, I> services) : base(services)
        {
            this.ItemPortal = services.ReadWritePortalChild;
        }

        public bool IsModified => this.Any(c => c.IsModified) || DeletedList.Any();
        public bool IsSelfModified => false;
        public bool IsMarkedModified => false;
        public bool IsSavable => false;
        public bool IsNew => false;
        public bool IsDeleted => false;
        public bool IsChild => false;
        protected List<I> DeletedList { get; } = new List<I>();

        protected (bool IsModified, bool IsSelfModified, bool IsSavable) EditMetaState { get; private set; }

        protected override void CheckIfMetaPropertiesChanged(bool resetBusy = false)
        {
            base.CheckIfMetaPropertiesChanged(resetBusy);

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

        protected override void InsertItem(int index, I item)
        {
            if (!IsPaused)
            {
                if (item.IsDeleted)
                {
                    item.UnDelete();
                }

                if (!item.IsNew)
                {
                    ((IPortalEditTarget)item).MarkModified();
                }
            }

            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            if (!IsPaused)
            {
                var item = this[index];

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

        public override void OnDeserializing()
        {
            base.OnDeserializing();
            IsPaused = true;
        }

        public override void OnDeserialized()
        {
            base.OnDeserialized();
            IsPaused = false;
        }
    }



}
