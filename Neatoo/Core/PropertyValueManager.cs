using Neatoo.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Neatoo.Core
{

    /// <summary>
    /// DO NOT REGISTER IN THE CONTAINER
    /// </summary>
    public interface IPropertyValueManager : INotifyPropertyChanged
    {
        IRegisteredProperty GetRegisteredProperty(string name);

        bool HasProperty(string propertyName);

        // This isn't possible without some nasty reflection or static backing fields
        // If the property is being loaded for the first time you need the type
        //void LoadProperty(IRegisteredProperty registeredProperty, object newValue);
        IPropertyValue GetProperty(string propertyName);
        IPropertyValue GetProperty(IRegisteredProperty registeredProperty);

        public IPropertyValue this[string propertyName] { get => GetProperty(propertyName); }
        public IPropertyValue this[IRegisteredProperty registeredProperty] { get => GetProperty(registeredProperty); }

        /// <summary>
        /// Set the parent for all of the properties
        /// Needed for serialization and deserialization
        /// </summary>
        internal void SetParent(IBase Parent); 
    }


    /// <summary>
    /// This is what is registered from the container so that it is Type specific
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPropertyValueManager<T> : IPropertyValueManager
    {
        internal IRegisteredPropertyManager<T> RegisteredPropertyManager { get; }
    }

    public interface IPropertyValue : INotifyPropertyChanged
    {
        string Name { get; }
        object Value { get; set; }

        void SetValue(object newValue);

        IBase Parent { get; protected internal set; }

        Task Task { get; }

        TaskAwaiter GetAwaiter() => Task.GetAwaiter();
    }

    public interface IPropertyValue<T> : IPropertyValue
    {
        new T Value { get; set; }

    }

    [PortalDataContract]
    public class PropertyValue<T> : IPropertyValue<T>, IPropertyValue, INotifyPropertyChanged
    {
        // TODO: Shouldn't be modified from the outside
        [PortalDataMember]
        public string Name { get; } // Setter for Deserialization of Edit

        [PortalDataMember]
        public IBase Parent { get; set; }

        [PortalDataMember]
        protected T _value = default;

        public virtual T Value {
            get => _value;
            // TODO - Don't allow if the property setting on the ValidateBase<> is private set
            set
            {
                SetValue(value);
            }
        }

        object IPropertyValue.Value { get => Value; set => SetValue(value); }

        public Task Task { get; protected set; }

        public virtual void SetValue(object newValue)
        {
            if(newValue == null && _value == null) { return; }

            Task = Task.CompletedTask;

            if(newValue == null)
            {
                _value = default;
                OnPropertyChanged(nameof(Value));
                Task = HandlePropertyChanged(Name, Parent);
            }
            else if (newValue is T value)
            {
                SetParentOfValue(newValue);
                var isDiff = AreSame(_value, value);

                _value = value;
                if (isDiff)
                {
                    OnPropertyChanged(nameof(Value));
                    Task = HandlePropertyChanged(Name, Parent);
                }
            }
            else
            {
                throw new PropertyTypeMismatchException($"Type {newValue.GetType()} is not type {typeof(T).FullName}");
            }

            if(Task.IsCompleted && Task.IsFaulted)
            {
                throw Task.Exception;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual Task HandlePropertyChanged(string propertyName, IBase source)
        {
            return Parent?.HandlePropertyChanged(Name, Parent);
        }

        public PropertyValue(string name)
        {
            this.Name = name;
        }

        protected void SetParentOfValue(object newValue)
        {
            if (newValue is ISetParent x)
            {
                if (Parent == null) { throw new ArgumentNullException(nameof(Parent)); }
                x.SetParent(Parent);
            }
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
    }

    public class PropertyValueManager<T> : PropertyValueManagerBase<T, IPropertyValue>, IPropertyValueManager<T>
        where T : IBase
    {
        public PropertyValueManager(IRegisteredPropertyManager<T> registeredPropertyManager, IFactory factory) : base(registeredPropertyManager, factory)
        {

        }

        IRegisteredPropertyManager<T> IPropertyValueManager<T>.RegisteredPropertyManager => RegisteredPropertyManager;

        protected override IPropertyValue CreatePropertyValue<PV>(IRegisteredProperty registeredProperty, IBase parent)
        {
            return Factory.CreatePropertyValue<PV>(registeredProperty, parent);
        }

        public IPropertyValue this[string propertyName]
        {
            get => GetProperty(propertyName);
        }

        public IPropertyValue this[IRegisteredProperty registeredProperty]
        {
            get => GetProperty(registeredProperty);
        }

        public virtual IPropertyValue GetProperty(string propertyName)
        {
            return GetProperty(RegisteredPropertyManager.GetRegisteredProperty(propertyName));
        }

        public virtual IPropertyValue GetProperty(IRegisteredProperty registeredProperty)
        {
            if (fieldData.TryGetValue(registeredProperty.Name, out var fd))
            {
                return fd;
            }

            var newPropertyValue = (IPropertyValue) this.GetType().GetMethod(nameof(this.CreatePropertyValue), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).MakeGenericMethod(registeredProperty.Type).Invoke(this, new object[] { registeredProperty, Target });

            fieldData[registeredProperty.Name] = newPropertyValue;

            return newPropertyValue;
        }


    }

    [PortalDataContract]
    public abstract class PropertyValueManagerBase<T, P> : ISetTarget
        where T : IBase
        where P : IPropertyValue
    {
        protected T Target { get; set; }

        protected IFactory Factory { get; }

        protected readonly IRegisteredPropertyManager<T> RegisteredPropertyManager;

        public bool HasProperty(string propertyName)
        {
            return RegisteredPropertyManager.HasProperty(propertyName);
        }

        [PortalDataMember]
        protected IDictionary<string, P> fieldData = new ConcurrentDictionary<string, P>();

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public PropertyValueManagerBase(IRegisteredPropertyManager<T> registeredPropertyManager, IFactory factory)
        {
            this.RegisteredPropertyManager = registeredPropertyManager;
            Factory = factory;
        }

        protected abstract P CreatePropertyValue<PV>(IRegisteredProperty registeredProperty, IBase parent);

        public IRegisteredProperty GetRegisteredProperty(string name)
        {
            return RegisteredPropertyManager.GetRegisteredProperty(name);
        }

        void ISetTarget.SetTarget(IBase target)
        {
            this.Target = (T)(target ?? throw new ArgumentNullException(nameof(target)));
        }

        public void SetParent(IBase Parent)
        {
            foreach (var item in fieldData.Values)
            {
                item.Parent = Parent;
            }
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

