using Neatoo.Rules;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Neatoo.Core
{
    public interface IValidatePropertyValueManager : IPropertyValueManager
    {
        bool IsValid { get; }
        bool IsBusy { get; }
        Task CheckAllRules(CancellationToken token);
        Task WaitForRules();
        Task<bool> SetProperty<P>(IRegisteredProperty<P> registeredProperty, P newValue);


    }

    public interface IValidatePropertyValueManager<T> : IValidatePropertyValueManager, IPropertyValueManager<T>
    {

    }

    public interface IValidatePropertyValue : IPropertyValue, INotifyPropertyChanged
    {
        bool IsValid { get; }
        bool IsBusy { get; }
        Task CheckAllRules(CancellationToken token);

        Task WaitForRules();
    }

    public interface IValidatePropertyValue<T> : IValidatePropertyValue, IPropertyValue<T>
    {

    }

    public class ValidatePropertyValue<T> : PropertyValue<T>, IValidatePropertyValue<T>
    {

        public virtual IValidateBase Child => Value as IValidateBase;
        public ValidatePropertyValue(string name, T value) : base(name, value)
        {
        }

        public override T Value
        {
            get => base.Value;
            set
            {

                OnValueChanging(base.Value, value);
                base.Value = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
                OnValueChanged(base.Value);
            }
        }


        /// <summary>
        /// Before any checks on if the value actually changed
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected virtual void OnValueChanging(T oldValue, T newValue)
        {

        }
        protected virtual void OnValueChanged(T newValue)
        {

        }

        public bool IsValid => (Child?.IsValid ?? true);
        public bool IsBusy => (Child?.IsBusy ?? false);

        public Task WaitForRules() { return Child?.WaitForRules() ?? Task.CompletedTask; }
        public Task CheckAllRules(CancellationToken token) { return Child?.CheckAllRules(token) ?? Task.CompletedTask; }

        public event PropertyChangedEventHandler PropertyChanged;

        // TODO : Future - get and assign PropertyValue<>
        // Gives the ability to wait for a property set if the Property is PropertyValue<XYZ>
        // Too complicated for now.

        //public static implicit operator T(ValidatePropertyValue<T> value) => value.Value;
        //public static implicit operator ValidatePropertyValue<T>(T value) => new ValidatePropertyValue<T>(value);
        //public static implicit operator Task(ValidatePropertyValue<T> value) => value.Task;
    }

    public class ValidatePropertyValueManager<T> : ValidatePropertyValueManagerBase<T, IValidatePropertyValue>, IValidatePropertyValueManager<T>
        where T : IBase
    {

        IRegisteredPropertyManager<T> IPropertyValueManager<T>.RegisteredPropertyManager => RegisteredPropertyManager;

        public ValidatePropertyValueManager(IRegisteredPropertyManager<T> registeredPropertyManager, IFactory factory, IValuesDiffer valuesDiffer) : base(registeredPropertyManager, factory, valuesDiffer)
        {

        }

        protected override IValidatePropertyValue CreatePropertyValue<PV>(IRegisteredProperty<PV> registeredProperty, PV value)
        {
            return Factory.CreateValidatePropertyValue(registeredProperty, value);
        }


        public virtual void LoadProperty<PV>(IRegisteredProperty<PropertyValue<PV>> registeredProperty, PropertyValue<PV> newValue)
        {
            if (!fieldData.ContainsKey(registeredProperty.Index))
            {
                // TODO Destroy and Delink to old value
                // TODO - If they've created an event link to the PropertyValue.NotifyPropertyChanged event
                // Does just create a new one create a memory leak?
            }
            newValue.Name = registeredProperty.Name;
            fieldData[registeredProperty.Index] = (IValidatePropertyValue) newValue;

            SetParent(newValue.Value);
        }

    }

    public abstract class ValidatePropertyValueManagerBase<T, P> : PropertyValueManagerBase<T, P>
        where T : IBase
        where P : IValidatePropertyValue
    {
        public ValidatePropertyValueManagerBase(IRegisteredPropertyManager<T> registeredPropertyManager, IFactory factory, IValuesDiffer valuesDiffer) : base(registeredPropertyManager, factory)
        {
            ValuesDiffer = valuesDiffer;
        }

        public bool IsValid => !fieldData.Values.Any(_ => !_.IsValid);

        public bool IsBusy => fieldData.Values.Any(_ => _.IsBusy);

        public IValuesDiffer ValuesDiffer { get; }

        public Task WaitForRules()
        {
            return Task.WhenAll(fieldData.Values.Select(x => x.WaitForRules()));
        }

        public Task CheckAllRules(CancellationToken token)
        {
            var tasks = fieldData.Values.Select(x => x.CheckAllRules(token)).ToList();
            return Task.WhenAll(tasks.Where(t => t != null));
        }

        public virtual Task<bool> SetProperty<PV>(string name, PV newValue)
        {
            return SetProperty(GetRegisteredProperty<PV>(name), newValue);
        }

        public virtual async Task<bool> SetProperty<PV>(IRegisteredProperty<PV> registeredProperty, PV newValue)
        {
            if (!fieldData.TryGetValue(registeredProperty.Index, out var value))
            {
                // Default(P) so that it get's marked dirty
                // Maybe it would be better to use MarkSelfModified; you know; once I write that
                fieldData[registeredProperty.Index] = value = CreatePropertyValue(registeredProperty, default(PV));
            }

            PropertyValue<PV> fd = value as PropertyValue<PV> ?? throw new PropertyTypeMismatchException($"Property {registeredProperty.Name} is not type {typeof(PV).FullName}");

            if (ValuesDiffer.Check(fd.Value, newValue))
            {
                SetParent(newValue);
                fd.Value = newValue;
                OnPropertyChanged(registeredProperty.Name);
                return true;
            } else
            {
                return false;
            }
        }
    }


    [Serializable]
    public class RegisteredPropertyValidateChildDataWrongTypeException : Exception
    {
        public RegisteredPropertyValidateChildDataWrongTypeException() { }
        public RegisteredPropertyValidateChildDataWrongTypeException(string message) : base(message) { }
        public RegisteredPropertyValidateChildDataWrongTypeException(string message, Exception inner) : base(message, inner) { }
        protected RegisteredPropertyValidateChildDataWrongTypeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

}
