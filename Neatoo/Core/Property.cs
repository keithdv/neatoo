﻿using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Neatoo.Core
{

    public interface IProperty : INotifyPropertyChanged, INotifyNeatooPropertyChanged
    {
        string Name { get; }
        object Value { get; set; }

        Task SetValue<P>(P newValue);

        Task Task { get; }
        bool IsBusy { get; }
        bool IsSelfBusy { get; }

        Task WaitForTasks();
        TaskAwaiter GetAwaiter() => Task.GetAwaiter();
    }

    public interface IProperty<T> : IProperty
    {
        new T Value { get; set; }
    }

    public class PropertyNameBreadCrumbs
    {
        public PropertyNameBreadCrumbs(string propertyName, object source)
        {
            PropertyName = propertyName;
            Source = source;
        }

        public PropertyNameBreadCrumbs(string propertyName, object source, PropertyNameBreadCrumbs previousPropertyName) : this(propertyName, source)
        {
            PreviousPropertyName = previousPropertyName;
        }

        public string PropertyName { get; private set; }
        public object Source { get; private set; }

        public PropertyNameBreadCrumbs PreviousPropertyName { get; private set; }

        public string FullPropertyName => PropertyName + (PreviousPropertyName == null ? "" : "." + PreviousPropertyName.FullPropertyName);
    }

    public delegate Task NeatooPropertyChanged(PropertyNameBreadCrumbs propertyNameBreadCrumbs);

    public class Property<T> : IProperty<T>, IProperty, INotifyPropertyChanged, IJsonOnDeserialized
    {
        public string Name { get; }

        protected T _value = default;

        public virtual T Value
        {
            get => _value;
            set
            {
                SetValue(value);
            }
        }

        object IProperty.Value { get => Value; set => SetValue(value); }

        [JsonIgnore]
        public Task Task { get; protected set; } = Task.CompletedTask;

        protected IBase ValueAsBase => Value as IBase;

        public bool IsBusy => ValueAsBase?.IsBusy ?? false || IsSelfBusy;

        public Task WaitForTasks() => ValueAsBase?.WaitForTasks() ?? Task.CompletedTask;

        public bool IsSelfBusy { get; private set; } = false;

        public virtual Task SetValue<P>(P newValue)
        {
            if (newValue == null && _value == null) { return Task.CompletedTask; }

            Task = Task.CompletedTask;

            if (newValue == null)
            {
                HandleNullValue();
            }
            else if (newValue is T value)
            {
                HandleNonNullValue(value);
            }
            else
            {
                throw new PropertyTypeMismatchException($"Type {newValue.GetType()} is not type {typeof(T).FullName}");
            }

            if (Task.IsCompleted && Task.IsFaulted)
            {
                throw Task.Exception;
            }

            return Task;
        }

        protected virtual void HandleNullValue()
        {
            if (_value is INotifyNeatooPropertyChanged neatooPropertyChanged)
            {
                neatooPropertyChanged.NeatooPropertyChanged -= _OnValueNeatooPropertyChanged;
            }

            _value = default;
            OnPropertyChanged(nameof(Value));

            Task = OnValueNeatooPropertyChanged(new PropertyNameBreadCrumbs(this.Name, this));
        }

        protected virtual void HandleNonNullValue(T value)
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

                Task = OnValueNeatooPropertyChanged(new PropertyNameBreadCrumbs(this.Name, this));
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual Task _OnValueNeatooPropertyChanged(PropertyNameBreadCrumbs breadCrumbs)
        {
            return OnValueNeatooPropertyChanged(new PropertyNameBreadCrumbs(this.Name, this.Value, breadCrumbs));
        }

        protected virtual Task OnValueNeatooPropertyChanged(PropertyNameBreadCrumbs breadCrumbs)
        {

            try
            {
                // ValidateBase sticks Task into AsyncTaskSequencer for us
                // so that it will be awaited by WaitForTasks()
                Task = NeatooPropertyChanged?.Invoke(breadCrumbs) ?? Task.CompletedTask;

                //if (!Task.IsCompleted)
                //{
                //    IsSelfBusy = true;

                //    OnPropertyChanged(nameof(IsBusy));
                //    OnPropertyChanged(nameof(IsSelfBusy));

                //    Task.ContinueWith(_ =>
                //    {
                //        IsSelfBusy = false;
                //        OnPropertyChanged(nameof(IsBusy));
                //        OnPropertyChanged(nameof(IsSelfBusy));
                //    });
                //}

                return Task;
            }
            catch
            {

                IsSelfBusy = false;

                throw;
            }
        }

        public Property(string name)
        {
            this.Name = name;
        }

        [JsonConstructor]
        public Property(string name, T value) : this(name)
        {
            _value = value;
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

        public void OnDeserialized()
        {
            if (Value is INotifyNeatooPropertyChanged neatooPropertyChanged)
            {
                neatooPropertyChanged.NeatooPropertyChanged += OnValueNeatooPropertyChanged;
            }
        }
    }
}




