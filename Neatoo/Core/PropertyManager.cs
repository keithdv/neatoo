using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Neatoo.Core
{
    public interface IPropertyManager<out P> : INotifyNeatooPropertyChanged
        where P : IProperty
    {
        bool IsBusy { get; }
        bool IsSelfBusy { get; }
        Task WaitForTasks();
        IRegisteredProperty GetRegisteredProperty(string name);
        bool HasProperty(string propertyName);

        P GetProperty(string propertyName);
        P GetProperty(IRegisteredProperty registeredProperty);

        public P this[string propertyName] { get => GetProperty(propertyName); }
        public P this[IRegisteredProperty registeredProperty] { get => GetProperty(registeredProperty); }

        internal IRegisteredPropertyManager RegisteredPropertyManager { get; }

        internal IEnumerable<P> GetProperties { get; }

        void SetProperties(IEnumerable<IProperty> properties);
    }

    public delegate IPropertyManager<IProperty> CreatePropertyManager(IRegisteredPropertyManager registeredPropertyManager);

    public class PropertyManager<P> : IPropertyManager<P>, IJsonOnDeserialized
        where P : IProperty
    {
        protected IFactory Factory { get; }

        protected readonly IRegisteredPropertyManager RegisteredPropertyManager;

        IRegisteredPropertyManager IPropertyManager<P>.RegisteredPropertyManager => RegisteredPropertyManager;

        public bool IsBusy => PropertyBag.Values.Any(_ => _.IsBusy);
        public bool IsSelfBusy => PropertyBag.Values.Any(_ => _.IsSelfBusy);

        public bool HasProperty(string propertyName)
        {
            return RegisteredPropertyManager.HasProperty(propertyName);
        }

        protected IDictionary<string, P> _propertyBag = new Dictionary<string, P>();

        protected IDictionary<string, P> PropertyBag
        {
            get => _propertyBag;
            set
            {
                _propertyBag = value;
            }
        }
        
        public async Task WaitForTasks() {
            foreach(var p in PropertyBag.Values.ToList())
            {
                await p.WaitForTasks();
            }
        }

        public event NeatooPropertyChanged? NeatooPropertyChanged;

        private Task _OnNeatooPropertyChanged(PropertyNameBreadCrumbs breadCrumbs)
        {
            return NeatooPropertyChanged?.Invoke(breadCrumbs) ?? Task.CompletedTask;
        }

        public PropertyManager(IRegisteredPropertyManager registeredPropertyManager, IFactory factory)
        {
            this.RegisteredPropertyManager = registeredPropertyManager;
            Factory = factory;
        }

        protected IProperty CreateProperty<PV>(IRegisteredProperty registeredProperty)
        {
            return Factory.CreateProperty<PV>(registeredProperty);
        }

        public IRegisteredProperty GetRegisteredProperty(string name)
        {
            return RegisteredPropertyManager.GetRegisteredProperty(name);
        }

        public virtual P GetProperty(IRegisteredProperty registeredProperty)
        {
            if (PropertyBag.TryGetValue(registeredProperty.Name, out var fd))
            {
                return fd;
            }

            var newProperty = (P)this.GetType().GetMethod(nameof(this.CreateProperty), System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!.MakeGenericMethod(registeredProperty.Type).Invoke(this, new object[] { registeredProperty })!;

            newProperty.NeatooPropertyChanged += _OnNeatooPropertyChanged;

            PropertyBag[registeredProperty.Name] = newProperty;

            return newProperty;
        }

        public P this[string propertyName]
        {
            get => GetProperty(propertyName);
        }

        public P this[IRegisteredProperty registeredProperty]
        {
            get => GetProperty(registeredProperty);
        }

        public virtual P GetProperty(string propertyName)
        {
            return GetProperty(RegisteredPropertyManager.GetRegisteredProperty(propertyName));
        }

        void IPropertyManager<P>.SetProperties(IEnumerable<IProperty> properties)
        {
            foreach (var p in properties.Cast<P>())
            {
                if (PropertyBag.TryGetValue(p.Name, out var fd))
                {
                    throw new InvalidOperationException("Property already set");
                }
                PropertyBag[p.Name] = p;
            }
        }

        public void OnDeserialized()
        {
            foreach (var p in PropertyBag.Values)
            {
                p.NeatooPropertyChanged += _OnNeatooPropertyChanged;
            }
        }

        IEnumerable<P> IPropertyManager<P>.GetProperties => PropertyBag.Values.AsEnumerable();
    }

    [Serializable]
    public class PropertyTypeMismatchException : Exception
    {
        public PropertyTypeMismatchException() { }
        public PropertyTypeMismatchException(string message) : base(message) { }
        public PropertyTypeMismatchException(string message, Exception inner) : base(message, inner) { }
    }

    [Serializable]
    public class PropertyNotFoundException : Exception
    {
        public PropertyNotFoundException() { }
        public PropertyNotFoundException(string message) : base(message) { }
        public PropertyNotFoundException(string message, Exception inner) : base(message, inner) { }
    }
}




