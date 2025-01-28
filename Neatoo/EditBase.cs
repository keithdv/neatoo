using Neatoo.Attributes;
using Neatoo.Core;
using Neatoo.Portal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Neatoo
{

    public abstract class EditBase<T> : ValidateBase<T>, INeatooObject, IEditBase, IPortalEditTarget, IEditMetaProperties
        where T : EditBase<T>
    {
        [PortalDataMember]
        protected new IEditPropertyManager PropertyManager => (IEditPropertyManager)  base.PropertyManager;

        public EditBase(IEditBaseServices<T> services) : base(services)
        {
            ReadWritePortal = services.ReadWritePortal;
        }

        [PortalDataMember]
        public bool IsMarkedModified { get; protected set; } = false;
        public bool IsModified => PropertyManager.IsModified || IsDeleted || IsNew || IsSelfModified;
        public bool IsSelfModified { get => PropertyManager.IsSelfModified || IsDeleted || IsMarkedModified; protected set => IsMarkedModified = value; }
        public bool IsSavable => IsModified && IsValid && !IsBusy && !IsChild;
        [PortalDataMember]
        public bool IsNew { get; protected set; }
        [PortalDataMember]
        public bool IsDeleted { get; protected set; }
        public IEnumerable<string> ModifiedProperties => PropertyManager.ModifiedProperties;
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
                    RaisePropertyChanged(nameof(IsModified));
                }
                if (EditMetaState.IsSelfModified != IsSelfModified)
                {
                    RaisePropertyChanged(nameof(IsSelfModified));
                }
                if (EditMetaState.IsSavable != IsSavable)
                {
                    RaisePropertyChanged(nameof(IsSavable));
                }
                if (EditMetaState.IsDeleted != IsDeleted)
                {
                    RaisePropertyChanged(nameof(IsDeleted));
                }
            }

            base.RaiseMetaPropertiesChanged(raiseBusy);
        }

        protected override void ResetMetaState()
        {
            base.ResetMetaState();
            EditMetaState = (IsModified, IsSelfModified, IsSavable, IsDeleted);
        }

        bool IEditBase.IsMarkedModified => IsMarkedModified;

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
            PropertyManager.MarkSelfUnmodified();
            IsMarkedModified = false;
            RaiseMetaPropertiesChanged(); // Really shouldn't be anything listening to this
        }

        void IPortalEditTarget.MarkUnmodified()
        {
            MarkUnmodified();
        }

        protected virtual void MarkModified()
        {
            IsMarkedModified = true;
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
        


        public virtual async Task<IEditBase> Save()
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

            return await ReadWritePortal.Update((T) (IEditBase) this);
        }

        new protected IEditProperty GetProperty(string propertyName)
        {
            return PropertyManager[propertyName];
        }

        new protected IEditProperty GetProperty(IRegisteredProperty registeredProperty)
        {
            return PropertyManager[registeredProperty];
        }

        new protected IEditProperty this[string propertyName] { get => GetProperty(propertyName); }
        new protected IEditProperty this[IRegisteredProperty registeredProperty] { get => GetProperty(registeredProperty); }
    }
}
