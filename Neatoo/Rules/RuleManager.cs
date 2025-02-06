using Neatoo.Core;
using Neatoo.Rules.Rules;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Neatoo.Rules
{
    public interface IRuleManager
    {
        IEnumerable<IRule> Rules { get; }
        void AddRule(IRule rule);
        void AddRules(params IRule[] rules);
        Task CheckRulesForProperty(string propertyName);
        Task CheckAllRules(CancellationToken token = new CancellationToken());
    }

    public interface IRuleManager<T> : IRuleManager
        where T : IValidateBase
    {
        ActionFluentRule<T> AddAction(Action<T> func, string triggerProperty);
        ValidationFluentRule<T> AddValidation(Func<T, string> func, string triggerProperty);
        ActionAsyncFluentRule<T> AddActionAsync(Func<T, Task> func, string triggerProperty);
        AsyncFluentRule<T> AddValidationAsync(Func<T, Task<string>> func, string triggerProperty);
    }

    public class RuleManagerFactory<T>
        where T : IValidateBase
    {
        public RuleManagerFactory(IAttributeToRule attributeToRule)
        {
            AttributeToRule = attributeToRule;
        }

        public IAttributeToRule AttributeToRule { get; }

        public IRuleManager<T> CreateRuleManager(T target, IRegisteredPropertyManager registeredPropertyManager)
        {
            return new RuleManager<T>(target, registeredPropertyManager, AttributeToRule);
        }
    }

    public class RuleManager<T> : IRuleManager<T>
        where T : IValidateBase
    {
        protected T Target { get; }

        public RuleManager(T target, IRegisteredPropertyManager registeredPropertyManager, IAttributeToRule attributeToRule)
        {
            this.Target = target ?? throw new TargetIsNullException();
            AddAttributeRules(attributeToRule, registeredPropertyManager);
        }

        IEnumerable<IRule> IRuleManager.Rules => Rules.Values;

        private IDictionary<uint, IRule> Rules { get; } = new ConcurrentDictionary<uint, IRule>();

        protected virtual void AddAttributeRules(IAttributeToRule attributeToRule, IRegisteredPropertyManager registeredPropertyManager)
        {
            var requiredRegisteredProp = registeredPropertyManager.GetRegisteredProperties();

            foreach (var r in requiredRegisteredProp)
            {
                foreach (var a in r.PropertyInfo.GetCustomAttributes(true))
                {
                    var rule = attributeToRule.GetRule(r, a.GetType());
                    if (rule != null) { AddRule(rule); }
                }
            }
        }

        public void AddRules(params IRule[] rules)
        {
            foreach (var r in rules) { AddRule(r); }
        }

        public void AddRule(IRule rule)
        {
            Rules.Add(rule.UniqueIndex, rule ?? throw new ArgumentNullException(nameof(rule)));
        }

        public ActionAsyncFluentRule<T> AddActionAsync(Func<T, Task> func, string triggerProperty)
        {
            ActionAsyncFluentRule<T> rule = new ActionAsyncFluentRule<T>(func, triggerProperty);
            Rules.Add(rule.UniqueIndex, rule);
            return rule;
        }

        public ActionFluentRule<T> AddAction(Action<T> func, string triggerProperty)
        {
            ActionFluentRule<T> rule = new ActionFluentRule<T>(func, triggerProperty);
            Rules.Add(rule.UniqueIndex, rule);
            return rule;
        }

        public ValidationFluentRule<T> AddValidation(Func<T, string> func, string triggerProperty)
        {
            ValidationFluentRule<T> rule = new ValidationFluentRule<T>(func, triggerProperty);
            Rules.Add(rule.UniqueIndex, rule);
            return rule;
        }

        public AsyncFluentRule<T> AddValidationAsync(Func<T, Task<string>> func, string triggerProperty)
        {
            AsyncFluentRule<T> rule = new AsyncFluentRule<T>(func, triggerProperty);
            Rules.Add(rule.UniqueIndex, rule);
            return rule;
        }

        public async Task CheckRulesForProperty(string propertyName)
        {
            foreach (var rule in Rules.Values.Where(r => r.TriggerProperties.Any(t => t.IsMatch(Target, propertyName))).ToList())
            {
                await RunRule(rule, CancellationToken.None);
            }
        }

        public async Task CheckAllRules(CancellationToken token = new CancellationToken())
        {
            foreach (var ruleIndex in Rules.ToList())
            {
                await RunRule(ruleIndex.Value, token);
            }
        }

        private async Task RunRule(IRule r, CancellationToken token)
        {
            if (r is IRule<T> rule)
            {
                await rule.RunRule(Target, token);
            }
            else
            {
                throw new InvalidRuleTypeException($"{r.GetType().FullName} cannot be executed for {typeof(T).FullName}");
            }

            if (token.IsCancellationRequested)
            {
                return;
            }
        }
    }

    [Serializable]
    public class TargetRulePropertyChangeException : Exception
    {
        public TargetRulePropertyChangeException() { }
        public TargetRulePropertyChangeException(string message) : base(message) { }
        public TargetRulePropertyChangeException(string message, Exception inner) : base(message, inner) { }
        protected TargetRulePropertyChangeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class InvalidRuleTypeException : Exception
    {
        public InvalidRuleTypeException() { }
        public InvalidRuleTypeException(string message) : base(message) { }
        public InvalidRuleTypeException(string message, Exception inner) : base(message, inner) { }
        protected InvalidRuleTypeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class InvalidTargetTypeException : Exception
    {
        public InvalidTargetTypeException() { }
        public InvalidTargetTypeException(string message) : base(message) { }
        public InvalidTargetTypeException(string message, Exception inner) : base(message, inner) { }
        protected InvalidTargetTypeException(
          SerializationInfo info,
          StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class TargetIsNullException : Exception
    {
        public TargetIsNullException() { }
        public TargetIsNullException(string message) : base(message) { }
        public TargetIsNullException(string message, Exception inner) : base(message, inner) { }
        protected TargetIsNullException(
          SerializationInfo info,
          StreamingContext context) : base(info, context) { }
    }
}