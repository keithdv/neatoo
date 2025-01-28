using Neatoo.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Neatoo.Core
{

    /// <summary>
    /// DO NOT REGISTER IN THE CONTAINER
    /// </summary>
    public interface IPropertyManager : INotifyPropertyChanged, INotifyNeatooPropertyChanged
    {
        IRegisteredProperty GetRegisteredProperty(string name);

        bool HasProperty(string propertyName);

        // This isn't possible without some nasty reflection or static backing fields
        // If the property is being loaded for the first time you need the type
        //void LoadProperty(IRegisteredProperty registeredProperty, object newValue);
        IProperty GetProperty(string propertyName);
        IProperty GetProperty(IRegisteredProperty registeredProperty);

        public IProperty this[string propertyName] { get => GetProperty(propertyName); }
        public IProperty this[IRegisteredProperty registeredProperty] { get => GetProperty(registeredProperty); }

        internal IRegisteredPropertyManager RegisteredPropertyManager { get; }
    }


    public delegate Task NeatooPropertyChanged(string propertyName, object source);

    public interface INotifyNeatooPropertyChanged
    {
        event NeatooPropertyChanged NeatooPropertyChanged;
    }


    public interface IProperty : INotifyPropertyChanged, INotifyNeatooPropertyChanged
    {
        string Name { get; }
        object Value { get; set; }

        void SetValue(object newValue);

        Task Task { get; }

        TaskAwaiter GetAwaiter() => Task.GetAwaiter();

    }

    public interface IProperty<T> : IProperty
    {
        new T Value { get; set; }

    }

    [PortalDataContract]
    public class Property<T> : IProperty<T>, IProperty, INotifyPropertyChanged
    {
        // TODO: Shouldn't be modified from the outside
        [PortalDataMember]
        public string Name { get; } // Setter for Deserialization of Edit

        [PortalDataMember]
        protected T _value = default;

        public virtual T Value
        {
            get => _value;
            // TODO - Don't allow if the property setting on the ValidateBase<> is private set
            set
            {
                SetValue(value);
            }
        }

        object IProperty.Value { get => Value; set => SetValue(value); }

        public Task Task { get; protected set; } = Task.CompletedTask;
        public virtual void SetValue(object newValue)
        {
            if (newValue == null && _value == null) { return; }

            Task = Task.CompletedTask;

            if (newValue == null)
            {
                if (_value is INotifyNeatooPropertyChanged neatooPropertyChanged)
                {
                    neatooPropertyChanged.NeatooPropertyChanged -= _OnValueNeatooPropertyChanged;
                }

                _value = default;
                OnPropertyChanged(nameof(Value));
                Task = OnValueNeatooPropertyChanged(nameof(Value), this);
            }
            else if (newValue is T value)
            {
                var isDiff = AreSame(_value, value);

                if (isDiff)
                {
                    if (value is INotifyNeatooPropertyChanged neatooPropertyChanged)
                    {
                        neatooPropertyChanged.NeatooPropertyChanged -= _OnValueNeatooPropertyChanged;
                    }
                }

                _value = value;

                if (isDiff)
                {
                    if (value is INotifyNeatooPropertyChanged neatooPropertyChanged)
                    {
                        neatooPropertyChanged.NeatooPropertyChanged += _OnValueNeatooPropertyChanged;
                    }

                    OnPropertyChanged(nameof(Value));
                    Task = OnValueNeatooPropertyChanged(nameof(Value), this);
                }
            }
            else
            {
                throw new PropertyTypeMismatchException($"Type {newValue.GetType()} is not type {typeof(T).FullName}");
            }

            if (Task.IsCompleted && Task.IsFaulted)
            {
                throw Task.Exception;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Task _OnValueNeatooPropertyChanged(string propertyName, object source)
        {
            return OnValueNeatooPropertyChanged(propertyName, source);
        }

        protected virtual Task OnValueNeatooPropertyChanged(string propertyName, object source)
        {
            return NeatooPropertyChanged?.Invoke(this.Name, source) ?? Task.CompletedTask;
        }


        public Property(string name)
        {
            this.Name = name;
        }

        protected virtual bool AreSame<P>(P oldValue, P newValue)
        {
            if (!typeof(P).IsValueType)
            {
                if (oldValue == null && newValue == null)
                {
                    return true;
                }
                return !(ReferenceEquals(oldValue, newValue));
            }
            else
            {
                return !oldValue.Equals(newValue);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event NeatooPropertyChanged NeatooPropertyChanged;

        [OnSerialized]
        private void OnSerialized(StreamingContext context)
        {
            if (Value is INotifyNeatooPropertyChanged neatooPropertyChanged)
            {
                neatooPropertyChanged.NeatooPropertyChanged -= OnValueNeatooPropertyChanged;
            }
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            if (Value is INotifyNeatooPropertyChanged neatooPropertyChanged)
            {
                neatooPropertyChanged.NeatooPropertyChanged += OnValueNeatooPropertyChanged;
            }
        }
    }

    public class PropertyManager : PropertyManagerBase<IProperty>, IPropertyManager
    {

        public PropertyManager(IRegisteredPropertyManager registeredPropertyManager, IFactory factory) : base(registeredPropertyManager, factory)
        {
        }

        IRegisteredPropertyManager IPropertyManager.RegisteredPropertyManager => RegisteredPropertyManager;


        protected override IProperty CreateProperty<PV>(IRegisteredProperty registeredProperty)
        {
            return Factory.CreateProperty<PV>(registeredProperty);
        }

        public IProperty this[string propertyName]
        {
            get => GetProperty(propertyName);
        }

        public IProperty this[IRegisteredProperty registeredProperty]
        {
            get => GetProperty(registeredProperty);
        }

        public virtual IProperty GetProperty(string propertyName)
        {
            return GetProperty(RegisteredPropertyManager.GetRegisteredProperty(propertyName));
        }

    }

    [PortalDataContract]
    public abstract class PropertyManagerBase<P>
        where P : IProperty
    {
        protected IFactory Factory { get; }

        protected readonly IRegisteredPropertyManager RegisteredPropertyManager;

        public bool HasProperty(string propertyName)
        {
            return RegisteredPropertyManager.HasProperty(propertyName);
        }

        protected IDictionary<string, P> propertyValueStore = new Dictionary<string, P>();

        [PortalDataMember]
        protected IDictionary<string, P> fieldData
        {
            get => propertyValueStore;
            set
            {
                propertyValueStore = value;
            }
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            foreach (var p in fieldData.Values)
            {
                p.PropertyChanged += _OnPropertyChanged;
                p.NeatooPropertyChanged += _OnNeatooPropertyChanged;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public event NeatooPropertyChanged NeatooPropertyChanged;

        private void _OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IProperty.Value))
            {
                // Switch it from the IProperty.Name to IPropertyManager.PropertyName
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(((IProperty)sender).Name));
            }
        }

        private Task _OnNeatooPropertyChanged(string propertyName, object source)
        {
            return NeatooPropertyChanged?.Invoke(propertyName, this); // Switch source on purpose
        }

        public PropertyManagerBase(IRegisteredPropertyManager registeredPropertyManager, IFactory factory)
        {
            this.RegisteredPropertyManager = registeredPropertyManager;
            Factory = factory;
        }

        protected abstract P CreateProperty<PV>(IRegisteredProperty registeredProperty);

        public IRegisteredProperty GetRegisteredProperty(string name)
        {
            return RegisteredPropertyManager.GetRegisteredProperty(name);
        }

        public virtual P GetProperty(IRegisteredProperty registeredProperty)
        {
            if (fieldData.TryGetValue(registeredProperty.Name, out var fd))
            {
                return fd;
            }

            var newProperty = (P)this.GetType().GetMethod(nameof(this.CreateProperty), System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).MakeGenericMethod(registeredProperty.Type).Invoke(this, new object[] { registeredProperty });

            newProperty.PropertyChanged += _OnPropertyChanged;
            newProperty.NeatooPropertyChanged += _OnNeatooPropertyChanged;

            fieldData[registeredProperty.Name] = newProperty;

            return newProperty;
        }
    }


    [Serializable]
    public class PropertyTypeMismatchException : Exception
    {

        public PropertyTypeMismatchException() { }
        public PropertyTypeMismatchException(string message) : base(message) { }
        public PropertyTypeMismatchException(string message, Exception inner) : base(message, inner) { }
        protected PropertyTypeMismatchException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }


    [Serializable]
    public class PropertyNotFoundException : Exception
    {
        public PropertyNotFoundException() { }
        public PropertyNotFoundException(string message) : base(message) { }
        public PropertyNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected PropertyNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}

