﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Neatoo.Core
{

    public interface IProperty : INotifyPropertyChanged, INotifyNeatooPropertyChanged
    {
        string Name { get; }
        object? Value { get; set; }
        string? StringValue { get; set; }
        Task SetStringValue(string? value);
        Task SetValue<P>(P? newValue);
        Task Task { get; }
        bool IsBusy { get; }
        bool IsSelfBusy { get; }
        Task WaitForTasks();
        TaskAwaiter GetAwaiter() => Task.GetAwaiter();
    }

    public interface IProperty<T> : IProperty
    {
        new T? Value { get; set; }
    }

    public class PropertyChangedBreadCrumbs
    {
        public PropertyChangedBreadCrumbs(string propertyName, object source)
        {
            PropertyName = propertyName;
            Source = source;
        }

        public PropertyChangedBreadCrumbs(string propertyName, object source, PropertyChangedBreadCrumbs? previousPropertyName) : this(propertyName, source)
        {
            PreviousPropertyName = previousPropertyName;
        }

        public string PropertyName { get; private set; }
        public object Source { get; private set; }
        public PropertyChangedBreadCrumbs? PreviousPropertyName { get; private set; }
        public string FullPropertyName => PropertyName + (PreviousPropertyName == null ? "" : "." + PreviousPropertyName.FullPropertyName);
    }

    public delegate Task NeatooPropertyChanged(PropertyChangedBreadCrumbs propertyNameBreadCrumbs);

    public class Property<T> : IProperty<T>, IProperty, INotifyPropertyChanged, IJsonOnDeserialized
    {
        public string Name { get; }

        protected T? _value = default;

        public virtual T? Value
        {
            get => _value;
            set
            {
                SetValue(value);
            }
        }

        [JsonIgnore]
        public virtual string? StringValue
        {
            get
            {
                Console.WriteLine($"Read StringValue: {Value?.ToString()}");
                return Value?.ToString();
            }
            set
            {
                Task = SetStringValue(value);
            }
        }

        public async Task SetStringValue(string? value)
        {
            Console.WriteLine($"Set StringValue: {value}");
            if (value == null)
            {
                await SetValue<T?>(default);
            }
            else
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                if (converter != null && converter.IsValid(value))
                {
                    await SetValue((T?)converter.ConvertFromString(value));
                }
            }
        }

        object? IProperty.Value { get => Value; set => SetValue(value); }

        [JsonIgnore]
        public Task Task { get; protected set; } = Task.CompletedTask;

        protected IBase? ValueAsBase => Value as IBase;

        public bool IsBusy => ValueAsBase?.IsBusy ?? false || IsSelfBusy;

        public async Task WaitForTasks()
        {
            await (ValueAsBase?.WaitForTasks() ?? Task.CompletedTask);
        }

        public bool IsSelfBusy { get; private set; } = false;

        public virtual Task SetValue<P>(P? newValue)
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

            if (Task.Exception != null)
            {
                throw Task.Exception;
            }

            return Task;
        }

        protected virtual void HandleNullValue()
        {
            if (_value is INotifyNeatooPropertyChanged neatooPropertyChanged)
            {
                neatooPropertyChanged.NeatooPropertyChanged -= PassThruValueNeatooPropertyChanged;
            }

            _value = default;
            OnPropertyChanged(nameof(Value));

            Task = OnValueNeatooPropertyChanged(new PropertyChangedBreadCrumbs(this.Name, this));
        }

        protected virtual void HandleNonNullValue(T value)
        {
            var isDiff = !AreSame(_value, value);

            if (isDiff)
            {
                if (_value is INotifyNeatooPropertyChanged neatooPropertyChanged)
                {
                    neatooPropertyChanged.NeatooPropertyChanged -= PassThruValueNeatooPropertyChanged;
                }

                if (value is INotifyNeatooPropertyChanged valueNeatooPropertyChanged)
                {
                    valueNeatooPropertyChanged.NeatooPropertyChanged += PassThruValueNeatooPropertyChanged;
                }

                if (_value is IBase _valueBase){
                    if (_valueBase.IsBusy)
                    {
                        throw new Exception("Cannot remove a child that is busy");
                    }
                }
                
                if (value is IBase valueBase)
                {
                    if (valueBase.IsBusy)
                    {
                        throw new Exception("Cannot add a child that is busy");
                    }
                }

                if(_value is ISetParent _valueSetParent)
                {
                    _valueSetParent.SetParent(null);
                }


            }

            _value = value;

            if (isDiff)
            {
                OnPropertyChanged(nameof(Value));

                Task = OnValueNeatooPropertyChanged(new PropertyChangedBreadCrumbs(this.Name, this));
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual Task PassThruValueNeatooPropertyChanged(PropertyChangedBreadCrumbs breadCrumbs)
        {
            return NeatooPropertyChanged?.Invoke(new PropertyChangedBreadCrumbs(this.Name, breadCrumbs.Source, breadCrumbs)) ?? Task.CompletedTask;
        }
        private Task IsSelfBusyTask = Task.CompletedTask;

        protected virtual Task OnValueNeatooPropertyChanged(PropertyChangedBreadCrumbs breadCrumbs)
        {
            // ValidateBase sticks Task into AsyncTaskSequencer for us
            // so that it will be awaited by WaitForTasks()
            var task = NeatooPropertyChanged?.Invoke(breadCrumbs) ?? Task.CompletedTask;

            if (!task.IsCompleted && !IsSelfBusy)
            {
                IsSelfBusy = true;

                OnPropertyChanged(nameof(IsBusy));
                OnPropertyChanged(nameof(IsSelfBusy));

                task = task.ContinueWith(_ =>
                {
                    IsSelfBusy = false;
                    OnPropertyChanged(nameof(IsBusy));
                    OnPropertyChanged(nameof(IsSelfBusy));
                    if(_.Exception != null)
                    {
                        throw _.Exception;
                    }
                });
            }

            return task;
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

        protected virtual bool AreSame<P>(P? oldValue, P? newValue)
        {
            if (oldValue == null && newValue == null)
            {
                return true;
            }
            else if (oldValue == null || newValue == null)
            {
                return false;
            }

            if (!typeof(P).IsValueType)
            {
                return (ReferenceEquals(oldValue, newValue));
            }
            else
            {
                return oldValue.Equals(newValue);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public event NeatooPropertyChanged? NeatooPropertyChanged;

        public void OnDeserialized()
        {
            if (Value is INotifyNeatooPropertyChanged neatooPropertyChanged)
            {
                neatooPropertyChanged.NeatooPropertyChanged += PassThruValueNeatooPropertyChanged;
            }
        }
    }
}




