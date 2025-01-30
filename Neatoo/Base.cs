using Neatoo.Attributes;
using Neatoo.AuthorizationRules;
using Neatoo.Core;
using Neatoo.Portal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Neatoo
{

    public interface IBase : INeatooObject, INotifyPropertyChanged, INotifyNeatooPropertyChanged
    {
        IBase Parent { get; }

        internal IProperty GetProperty(string propertyName);
        internal IProperty GetProperty(IRegisteredProperty registeredProperty);

        internal IProperty this[string propertyName] { get => GetProperty(propertyName); }
        internal IProperty this[IRegisteredProperty registeredProperty] { get => GetProperty(registeredProperty); }
    }

    [PortalDataContract]
    public abstract class Base<T> : INeatooObject, IBase, IPortalTarget, ISetParent
        where T : Base<T>
    {
        
        [PortalDataMember]
        protected IPropertyManager PropertyManager { get; set; }

        public Base(IBaseServices<T> services)
        {
            PropertyManager = services.PropertyManager ?? throw new ArgumentNullException("PropertyManager");

            if (PropertyManager is IPropertyManager)
            {
                PropertyManager.NeatooPropertyChanged += _PropertyManagerNeatooPropertyChange;
                PropertyManager.PropertyChanged += _PropertyManagerPropertyChanged;
            }
        }

        [PortalDataMember]
        public IBase Parent { get; protected set; }

        protected virtual void SetParent(IBase parent)
        {
            Parent = parent;
        }
        void ISetParent.SetParent(IBase parent)
        {
            SetParent(parent);
        }
        protected AsyncTaskSequencer AsyncTaskSequencer { get; set; } = new AsyncTaskSequencer();

        public event PropertyChangedEventHandler PropertyChanged;
        public event NeatooPropertyChanged NeatooPropertyChanged;

        protected virtual void RaisePropertyChanged(string propertyName, object source = null)
        {
            PropertyChanged?.Invoke(source ?? this, new PropertyChangedEventArgs(propertyName));
        }

        protected Task RaiseNeatooPropertyChanged(string propertyName, object source)
        {
            return NeatooPropertyChanged?.Invoke(propertyName, source);
        }

        protected virtual Task PropertyManagerNeatooPropertyChange(string propertyName, object source)
        {
            return AsyncTaskSequencer.AddTask((t) => NeatooPropertyChanged?.Invoke(propertyName, source) ?? Task.CompletedTask);
        }

        private Task _PropertyManagerNeatooPropertyChange(string propertyName, object source)
        {
            return PropertyManagerNeatooPropertyChange(propertyName, source);
        }

        protected virtual void PropertyManagerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // TODO - if an object isn't assigned to another IBase
            // it will still consider us to be the Parent

            if (sender == this.PropertyManager)
            {
                if (this[e.PropertyName].Value is ISetParent child)
                {
                    child.SetParent(this);
                }

                if (this[e.PropertyName].Value is INotifyPropertyChanged notifyPropertyChanged)
                {
                    // TODO - This is never getting removed..which is not good
                    notifyPropertyChanged.PropertyChanged += Child_PropertyChanged;
                }

                // NOTE: Neatoo property changes only come thru the Property and PropertyManager - Don't connect directly to the object
            }

            if (sender == this.PropertyManager && PropertyManager.HasProperty(e.PropertyName))
            {
                RaisePropertyChanged(e.PropertyName, this);
            }
        }

        private void _PropertyManagerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyManagerPropertyChanged(sender, e);
        }

        protected virtual void ChildPropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        private void Child_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ChildPropertyChanged(sender, e);
        }

        [OnSerialized]
        protected void OnSerialized(StreamingContext context)
        {
            PropertyManager.NeatooPropertyChanged -= _PropertyManagerNeatooPropertyChange;
            PropertyManager.PropertyChanged -= _PropertyManagerPropertyChanged;
        }

        [OnDeserialized]
        protected void OnDeserialized(StreamingContext context)
        {
            PropertyManager.NeatooPropertyChanged += _PropertyManagerNeatooPropertyChange;
            PropertyManager.PropertyChanged += _PropertyManagerPropertyChanged;
        }



        protected virtual P Getter<P>([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            return (P)PropertyManager[propertyName].Value;
        }

        protected virtual void Setter<P>(P value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            PropertyManager[propertyName].SetValue(value);
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

        IDisposable IPortalTarget.StopAllActions()
        {
            return null;
        }

        void IPortalTarget.StartAllActions()
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

        protected IProperty this[string propertyName] { get => GetProperty(propertyName); }
        protected IProperty this[IRegisteredProperty registeredProperty] { get => GetProperty(registeredProperty); }
    }

}
