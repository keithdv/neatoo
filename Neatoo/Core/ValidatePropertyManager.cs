using Neatoo.Rules;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Neatoo.Core
{
    public interface IValidatePropertyManager<out P> : IPropertyManager<P>
        where P : IProperty
    {
        // Valid without looking at Children that are IValidateBase
        bool IsSelfValid { get; }
        bool IsValid { get; }
        bool IsBusy { get; }
        Task CheckAllRules(CancellationToken token);
        Task WaitForRules();
        Task WaitForTasks();
        IReadOnlyList<string> ErrorMessages { get; }

    }

    public interface IValidateProperty : IProperty, INotifyPropertyChanged
    {
        bool IsSelfValid { get; }
        bool IsValid { get; }
        bool IsBusy { get; }
        Task CheckAllRules(CancellationToken token);
        Task WaitForRules();
        IReadOnlyList<string> ErrorMessages { get; }
        void LoadProperty(object value);
        internal void SetErrorsForRule(uint ruleIndex, IReadOnlyList<string> errorMessages);
        internal void ClearErrorsForRule(uint ruleIndex);
        internal void ClearAllErrors();
    }

    public interface IValidateProperty<T> : IValidateProperty, IProperty<T>
    {

    }

    public class ValidateProperty<T> : Property<T>, IValidateProperty<T>
    {
        [JsonIgnore]
        public virtual IValidateMetaProperties ValueIsValidateBase => Value as IValidateMetaProperties;

        public ValidateProperty(string name) : base(name)
        {
        }

        [JsonConstructor]
        public ValidateProperty(string name, T value, string[] serializedErrorMessages) : base(name, value)
        {
            for(int i = 0; i < serializedErrorMessages.Length; i++)
            {
                SetError((uint)i, new List<string> { serializedErrorMessages[i].ToString() });
            }
        }

        public bool IsSelfValid => ValueIsValidateBase != null ? true : !RuleErrorMessages.Any();
        public bool IsValid => ValueIsValidateBase != null ? ValueIsValidateBase.IsValid : !RuleErrorMessages.Any();
        public bool IsBusy => ValueIsValidateBase != null ? (ValueIsValidateBase?.IsBusy ?? false) : IsSelfBusy;

        public bool IsSelfBusy { get; private set; } = false;

        public Task WaitForRules() { return ValueIsValidateBase?.WaitForRules() ?? Task.CompletedTask; }
        public Task CheckAllRules(CancellationToken token) { return ValueIsValidateBase?.CheckAllRules(token) ?? Task.CompletedTask; }

        [JsonIgnore]
        public IReadOnlyList<string> ErrorMessages => RuleErrorMessages.SelectMany(r => r.Value).ToList().AsReadOnly();

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

        protected override Task OnValueNeatooPropertyChanged(string propertyName, object source)
        {
            try
            {
                // ValidateBase sticks Task into AsyncTaskSequencer for us
                // so that it will be awaited by WaitForRules
                Task = base.OnValueNeatooPropertyChanged(propertyName, source);

                if (!Task.IsCompleted)
                {
                    IsSelfBusy = true;

                    OnPropertyChanged(nameof(IsBusy));
                    OnPropertyChanged(nameof(IsSelfBusy));

                    Task.ContinueWith(_ =>
                    {
                        IsSelfBusy = false;
                        OnPropertyChanged(nameof(IsBusy));
                        OnPropertyChanged(nameof(IsSelfBusy));
                    });
                }

                return Task;
            }
            catch
            {

                IsSelfBusy = false;

                throw;
            }
        }

        // [PortalDataMember] Ummm...ising the RuleIndex going to be different...
        protected Dictionary<uint, List<string>> RuleErrorMessages { get; } = new Dictionary<uint, List<string>>();

        public string[] SerializedErrorMessages => RuleErrorMessages.SelectMany(r => r.Value).ToArray();

        protected void SetError(uint ruleIndex, IReadOnlyList<string> errorMessages)
        {
            Debug.Assert(ValueIsValidateBase == null, "If the Child is IValidateBase then it should be handling the errors");
            RuleErrorMessages[ruleIndex] = errorMessages.ToList();
            OnPropertyChanged(nameof(IsValid));
            OnPropertyChanged(nameof(ErrorMessages));
        }


        void IValidateProperty.SetErrorsForRule(uint ruleIndex, IReadOnlyList<string> errorMessages)
        {
            SetError(ruleIndex, errorMessages);
        }

        void IValidateProperty.ClearErrorsForRule(uint ruleIndex)
        {
            RuleErrorMessages.Remove(ruleIndex);
            OnPropertyChanged(nameof(IsValid));
            OnPropertyChanged(nameof(ErrorMessages));
        }

        void IValidateProperty.ClearAllErrors()
        {
            RuleErrorMessages.Clear();
            OnPropertyChanged(nameof(IsValid));
            OnPropertyChanged(nameof(ErrorMessages));
        }
    }

    public class ValidatePropertyManager<P> : PropertyManager<P>, IValidatePropertyManager<P>
        where P : IValidateProperty
    {

        public ValidatePropertyManager(IRegisteredPropertyManager registeredPropertyManager, IFactory factory) : base(registeredPropertyManager, factory)
        {
        }


        protected new IProperty CreateProperty<PV>(IRegisteredProperty registeredProperty)
        {
            return Factory.CreateValidateProperty<PV>(registeredProperty);
        }

        public bool IsSelfValid => !fieldData.Values.Any(_ => !_.IsSelfValid);
        public bool IsValid => !fieldData.Values.Any(_ => !_.IsValid);

        public bool IsBusy => fieldData.Values.Any(_ => _.IsBusy);

        public IReadOnlyList<string> ErrorMessages => fieldData.Values.SelectMany(_ => _.ErrorMessages).ToList().AsReadOnly();

        public Task WaitForRules()
        {
            return Task.WhenAll(fieldData.Values.Select(x => x.WaitForRules()));
        }

        public Task WaitForTasks()
        {
            return Task.WhenAll(fieldData.Values.Select(x => x.Task));
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
