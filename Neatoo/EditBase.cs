using Neatoo.Attributes;
using Neatoo.Core;
using Neatoo.Portal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Neatoo
{

    public abstract class EditBase<T> : ValidateBase<T>, INeatooObject, IEditBase, IPortalEditTarget, IEditMetaProperties
        where T : EditBase<T>
    {
        [PortalDataMember]
        protected new IEditPropertyValueManager<T> PropertyValueManager => (IEditPropertyValueManager<T>)base.PropertyValueManager;

        public EditBase(IEditBaseServices<T> services) : base(services)
        {
            ReadWritePortal = services.ReadWritePortal;
        }

        [PortalDataMember]
        protected bool SetModified { get; set; } = false;
        public bool IsModified => PropertyValueManager.IsModified || IsDeleted || IsNew || IsSelfModified;
        public bool IsSelfModified => PropertyValueManager.IsSelfModified || IsDeleted || SetModified;
        public bool IsSavable => IsModified && IsValid && !IsBusy && !IsChild;
        [PortalDataMember]
        public bool IsNew { get; protected set; }
        [PortalDataMember]
        public bool IsDeleted { get; protected set; }
        public IEnumerable<string> ModifiedProperties => PropertyValueManager.ModifiedProperties;
        [PortalDataMember]
        public bool IsChild { get; protected set; }
        protected IReadWritePortal<T> ReadWritePortal { get; }

        protected (bool IsModified, bool IsSelfModified, bool IsSavable, bool IsDeleted) EditMetaState { get; private set; }

        protected override void RaiseMetaPropertiesChanged(bool raiseBusy = true)
        {

            if (!IsStopped)
            {
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
                if (EditMetaState.IsDeleted != IsDeleted)
                {
                    PropertyHasChanged(nameof(IsDeleted));
                }
            }

            base.RaiseMetaPropertiesChanged(raiseBusy);
        }

        protected override void ResetMetaState()
        {
            base.ResetMetaState();
            EditMetaState = (IsModified, IsSelfModified, IsSavable, IsDeleted);
        }

        bool IEditBase.SetModified => SetModified;

        protected virtual void MarkAsChild()
        {
            IsChild = true;
        }

        void IPortalEditTarget.MarkAsChild()
        {
            MarkAsChild();
        }

        // TODO - Recursive set clean for all children
        protected virtual void MarkUnmodified()
        {
            // TODO : What if busy??
            PropertyValueManager.MarkSelfUnmodified();
            SetModified = false;
            RaiseMetaPropertiesChanged(); // Really shouldn't be anything listening to this
        }

        void IPortalEditTarget.MarkUnmodified()
        {
            MarkUnmodified();
        }

        protected virtual void MarkModified()
        {
            SetModified = true;
            RaiseMetaPropertiesChanged();
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
            IsDeleted = true;
            RaiseMetaPropertiesChanged();
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
            if (IsDeleted)
            {
                IsDeleted = false;
                RaiseMetaPropertiesChanged();
            }
        }
        
        async Task<I> IEditBase.SaveRetrieve<I>()
        {
            return await Task.FromResult((await DoSave()) as I);
        }

        public Task Save()
        {
            return DoSave();
        }

        protected virtual async Task<T> DoSave()
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
                if (!(IsModified || IsSelfModified))
                {
                    throw new Exception("Object has not been modified.");
                }
                if (IsBusy)
                {
                    // TODO await this.WaitForRules(); ??
                    throw new Exception("Object is busy and cannot be saved.");
                }
            }

            return await ReadWritePortal.Update((T)this);
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
    }
}
