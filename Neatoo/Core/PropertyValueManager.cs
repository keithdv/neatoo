using Neatoo.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
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
        void LoadProperty<P>(IRegisteredProperty registeredProperty, P newValue);
        void LoadProperty<P>(IRegisteredProperty registeredProperty, PropertyValue<P> newValue);
        P ReadProperty<P>(IRegisteredProperty registeredProperty);
        P ReadProperty<P>(string propertyName);

        // This isn't possible without some nasty reflection or static backing fields
        // If the property is being loaded for the first time you need the type
        //void LoadProperty(IRegisteredProperty registeredProperty, object newValue);
        IPropertyValue ReadProperty(string propertyName);
        IPropertyValue ReadProperty(IRegisteredProperty registeredProperty);

    }


    /// <summary>
    /// This is what is registered from the container so that it is Type specific
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPropertyValueManager<T> : IPropertyValueManager
    {
        internal IRegisteredPropertyManager<T> RegisteredPropertyManager { get; }
    }

    public interface IPropertyValue
    {
        string Name { get; internal set; }
        object Value { get; }
    }

    public interface IPropertyValue<T> : IPropertyValue
    {
        new T Value { get; set; }
    }

    [PortalDataContract]
    public class PropertyValue<T> : IPropertyValue<T>, IPropertyValue
    {
        // TODO: Shouldn't be modified from the outside
        [PortalDataMember]
        public string Name { get; set; } // Setter for Deserialization of Edit

        [PortalDataMember]
        public virtual T Value { get; set; }

        object IPropertyValue.Value => Value;

        protected PropertyValue() { } // For EditPropertyValue Deserialization

        public PropertyValue(string name, T value)
        {
            this.Name = name;
            this.Value = value;
        }



        // NOTE: These also broke serialization
        //public static implicit operator T(PropertyValue<T> value) => value.Value;
        //public static implicit operator PropertyValue<T>(T value) => new PropertyValue<T>(value);
    }

    public class PropertyValueManager<T> : PropertyValueManagerBase<T, IPropertyValue>, IPropertyValueManager<T>
        where T : IBase
    {
        public PropertyValueManager(IRegisteredPropertyManager<T> registeredPropertyManager, IFactory factory) : base(registeredPropertyManager, factory)
        {

        }

        IRegisteredPropertyManager<T> IPropertyValueManager<T>.RegisteredPropertyManager => RegisteredPropertyManager;

        protected override IPropertyValue CreatePropertyValue<PV>(IRegisteredProperty registeredProperty, PV value)
        {
            return Factory.CreatePropertyValue(registeredProperty, value);
        }

        public virtual void LoadProperty<PV>(IRegisteredProperty registeredProperty, PropertyValue<PV> newValue)
        {
            if (!fieldData.ContainsKey(registeredProperty.Index))
            {
                // TODO Destroy and Delink to old value
                // TODO - If they've created an event link to the PropertyValue.NotifyPropertyChanged event
                // Does just create a new one create a memory leak?
            }
            newValue.Name = registeredProperty.Name;
            fieldData[registeredProperty.Index] = newValue;

            SetParent(newValue.Value);
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


        [PortalDataMember]
        protected IDictionary<uint, P> fieldData = new ConcurrentDictionary<uint, P>();

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

        protected abstract P CreatePropertyValue<PV>(IRegisteredProperty registeredProperty, PV value);

        public IRegisteredProperty GetRegisteredProperty(string name)
        {
            return RegisteredPropertyManager.GetRegisteredProperty(name);
        }

        void ISetTarget.SetTarget(IBase target)
        {
            this.Target = (T)(target ?? throw new ArgumentNullException(nameof(target)));
        }


        public virtual void LoadProperty<PV>(string name, PV newValue)
        {
            LoadProperty(GetRegisteredProperty(name), newValue);
        }

        public virtual void LoadProperty<PV>(IRegisteredProperty registeredProperty, PV newValue)
        {
            Debug.Assert(!(newValue is IPropertyValue), "IPropertyValue has a different call stack");

            if (!fieldData.ContainsKey(registeredProperty.Index))
            {
                // TODO Destroy and Delink to old value
                // TODO - If they've created an event link to the PropertyValue.NotifyPropertyChanged event
                // Does just create a new one create a memory leak?
            }

            fieldData[registeredProperty.Index] = CreatePropertyValue(registeredProperty, newValue);

            SetParent(newValue);
        }




        public PV ReadProperty<PV>(string name)
        {
            return ReadProperty<PV>(GetRegisteredProperty(name));
        }

        public virtual PV ReadProperty<PV>(IRegisteredProperty registeredProperty)
        {
            if (!fieldData.TryGetValue(registeredProperty.Index, out var value))
            {
                return default(PV);
            }

            if(value is PV propertyValue)
            {
                return propertyValue;
            }

            IPropertyValue<PV> fd = value as IPropertyValue<PV> ?? throw new PropertyTypeMismatchException($"Property {registeredProperty.Name} is not type {typeof(PV).FullName}");

            return fd.Value;
        }


        public virtual IPropertyValue ReadProperty(string propertyName)
        {
            return ReadProperty(RegisteredPropertyManager.GetRegisteredProperty(propertyName));
        }

        public virtual IPropertyValue ReadProperty(IRegisteredProperty registeredProperty)
        {
            if (fieldData.TryGetValue(registeredProperty.Index, out var fd))
            {
                return fd;
            }

            return null;
        }

        protected void SetParent(object newValue)
        {
            if (newValue is ISetParent x)
            {
                if (Target == null) { throw new ArgumentNullException(nameof(Target)); }
                x.SetParent(Target);
            }
        }

        //public virtual async Task HandlePropertyChange(string propertyName, object source)
        //{
        //    foreach (var item in fieldData)
        //    {
        //        if(item is INotifiedOfPropertyChanged notified)
        //        {
        //            await notified.HandlePropertyChange(propertyName, source);
        //        }
        //    }
        //}

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

