using Neatoo.Core;
using Neatoo.Portal;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Neatoo;

public interface IBase : INeatooObject, INotifyPropertyChanged, INotifyNeatooPropertyChanged, IBaseMetaProperties
{
    IBase Parent { get; }

    internal IProperty GetProperty(string propertyName);
    internal IProperty this[string propertyName] { get => GetProperty(propertyName); }
    internal IPropertyManager<IProperty> PropertyManager { get; }
}

public abstract class Base<T> : INeatooObject, IBase, ISetParent, IJsonOnDeserialized
    where T : Base<T>
{
    // Fields
    protected AsyncTaskSequencer AsyncTaskSequencer { get; private set; } = new AsyncTaskSequencer();

    // Properties
    protected IPropertyManager<IProperty> PropertyManager { get; set; }
    IPropertyManager<IProperty> IBase.PropertyManager => PropertyManager;
    public IBase Parent { get; protected set; }
    protected IProperty this[string propertyName] { get => GetProperty(propertyName); }
    public bool IsSelfBusy => !AsyncTaskSequencer.AllDone.IsCompleted || PropertyManager.IsSelfBusy;
    public bool IsBusy => IsSelfBusy || PropertyManager.IsBusy;

    // Constructors
    public Base(IBaseServices<T> services)
    {
        PropertyManager = services.PropertyManager ?? throw new ArgumentNullException("PropertyManager");

        if (PropertyManager is IPropertyManager<IProperty>)
        {
            PropertyManager.NeatooPropertyChanged += _NeatooPropertyChanged;
        }

        AsyncTaskSequencer.OnFullSequenceComplete = async () =>
        {
            CheckIfMetaPropertiesChanged(true);
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

    protected virtual void CheckIfMetaPropertiesChanged(bool raiseBusy = true)
    {

    }

    protected virtual void RaisePropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected virtual void HandlePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        CheckIfMetaPropertiesChanged(true);
    }

    protected virtual Task RaiseNeatooPropertyChanged(PropertyChangedBreadCrumbs breadCrumbs)
    {
        Debug.Assert(PropertyManager.GetProperties.Any(p => p.Name == breadCrumbs.PropertyName), "Property not found");

        // This is for databinding only - not other Neatoo objects
        return NeatooPropertyChanged?.Invoke(new PropertyChangedBreadCrumbs(breadCrumbs.PropertyName, this, breadCrumbs.PreviousPropertyName)) ?? Task.CompletedTask;
    }

    protected virtual Task HandleNeatooPropertyChanged(PropertyChangedBreadCrumbs breadCrumbs)
    {
        if (breadCrumbs.Source is IProperty property)
        {
            if (this[property.Name].Value is ISetParent child)
            {
                child.SetParent(this);
            }
            if (this[property.Name] is IBase childBase)
            {
                childBase.PropertyChanged += HandlePropertyChanged;
            }

            // This isn't meant to go to parent Neatoo objects, just outside listeners
            RaisePropertyChanged(breadCrumbs.FullPropertyName);
        }

        return RaiseNeatooPropertyChanged(breadCrumbs);
    }

    private Task _NeatooPropertyChanged(PropertyChangedBreadCrumbs breadCrumbs)
    {
        return HandleNeatooPropertyChanged(breadCrumbs);
    }

    protected virtual P? Getter<P>([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
    {
        return (P?) PropertyManager[propertyName]?.Value;
    }

    protected virtual void Setter<P>(P? value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
    {
        AsyncTaskSequencer.AddTask((t) => PropertyManager[propertyName].SetValue(value));
    }

    public IProperty GetProperty(string propertyName)
    {
        return PropertyManager[propertyName];
    }

    public virtual void OnDeserialized()
    {
        PropertyManager.NeatooPropertyChanged += _NeatooPropertyChanged;


        foreach (var property in PropertyManager.GetProperties)
        {
            if (property.Value is ISetParent setParent)
            {
                setParent.SetParent(this);
            }
            if (property.Value is IBase baseProperty)
            {
                baseProperty.PropertyChanged += HandlePropertyChanged;
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
    public event PropertyChangedEventHandler? PropertyChanged;
    public event NeatooPropertyChanged? NeatooPropertyChanged;
}