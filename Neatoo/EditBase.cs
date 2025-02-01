using Neatoo.Core;
using Neatoo.Portal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Neatoo
{

    public abstract class EditBase<T> : ValidateBase<T>, INeatooObject, IEditBase, IPortalEditTarget, IEditMetaProperties, IJsonOnDeserializing, IJsonOnDeserialized
        where T : EditBase<T>
    {
        protected new IEditPropertyManager PropertyManager => (IEditPropertyManager)base.PropertyManager;

        public EditBase(IEditBaseServices<T> services) : base(services)
        {
            ReadWritePortal = services.ReadWritePortal;
        }

        public bool IsMarkedModified { get; protected set; } = false;
        public bool IsModified => PropertyManager.IsModified || IsDeleted || IsNew || IsSelfModified;
        public bool IsSelfModified { get => PropertyManager.IsSelfModified || IsDeleted || IsMarkedModified; protected set => IsMarkedModified = value; }
        public bool IsSavable => IsModified && IsValid && !IsBusy && !IsChild;
        public bool IsNew { get; protected set; }
        public bool IsDeleted { get; protected set; }
        public IEnumerable<string> ModifiedProperties => PropertyManager.ModifiedProperties;
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

        bool IEditMetaProperties.IsMarkedModified => IsMarkedModified;

        protected virtual void MarkAsChild()
        {
            if (!IsStopped)
            {
                IsChild = true;
            }
        }

        void IPortalEditTarget.MarkAsChild()
        {
            MarkAsChild();
        }

        // TODO - Recursive set clean for all children
        protected virtual void MarkUnmodified()
        {
            if (!IsStopped)
            {
                // TODO : What if busy??
                PropertyManager.MarkSelfUnmodified();
                IsMarkedModified = false;
                RaiseMetaPropertiesChanged(); // Really shouldn't be anything listening to this
            }
        }

        void IPortalEditTarget.MarkUnmodified()
        {
            MarkUnmodified();
        }

        protected virtual void MarkModified()
        {
            if (!IsStopped)
            {
                IsMarkedModified = true;
                RaiseMetaPropertiesChanged();
            }
        }

        void IPortalEditTarget.MarkModified()
        {
            MarkModified();
        }

        protected virtual void MarkNew()
        {
            if (!IsStopped)
            {
                IsNew = true;
            }
        }

        void IPortalEditTarget.MarkNew()
        {
            MarkNew();
        }

        protected virtual void MarkOld()
        {
            if (!IsStopped)
            {
                IsNew = false;
            }
        }
        void IPortalEditTarget.MarkOld()
        {
            MarkOld();
        }

        protected virtual void MarkDeleted()
        {
            if (!IsStopped)
            {
                IsDeleted = true;
                RaiseMetaPropertiesChanged();
            }
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
            if (!IsStopped)
            {

                if (IsDeleted)
                {
                    IsDeleted = false;
                    RaiseMetaPropertiesChanged();
                }
            }
        }

        protected override void PropertyManagerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.PropertyManagerPropertyChanged(sender, e);

            // TODO - if an object isn't assigned to another IBase
            // it will still consider us to be the Parent

            if (sender == this.PropertyManager && this[e.PropertyName].Value is IEditBase child)
            {
                child.UnDelete();
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

            return await ReadWritePortal.Update((T)(IEditBase)this);
        }

        new protected IEditProperty GetProperty(string propertyName)
        {
            return PropertyManager[propertyName];
        }

        new protected IEditProperty GetProperty(IRegisteredProperty registeredProperty)
        {
            return PropertyManager[registeredProperty];
        }

        public void OnDeserializing()
        {
            IsStopped = true;
        }

        override public void OnDeserialized()
        {
            base.OnDeserialized();
            IsStopped = false;
        }

            new protected IEditProperty this[string propertyName] { get => GetProperty(propertyName); }
        new protected IEditProperty this[IRegisteredProperty registeredProperty] { get => GetProperty(registeredProperty); }
    }
}
