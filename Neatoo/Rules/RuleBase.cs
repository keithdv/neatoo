using Neatoo.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
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

    }

    /// <summary>
    /// Contravariant - Allows RuleManager to call even when generic types are different
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRule<in T> : IRule
    {
        Task<PropertyErrors> RunRule(T target, CancellationToken token);


        IReadOnlyList<ITriggerProperty<T>> TriggerProperties { get; }
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

        public AsyncRuleBase(params Expression<Func<T, object?>>[] triggerOnPropertyNames) : this(triggerOnPropertyNames.AsEnumerable()) { }

        public AsyncRuleBase(IEnumerable<Expression<Func<T, object?>>> triggerOnPropertyNames) : this()
        {
            TriggerProperties.AddRange(triggerOnPropertyNames.Select(propertyName => new TriggerProperty<T, object?>(propertyName)));
        }

        /// <summary>
        /// 
        /// </summary>
        public uint UniqueIndex { get; }

        protected PropertyErrors None = PropertyErrors.None;

        IReadOnlyList<ITriggerProperty<T>> IRule<T>.TriggerProperties => TriggerProperties.AsReadOnly();
        protected List<ITriggerProperty<T>> TriggerProperties { get; } = new List<ITriggerProperty<T>>();

        protected virtual void AddTriggerProperties(params Expression<Func<T, object?>>[] triggerOnExpression)
        {
            TriggerProperties.AddRange(triggerOnExpression.Select(expression => new TriggerProperty<T, object?>(expression)));
        }

        protected virtual void AddTriggerProperties(params ITriggerProperty<T>[] triggerProperties)
        {
            TriggerProperties.AddRange(triggerProperties);
        }

        public abstract Task<PropertyErrors> Execute(T t, CancellationToken token);


        public async Task<PropertyErrors> RunRule(T target, CancellationToken token)
        {
            // I want to allow registering rules as static
            try
            {
                var propertyErrors = await Execute(target, token);

                var setAtLeastOneProperty = true;

                foreach (var property in TriggerProperties)
                {
                    if (target.PropertyManager.HasProperty(property.PropertyName)) // TODO - Improve - Only allow expressions and child property callouts
                    {
                        setAtLeastOneProperty = true;
                        if (propertyErrors.TryGetValue(property.PropertyName, out var errors))
                        {
                            target[property.PropertyName].SetErrorsForRule(UniqueIndex, errors);
                        }
                        else
                        {
                            target[property.PropertyName].ClearErrorsForRule(UniqueIndex);
                        }
                    }
                }

                Debug.Assert(setAtLeastOneProperty, "You must have at least one trigger property that is a valid property on the target");

                return propertyErrors;
            }
            catch (Exception ex)
            {
                TriggerProperties.ForEach(p =>
                    {
                        // Allow children
                        if(target.PropertyManager.HasProperty(p.PropertyName))
                        {
                            var propertyValue = target[p.PropertyName];
                            propertyValue.SetErrorsForRule(UniqueIndex, [ex.Message]);
                        }
                    });

                throw;
            }
        }

        /// <summary>
        /// Write a property without re-running any rules
        /// </summary>
        /// <typeparam name="P"></typeparam>
        /// <param name="target"></param>
        /// <param name="triggerProperty"></param>
        /// <param name="value"></param>
        protected void LoadProperty(T target, ITriggerProperty<T> triggerProperty, object? value)
        {
            if (target[triggerProperty.PropertyName] is IValidateProperty editProperty)
            {
                editProperty.LoadValue(value);
            }
            else
            {
                target[triggerProperty.PropertyName].SetValue(value);
            }
        }

        protected void LoadProperty<P>(T target, ITriggerProperty<T, P> triggerProperty, P? value)
        {
            if (target[triggerProperty.PropertyName] is IValidateProperty editProperty)
            {
                editProperty.LoadValue(value);
            }
            else
            {
                target[triggerProperty.PropertyName].SetValue(value);
            }
        }

        protected void LoadProperty<P>(T target, Expression<Func<T, P?>> expression, P? value)
        {
            var triggerProperty = new TriggerProperty<T, P>(expression);

            if (target[triggerProperty.PropertyName] is IValidateProperty editProperty)
            {
                editProperty.LoadValue(value);
            }
            else
            {
                target[triggerProperty.PropertyName].SetValue(value);
            }
        }

    }


    public abstract class RuleBase<T> : AsyncRuleBase<T>
        where T : IValidateBase
    {
        protected RuleBase() { }

        protected RuleBase(params Expression<Func<T, object?>>[] triggerOnPropertyNames) : base(triggerOnPropertyNames)
        {
        }

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
        public ActionFluentRule(Action<T> execute, Expression<Func<T, object?>> triggerProperty) : base([triggerProperty])
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
        public ActionAsyncFluentRule(Func<T, Task> execute, Expression<Func<T, object?>> triggerProperty) : base([triggerProperty])
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
        public ValidationFluentRule(Func<T, string> execute, Expression<Func<T, object?>> triggerProperty) : base([triggerProperty])
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
                return new PropertyError(TriggerProperties.Single().PropertyName, result);
            }
        }
    }

    public class AsyncFluentRule<T> : AsyncRuleBase<T>
    where T : IValidateBase
    {
        private Func<T, Task<string>> ExecuteFunc { get; }

        public AsyncFluentRule(Func<T, Task<string>> execute, Expression<Func<T, object?>> triggerProperty) : base([triggerProperty])
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
                return new PropertyError(TriggerProperties.Single().PropertyName, result);
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