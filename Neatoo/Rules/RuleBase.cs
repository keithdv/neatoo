using Neatoo.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Neatoo.Rules
{

    public interface IRule
    {
        /// <summary>
        /// Must be unique for every rule across all types
        /// </summary>
        uint UniqueIndex { get; }
        IReadOnlyList<ITriggerProperty> TriggerProperties { get; }

    }

    /// <summary>
    /// Contravariant - Allows RuleManager to call even when generic types are different
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRule<in T> : IRule
    {
        Task<PropertyErrors> RunRule(T target, CancellationToken token);
    }

    internal static class RuleIndexer
    {
        private static uint staticIndex = 0;
        internal static uint StaticIndex
        {
            get
            {
                staticIndex++;
                return staticIndex;
            }
        }
    }




    public abstract class AsyncRuleBase<T> : IRule<T>
        where T : IValidateBase
    {


        protected AsyncRuleBase()
        {
            /// Must be unique for every rule across all types so Static counter is important
            UniqueIndex = RuleIndexer.StaticIndex;
        }

        public AsyncRuleBase(params string[] triggerOnPropertyNames) : this(triggerOnPropertyNames.AsEnumerable()) { }

        public AsyncRuleBase(IEnumerable<string> triggerOnPropertyNames) : this()
        {
            TriggerProperties.AddRange(triggerOnPropertyNames.Select(propertyName => new TriggerProperty(propertyName)));
        }

        public AsyncRuleBase(params IRegisteredProperty[] triggerOnProperty) : this(triggerOnProperty.Select(t => t.Name).AsEnumerable()) { }

        /// <summary>
        /// Define the properties without using nameof()
        /// Much more involved and complicated
        /// I don't know that I am going to support this long term. 
        /// </summary>
        /// <param name="triggerProperties"></param>
        public AsyncRuleBase(IEnumerable<IRegisteredProperty> triggerOnProperty) : this(triggerOnProperty.Select(t => t.Name))
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public uint UniqueIndex { get; }

        protected PropertyErrors None = PropertyErrors.None;

        IReadOnlyList<ITriggerProperty> IRule.TriggerProperties => TriggerProperties.AsReadOnly();
        protected List<ITriggerProperty> TriggerProperties { get; } = new List<ITriggerProperty>();

        protected AsyncLocal<T> runRuleTarget = new AsyncLocal<T>();

        protected T RunRuleTarget { get => runRuleTarget.Value; set { runRuleTarget.Value = value; } }

        protected virtual void AddTriggerProperties(params string[] triggerOnPropertyNames)
        {
            TriggerProperties.AddRange(triggerOnPropertyNames.Select(propertyName => new TriggerProperty(propertyName)));
        }

        public abstract Task<PropertyErrors> Execute(T t, CancellationToken token);


        public async Task<PropertyErrors> RunRule(T target, CancellationToken token)
        {
            // I want to allow registering rules as static

            RunRuleTarget = target;
            try
            {
                var propertyErrors = await Execute(target, token);

                foreach (var property in TriggerProperties)
                {
                    if (propertyErrors.TryGetValue(property.PropertyName, out var errors))
                    {
                        target[property.PropertyName].SetErrorsForRule(UniqueIndex, errors);
                    }
                    else
                    {
                        target[property.PropertyName].ClearErrorsForRule(UniqueIndex);
                    }
                }

                return propertyErrors;
            }
            catch (Exception ex)
            {
                TriggerProperties.ForEach(p =>
                    {
                        var propertyValue = target[p.PropertyName];
                        propertyValue.SetErrorsForRule(UniqueIndex, [ex.Message]);
                    });

                throw;
            }
            finally
            {
                runRuleTarget.Value = default;
            }
        }


        protected object ReadProperty(string propertyName)
        {
            return RunRuleTarget[propertyName].Value;
        }

        protected object ReadProperty(ITriggerProperty triggerProperty)
        {
            return RunRuleTarget[triggerProperty.PropertyName].Value;
        }

        protected P ReadProperty<P>(IRegisteredProperty registeredProperty)
        {
            return (P)RunRuleTarget[registeredProperty].Value;
        }

        protected void SetProperty<P>(IRegisteredProperty registeredProperty, P value)
        {
            RunRuleTarget[registeredProperty].SetValue(value);
        }

        /// <summary>
        /// Write a property without re-running any rules
        /// </summary>
        /// <typeparam name="P"></typeparam>
        /// <param name="target"></param>
        /// <param name="registeredProperty"></param>
        /// <param name="value"></param>
        protected void LoadProperty<P>(IRegisteredProperty registeredProperty, P value)
        {
            if (RunRuleTarget[registeredProperty] is IValidateProperty editProperty)
            {
                editProperty.LoadValue(value);
            }
            else
            {
                RunRuleTarget[registeredProperty].SetValue(value);
            }
        }

    }


    public abstract class RuleBase<T> : AsyncRuleBase<T>
        where T : IValidateBase
    {
        protected RuleBase() { }

        protected RuleBase(params string[] triggerOnPropertyNames) : base(triggerOnPropertyNames) { }

        protected RuleBase(IEnumerable<string> triggerOnPropertyNames) : base(triggerOnPropertyNames) { }

        protected RuleBase(params IRegisteredProperty[] triggerOnProperty) : base(triggerOnProperty) { }

        protected RuleBase(IEnumerable<IRegisteredProperty> triggerOnProperty) : base(triggerOnProperty) { }

        public abstract PropertyErrors Execute(T target);

        public sealed override Task<PropertyErrors> Execute(T target, CancellationToken token)
        {
            return Task.FromResult(Execute(target));
        }

    }

    public class ActionFluentRule<T> : RuleBase<T>
    where T : IValidateBase
    {
        private Action<T> ExecuteFunc { get; }
        public ActionFluentRule(Action<T> execute, string triggerProperty) : base([triggerProperty])
        {
            this.ExecuteFunc = execute;
        }

        public override PropertyErrors Execute(T target)
        {
            ExecuteFunc(target);
            return PropertyErrors.None;
        }
    }

    public class ActionAsyncFluentRule<T> : AsyncRuleBase<T>
where T : IValidateBase
    {
        private Func<T, Task> ExecuteFunc { get; }
        public ActionAsyncFluentRule(Func<T, Task> execute, string triggerProperty) : base([triggerProperty])
        {
            this.ExecuteFunc = execute;
        }

        override public async Task<PropertyErrors> Execute(T target, CancellationToken token)
        {
            await ExecuteFunc(target);
            return PropertyErrors.None;
        }
    }

    public class ValidationFluentRule<T> : RuleBase<T>
        where T : IValidateBase
    {
        private Func<T, string> ExecuteFunc { get; }
        public ValidationFluentRule(Func<T, string> execute, string triggerProperty) : base([triggerProperty])
        {
            this.ExecuteFunc = execute;
        }

        public override PropertyErrors Execute(T target)
        {
            var result = ExecuteFunc(target);

            if (string.IsNullOrWhiteSpace(result))
            {
                return PropertyErrors.None;
            }
            else
            {
                return TriggerProperties.Single().PropertyError(result);
            }
        }
    }

    public class AsyncFluentRule<T> : AsyncRuleBase<T>
    where T : IValidateBase
    {
        private Func<T, Task<string>> ExecuteFunc { get; }

        public AsyncFluentRule(Func<T, Task<string>> execute, string triggerProperties) : base(triggerProperties)
        {
            this.ExecuteFunc = execute;
        }

        public override async Task<PropertyErrors> Execute(T target, CancellationToken token)
        {
            var result = await ExecuteFunc(target);

            if (string.IsNullOrWhiteSpace(result))
            {
                return PropertyErrors.None;
            }
            else
            {
                return TriggerProperties.Single().PropertyError(result);
            }
        }
    }

    public class PropertyErrors : Dictionary<string, List<string>>
    {

        public static PropertyErrors None = new PropertyErrors();

        public void Add(string propertyName, string message)
        {
            if (TryGetValue(propertyName, out var messages))
            {
                messages.Add(message);
            }
            else
            {
                Add(propertyName, new List<string> { message });
            }
        }

        public static implicit operator PropertyErrors(PropertyError error)
        {
            return new PropertyErrors { [error.PropertyName] = new List<string> { error.Message } };
        }
        public static implicit operator PropertyErrors((string name, string errorMessage) propertyError)
        {
            return new PropertyError(propertyError.name, propertyError.errorMessage);
        }
    }

    public class PropertyError
    {
        public string PropertyName { get; }
        public string Message { get; }
        public PropertyError(string propertyName, string message)
        {
            PropertyName = propertyName;
            Message = message;
        }

        public static implicit operator PropertyError((string name, string errorMessage) propertyError)
        {
            return new PropertyError(propertyError.name, propertyError.errorMessage);
        }

    }

    public static class PropertyErrorExtension
    {
        public static PropertyError PropertyError(this string propertyName, string message)
        {
            return new PropertyError(propertyName, message);
        }
    }
}