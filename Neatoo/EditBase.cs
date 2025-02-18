using Neatoo.Core;
using Neatoo.Internal;
using Neatoo.Portal.Internal;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Neatoo;


public abstract class EditBase<T> : ValidateBase<T>, INeatooObject, IEditBase, IDataMapperEditTarget, IEditMetaProperties, IJsonOnDeserializing, IJsonOnDeserialized
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
    protected INeatooPortal<T> ReadWritePortal { get; }

    protected (bool IsModified, bool IsSelfModified, bool IsSavable, bool IsDeleted) EditMetaState { get; private set; }

    protected override void CheckIfMetaPropertiesChanged(bool raiseBusy = true)
    {
        if (!IsPaused)
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

        base.CheckIfMetaPropertiesChanged(raiseBusy);
    }

    protected override void ResetMetaState()
    {
        base.ResetMetaState();
        EditMetaState = (IsModified, IsSelfModified, IsSavable, IsDeleted);
    }

    bool IEditMetaProperties.IsMarkedModified => IsMarkedModified;

    protected virtual void MarkAsChild()
    {
        if (!IsPaused)
        {
            IsChild = true;
        }
    }

    void IDataMapperEditTarget.MarkAsChild()
    {
        MarkAsChild();
    }

    // TODO - Recursive set clean for all children
    protected virtual void MarkUnmodified()
    {
        if (!IsPaused)
        {
            // TODO : What if busy??
            PropertyManager.MarkSelfUnmodified();
            IsMarkedModified = false;
            CheckIfMetaPropertiesChanged(); // Really shouldn't be anything listening to this
        }
    }

    void IDataMapperEditTarget.MarkUnmodified()
    {
        MarkUnmodified();
    }

    protected virtual void MarkModified()
    {
        if (!IsPaused)
        {
            IsMarkedModified = true;
            CheckIfMetaPropertiesChanged();
        }
    }

    void IDataMapperEditTarget.MarkModified()
    {
        MarkModified();
    }

    protected virtual void MarkNew()
    {
        if (!IsPaused)
        {
            IsNew = true;
        }
    }

    void IDataMapperEditTarget.MarkNew()
    {
        MarkNew();
    }

    protected virtual void MarkOld()
    {
        if (!IsPaused)
        {
            IsNew = false;
        }
    }
    void IDataMapperEditTarget.MarkOld()
    {
        MarkOld();
    }

    protected virtual void MarkDeleted()
    {
        if (!IsPaused)
        {
            IsDeleted = true;
            CheckIfMetaPropertiesChanged();
        }
    }

    void IDataMapperEditTarget.MarkDeleted()
    {
        MarkDeleted();
    }

    public void Delete()
    {
        MarkDeleted();
    }

    public void UnDelete()
    {
        if (!IsPaused)
        {

            if (IsDeleted)
            {
                IsDeleted = false;
                CheckIfMetaPropertiesChanged();
            }
        }
    }

    protected override Task ChildNeatooPropertyChanged(PropertyChangedBreadCrumbs breadCrumbs)
    {
        
        // TODO - if an object isn't assigned to another IBase
        // it will still consider us to be the Parent

        if (breadCrumbs.PropertyName == nameof(IEditProperty.Value) && breadCrumbs.Source is IEditBase child)
        {
            child.UnDelete();
        }

        return base.ChildNeatooPropertyChanged(breadCrumbs);
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
                // TODO await this.WaitForTasks(); ??
                throw new Exception("Object is busy and cannot be saved.");
            }
        }

        return await ReadWritePortal.Update((T)(IEditBase)this);
    }

    new protected IEditProperty GetProperty(string propertyName)
    {
        return PropertyManager[propertyName];
    }

    public override IDisposable PauseAllActions()
    {
        PropertyManager.PauseAllActions();
        return base.PauseAllActions();
    }

    public override void ResumeAllActions()
    {
        PropertyManager.ResumeAllActions();
        base.ResumeAllActions();
    }

    public void OnDeserializing()
    {
        IsPaused = true;
    }

    override public void OnDeserialized()
    {
        base.OnDeserialized();
        IsPaused = false;
    }

    new protected IEditProperty this[string propertyName] { get => GetProperty(propertyName); }

}
