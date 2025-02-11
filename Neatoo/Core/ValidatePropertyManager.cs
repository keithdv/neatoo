using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
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
        Task RunAllRules(CancellationToken token);

        IReadOnlyList<string> ErrorMessages { get; }
        void ClearAllErrors();
        void ClearSelfErrors();
    }

    public interface IValidateProperty : IProperty, INotifyPropertyChanged
    {
        bool IsSelfValid { get; }
        bool IsValid { get; }
        Task RunAllRules(CancellationToken token);
        IReadOnlyList<string> ErrorMessages { get; }
        /// <summary>
        /// Sets the value without running any rules or raising any events
        /// </summary>
        /// <param name="value"></param>
        void LoadValue(object? value);
        internal void SetErrorsForRule(uint ruleIndex, IReadOnlyList<string> errorMessages);
        internal void ClearErrorsForRule(uint ruleIndex);
        internal void ClearAllErrors();
        internal void ClearSelfErrors();
    }

    public interface IValidateProperty<T> : IValidateProperty, IProperty<T>
    {

    }

    public class ValidateProperty<T> : Property<T>, IValidateProperty<T>
    {
        [JsonIgnore]
        public virtual IValidateMetaProperties? ValueIsValidateBase => Value as IValidateMetaProperties;

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

        public Task RunAllRules(CancellationToken token) { return ValueIsValidateBase?.RunAllRules(token) ?? Task.CompletedTask; }

        [JsonIgnore]
        public IReadOnlyList<string> ErrorMessages => RuleErrorMessages.SelectMany(r => r.Value).ToList().AsReadOnly();

        public virtual void LoadValue(object? value)
        {
            if(AreSame(_value, value))
            {
                return;
            }

            if (value == null)
            {
                _value = default;
            }
            else if (value is T x)
            {
                _value = x;
            }
            else
            {
                throw new PropertyValidateChildDataWrongTypeException();
            }

            OnPropertyChanged(nameof(Value));
        }


        // [PortalDataMember] Ummm...ising the RuleIndex going to be different...
        protected IDictionary<uint, List<string>> RuleErrorMessages { get; } = new ConcurrentDictionary<uint, List<string>>();

        public string[] SerializedErrorMessages => RuleErrorMessages.SelectMany(r => r.Value).ToArray();

        protected void SetError(uint ruleIndex, IReadOnlyList<string> errorMessages)
        {
            Console.WriteLine($"Set Error: {Name} {ruleIndex} {errorMessages.Count}");

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
            Console.WriteLine($"Clear Error: {Name} {ruleIndex}");
            RuleErrorMessages.Remove(ruleIndex);
            OnPropertyChanged(nameof(IsValid));
            OnPropertyChanged(nameof(ErrorMessages));
        }

        public virtual void ClearSelfErrors()
        {
            RuleErrorMessages.Clear();
            OnPropertyChanged(nameof(IsValid));
            OnPropertyChanged(nameof(ErrorMessages));
        }

        public virtual void ClearAllErrors()
        {
            RuleErrorMessages.Clear();
            ValueIsValidateBase?.ClearAllErrors();

            OnPropertyChanged(nameof(IsValid));
            OnPropertyChanged(nameof(ErrorMessages));
        }
    }


    public delegate IValidatePropertyManager<IValidateProperty> CreateValidatePropertyManager(IPropertyInfoList propertyInfoList);

    public class ValidatePropertyManager<P> : PropertyManager<P>, IValidatePropertyManager<P>
        where P : IValidateProperty
    {

        public ValidatePropertyManager(IPropertyInfoList propertyInfoList, IFactory factory) : base(propertyInfoList, factory)
        {
        }


        protected new IProperty CreateProperty<PV>(IPropertyInfo propertyInfo)
        {
            return Factory.CreateValidateProperty<PV>(propertyInfo);
        }

        public bool IsSelfValid => !PropertyBag.Values.Any(_ => !_.IsSelfValid);
        public bool IsValid => !PropertyBag.Values.Any(_ => !_.IsValid);

        public IReadOnlyList<string> ErrorMessages => PropertyBag.Values.SelectMany(_ => _.ErrorMessages).ToList().AsReadOnly();


        public async Task RunAllRules(CancellationToken token)
        {
            foreach (var p in PropertyBag.Values.ToList())
            {
                await p.RunAllRules(token);
            }
        }

        public void ClearSelfErrors()
        {
            foreach (var p in PropertyBag.Values)
            {
                p.ClearSelfErrors();
            }
        }

        public void ClearAllErrors()
        {
            foreach (var p in PropertyBag.Values)
            {
                p.ClearAllErrors();
            }
        }
    }


    [Serializable]
    public class PropertyValidateChildDataWrongTypeException : Exception
    {
        public PropertyValidateChildDataWrongTypeException() { }
        public PropertyValidateChildDataWrongTypeException(string message) : base(message) { }
        public PropertyValidateChildDataWrongTypeException(string message, Exception inner) : base(message, inner) { }

    }

}
