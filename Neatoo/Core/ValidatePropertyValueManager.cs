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


        new IValidatePropertyValue GetProperty(string propertyName);
        new IValidatePropertyValue GetProperty(IRegisteredProperty registeredProperty);

        public new IValidatePropertyValue this[string propertyName] { get => GetProperty(propertyName); }
        public new IValidatePropertyValue this[IRegisteredProperty registeredProperty] { get => GetProperty(registeredProperty); }
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

        void LoadProperty(object value);
    }

    public interface IValidatePropertyValue<T> : IValidatePropertyValue, IPropertyValue<T>
    {

    }

    public class ValidatePropertyValue<T> : PropertyValue<T>, IValidatePropertyValue<T>
    {

        public virtual IValidateBase Child => Value as IValidateBase;

        public ValidatePropertyValue(string name) : base(name)
        {
        }

        public bool IsValid => (Child?.IsValid ?? true);
        public bool IsBusy => (Child?.IsBusy ?? false);

        public Task WaitForRules() { return Child?.WaitForRules() ?? Task.CompletedTask; }
        public Task CheckAllRules(CancellationToken token) { return Child?.CheckAllRules(token) ?? Task.CompletedTask; }

        public virtual void LoadProperty(object value)
        {
            if (value is T x)
            {
                _value = x;
            }
            else
            {
                throw new RegisteredPropertyValidateChildDataWrongTypeException();
            }
        }

    }

    public class ValidatePropertyValueManager<T> : ValidatePropertyValueManagerBase<T, IValidatePropertyValue>, IValidatePropertyValueManager<T>
        where T : IBase
    {

        IRegisteredPropertyManager<T> IPropertyValueManager<T>.RegisteredPropertyManager => RegisteredPropertyManager;

        public ValidatePropertyValueManager(IRegisteredPropertyManager<T> registeredPropertyManager, IFactory factory) : base(registeredPropertyManager, factory)
        {

        }

        protected override IValidatePropertyValue CreatePropertyValue<PV>(IRegisteredProperty registeredProperty, IBase parent)
        {
            return Factory.CreateValidatePropertyValue<PV>(registeredProperty, parent);
        }

        public IValidatePropertyValue this[string propertyName]
        {
            get => GetProperty(propertyName);
        }

        public IValidatePropertyValue this[IRegisteredProperty registeredProperty]
        {
            get => GetProperty(registeredProperty);
        }


        public virtual IValidatePropertyValue GetProperty(string propertyName)
        {
            return GetProperty(RegisteredPropertyManager.GetRegisteredProperty(propertyName));
        }

        public virtual IValidatePropertyValue GetProperty(IRegisteredProperty registeredProperty)
        {
            if (fieldData.TryGetValue(registeredProperty.Index, out var fd))
            {
                return fd;
            }

            var newPropertyValue = (IValidatePropertyValue) this.GetType().GetMethod(nameof(this.CreatePropertyValue), System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).MakeGenericMethod(registeredProperty.Type).Invoke(this, new object[] { registeredProperty, Target });

            fieldData[registeredProperty.Index] = newPropertyValue;

            return newPropertyValue;
        }

        IPropertyValue IPropertyValueManager.GetProperty(string propertyName)
        {
            return GetProperty(propertyName);
        }

        IPropertyValue IPropertyValueManager.GetProperty(IRegisteredProperty registeredProperty)
        {
            return GetProperty(registeredProperty);
        }
    }

    public abstract class ValidatePropertyValueManagerBase<T, P> : PropertyValueManagerBase<T, P>
        where T : IBase
        where P : IValidatePropertyValue
    {
        public ValidatePropertyValueManagerBase(IRegisteredPropertyManager<T> registeredPropertyManager, IFactory factory) : base(registeredPropertyManager, factory)
        {
        }

        public bool IsValid => !fieldData.Values.Any(_ => !_.IsValid);

        public bool IsBusy => fieldData.Values.Any(_ => _.IsBusy);


        public Task WaitForRules()
        {
            return Task.WhenAll(fieldData.Values.Select(x => x.WaitForRules()));
        }

        public Task CheckAllRules(CancellationToken token)
        {
            var tasks = fieldData.Values.Select(x => x.CheckAllRules(token)).ToList();
            return Task.WhenAll(tasks.Where(t => t != null));
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
