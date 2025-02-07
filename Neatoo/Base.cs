using Neatoo.Core;
using Neatoo.Portal;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Neatoo;

public interface IBase : INeatooObject, INotifyPropertyChanged, INotifyNeatooPropertyChanged, IBaseMetaProperties
{
    IBase Parent { get; }

    internal IProperty GetProperty(string propertyName);
    internal IProperty GetProperty(IRegisteredProperty registeredProperty);

    internal IProperty this[string propertyName] { get => GetProperty(propertyName); }
    internal IProperty this[IRegisteredProperty registeredProperty] { get => GetProperty(registeredProperty); }

    internal IPropertyManager<IProperty> PropertyManager { get; }
}

public abstract class Base<T> : INeatooObject, IBase, IPortalTarget, ISetParent, IJsonOnDeserialized
    where T : Base<T>
{
    // Fields
    protected AsyncTaskSequencer AsyncTaskSequencer { get; private set; } = new AsyncTaskSequencer();

    // Properties
    protected IPropertyManager<IProperty> PropertyManager { get; set; }
    IPropertyManager<IProperty> IBase.PropertyManager => PropertyManager;
    public IBase Parent { get; protected set; }
    protected IProperty this[string propertyName] { get => GetProperty(propertyName); }
    protected IProperty this[IRegisteredProperty registeredProperty] { get => GetProperty(registeredProperty); }
    public bool IsSelfBusy => !AsyncTaskSequencer.AllDone.IsCompleted || PropertyManager.IsSelfBusy;
    public bool IsBusy => IsSelfBusy || PropertyManager.IsBusy;

    // Constructors
    public Base(IBaseServices<T> services)
    {
        PropertyManager = services.PropertyManager ?? throw new ArgumentNullException("PropertyManager");

        if (PropertyManager is IPropertyManager<IProperty>)
        {
            PropertyManager.NeatooPropertyChanged += _PropertyManagerNeatooPropertyChange;
        }

        AsyncTaskSequencer.OnFullSequenceComplete = async () =>
        {
            RaiseMetaPropertiesChanged(true);
        };
    }

    // Methods
    protected virtual void SetParent(IBase parent)
    {
        Parent = parent;
    }

    void ISetParent.SetParent(IBase parent)
    {
        SetParent(parent);
    }

    protected virtual void RaiseMetaPropertiesChanged(bool raiseBusy = true)
    {

    }

    protected virtual void RaisePropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected virtual Task RaiseNeatooPropertyChanged(PropertyNameBreadCrumbs breadCrumbs)
    {
        Debug.Assert(PropertyManager.GetProperties.Any(p => p.Name == breadCrumbs.PropertyName), "Property not found");

        // This is for databinding only - not other Neatoo objects
        return NeatooPropertyChanged?.Invoke(new PropertyNameBreadCrumbs(breadCrumbs.PropertyName, this, breadCrumbs.PreviousPropertyName)) ?? Task.CompletedTask;
    }

    protected virtual Task PropertyManagerNeatooPropertyChanged(PropertyNameBreadCrumbs breadCrumbs)
    {
        if (breadCrumbs.Source is IProperty property)
        {
            if (this[property.Name].Value is ISetParent child)
            {
                child.SetParent(this);
            }
        }

        return RaiseNeatooPropertyChanged(breadCrumbs);
    }

    private Task _PropertyManagerNeatooPropertyChange(PropertyNameBreadCrumbs breadCrumbs)
    {
        return PropertyManagerNeatooPropertyChanged(breadCrumbs);
    }

    protected virtual P Getter<P>([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
    {
        return (P)PropertyManager[propertyName].Value;
    }

    protected virtual void Setter<P>(P value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
    {
        AsyncTaskSequencer.AddTask((t) => PropertyManager[propertyName].SetValue(value));
    }

    protected IRegisteredProperty GetRegisteredProperty(string propertyName)
    {
        return PropertyManager.RegisteredPropertyManager.GetRegisteredProperty(propertyName);
    }

    public IProperty GetProperty(string propertyName)
    {
        return PropertyManager[propertyName];
    }

    public IProperty GetProperty(IRegisteredProperty registeredProperty)
    {
        return PropertyManager[registeredProperty];
    }

    IDisposable IPortalTarget.PauseAllActions()
    {
        return null;
    }

    void IPortalTarget.ResumeAllActions()
    {
    }

    Task IPortalTarget.PostPortalConstruct()
    {
        return this.PostPortalConstruct();
    }

    protected virtual Task PostPortalConstruct()
    {
        return Task.CompletedTask;
    }

    public virtual void OnDeserialized()
    {
        PropertyManager.NeatooPropertyChanged += _PropertyManagerNeatooPropertyChange;

        foreach (var property in PropertyManager.GetProperties)
        {
            if (property.Value is ISetParent setParent)
            {
                setParent.SetParent(this);
            }
        }
    }

    public virtual async Task WaitForTasks()
    {
        // I don't like this...
        while (IsBusy)
        {
            await Task.Yield();
            // Enumrator changed errors
            // Will need to use events or something to signify when busy
            //while(PropertyManager.IsBusy)
            //{
            //    await PropertyManager.WaitForTasks();
            //}
            //await AsyncTaskSequencer.AllDone;
        }

        Debug.Assert(!IsBusy, "Should not be busy after running all rules");

        // Raise Errors
        await Task.WhenAll(AsyncTaskSequencer.AllDone, PropertyManager.WaitForTasks());

    }

    // Events
    public event PropertyChangedEventHandler PropertyChanged;
    public event NeatooPropertyChanged NeatooPropertyChanged;
}