﻿using Neatoo.Attributes;
using Neatoo.Core;
using Neatoo.Rules.Rules;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Neatoo.Rules
{


    public interface IRuleManager
    {

        bool IsValid { get; }
        IEnumerable<IRule> Rules { get; }
        IRuleResult OverrideResult { get; }

        /// <summary>
        /// Set OverrideResult and permenantly mark as invalid
        /// </summary>
        /// <param name="message"></param>
        void MarkInvalid(string message);

        void AddRule(IRule rule);
        void AddRules(params IRule[] rules);
        IRuleResult this[string propertyName] { get; }
        IRuleResultReadOnlyList Results { get; }
        Task CheckRulesForProperty(string propertyName);
        Task CheckAllRules(CancellationToken token = new CancellationToken());

    }

    public interface IRuleManager<T> : IRuleManager
    {
        FluentRule<T> AddRule(Func<T, IRuleResult> func, params string[] triggerProperty);

    }


    [PortalDataContract]
    public class RuleManager<T> : IRuleManager<T>, ISetTarget
    {

        protected T Target { get; set; }

        [PortalDataMember]
        protected IRuleResultList Results { get; private set; }

        IRuleResultReadOnlyList IRuleManager.Results => Results.RuleResultList;

        protected bool TransferredResults = false;
        public bool IsValid => !Results.Values.Where(r => r.IsError).Any();

        public RuleManager(IRegisteredPropertyManager<T> registeredPropertyManager)
        {
            Results = new RuleResultList();
            AddAttributeRules(new AttributeToRule(rp => new RequiredRule(rp)), registeredPropertyManager);
        }

        public RuleManager(IRuleResultList results, IAttributeToRule attributeToRule, IRegisteredPropertyManager<T> registeredPropertyManager)
        {
            Results = results;
            AddAttributeRules(attributeToRule, registeredPropertyManager);
        }

        IEnumerable<IRule> IRuleManager.Rules => Rules.Values;

        private IDictionary<uint, IRule> Rules { get; } = new ConcurrentDictionary<uint, IRule>();

        IRuleResult IRuleManager.this[string propertyName]
        {
            get { return Results[propertyName]; }
        }

        protected virtual void AddAttributeRules(IAttributeToRule attributeToRule, IRegisteredPropertyManager<T> registeredPropertyManager)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rule"></param>
        public void AddRule(IRule rule)
        {
            Rules.Add(rule.UniqueIndex, rule ?? throw new ArgumentNullException(nameof(rule)));
        }

        public FluentRule<T> AddRule(Func<T, IRuleResult> func, params string[] triggerProperty)
        {
            FluentRule<T> rule = new FluentRule<T>(func, triggerProperty); // TODO - DI
            Rules.Add(rule.UniqueIndex, rule);
            return rule;
        }

        public async Task CheckRulesForProperty(string propertyName)
        {

            if (OverrideResult == null)
            {
                if (TransferredResults)
                {
                    var oldResults = Results.Where(x => x.Key < 0 && x.Value.TriggerProperties.Contains(propertyName)).ToList();
                    oldResults.ForEach(r => Results.Remove(r.Key));

                    if (!Results.Where(x => x.Key < 0).Any())
                    {
                        TransferredResults = false;
                    }
                }

                foreach (var rule in Rules.Values.Where(r => r.TriggerProperties.Contains(propertyName)).ToList())
                {
                    // System.Diagnostics.Debug.WriteLine($"Enqueue {propertyName}");
                    await RunRule(rule, CancellationToken.None);
                }
            }
            else
            {
                await Task.CompletedTask;
            }
        }

        public async Task CheckAllRules(CancellationToken token = new CancellationToken())
        {

            if (OverrideResult == null)
            {
                var isValidAtStart = IsValid;

                Results.Clear(); // Cover in case something unexpected has happened like a weird Serialization cover or maybe a Rule that exists on the client or not the server

                foreach (var ruleIndex in Rules.ToList())
                {
                    await RunRule(ruleIndex.Value, token);
                }


                // TODO - This is lazy - we should be able to do this in the CheckRulesQueue
                if (isValidAtStart != IsValid)
                {
                    //if (Target is INotifiedOfPropertyChanged target)
                    //{
                    //    await target.HandlePropertyChange(nameof(IsValid), this);
                    //}
                }
            }
            else
            {
                await Task.CompletedTask;
            }
        }



        [PortalDataMember]
        public IRuleResult OverrideResult
        {
            get { return Results.OverrideResult; }
            set { Results.OverrideResult = value; }
        }

        public void MarkInvalid(string message)
        {
            Results.OverrideResult = RuleResult.PropertyError(typeof(T).FullName, message);
        }

        //public Task WaitForRules => allDoneCompletionSource?.Task ?? Task.CompletedTask;

        //private class MyTaskCompletionSource
        //{
        //    // Be clear which Func<Task, Task> is being used
        //    public MyTaskCompletionSource(Func<Task, Task> initialContinueWith)
        //    {
        //        TaskCompletionSource = new TaskCompletionSource<bool>();
        //        ContinueWith = initialContinueWith;
        //        TaskCompletionSource.Task.ContinueWith(t =>
        //        {
        //            return ContinueWith(t);
        //        });
        //    }

        //    private TaskCompletionSource<bool> TaskCompletionSource { get; set; }

        //    public Task Task => TaskCompletionSource.Task;

        //    public void SetException(Exception ex)
        //    {
        //        TaskCompletionSource.SetException(ex);
        //    }

        //    public void SetResult()
        //    {
        //        TaskCompletionSource.SetResult(true);
        //    }
        //        // TODO
        //    public CancellationTokenSource CancellationTokenSource { get; set; }
        //    public Func<Task, Task> ContinueWith { get; set; }
        //}

        //private MyTaskCompletionSource firstTaskCompletionSource;
        //private MyTaskCompletionSource lastTaskCompletionSource;
        //private TaskCompletionSource<bool> allDoneCompletionSource;
        //private CancellationTokenSource cancellationTokenSource;

        //private bool isRunningRules = false;
        //private object isRunningRulesLock = new object();


        //public Task CheckRulesQueue()
        //{

        //    // Goals
        //    // If all the rules are syncronous, run on the same task, when the property setting is finish isbusy=false

        //    // This method runs rule one at a time
        //    // If there is an async rule it waits for the async rule to complete
        //    // to run the next rule
        //    // If rules lead to more rules the queue will grow and the task
        //    // will not be completed until all rules are ran

        //    // This method does not return a Task
        //    // Since we know that rule will be ran in the context of a property change
        //    // that cannot be awaited
        //    // We leave it up to the caller to await WaitForRules

        //    if (Target == null) { throw new TargetIsNullException(); }

        //    // DISCUSS : Runes the rules sequentially - even Async Rules
        //    // Make async rules changing properties a non-issue

        //    lock (isRunningRulesLock)
        //    {
        //        if (OverrideResult != null)
        //        {
        //            return Task.CompletedTask;
        //        }

        //        if (isRunningRules)
        //        {

        //            var nextTaskCompletionSource = new MyTaskCompletionSource((t) =>
        //            {
        //                if (t.IsFaulted)
        //                {
        //                    allDoneCompletionSource.SetException(t.Exception);
        //                }
        //                else
        //                {
        //                    allDoneCompletionSource.SetResult(true);
        //                }

        //                return allDoneCompletionSource.Task;

        //            });


        //            lastTaskCompletionSource.ContinueWith = (t) =>
        //            {
        //                nextTaskCompletionSource.SetResult();
        //                return allDoneCompletionSource.Task;
        //            };

        //            lastTaskCompletionSource = nextTaskCompletionSource;

        //            return nextTaskCompletionSource.Task;
        //        }
        //        else
        //        {
        //            // Start

        //            isRunningRules = true;

        //            firstTaskCompletionSource = new MyTaskCompletionSource((t) =>
        //            {
        //                if (t.IsFaulted)
        //                {
        //                    allDoneCompletionSource.SetException(t.Exception);
        //                }
        //                else
        //                {
        //                    allDoneCompletionSource.SetResult(true);
        //                }

        //                return allDoneCompletionSource.Task;

        //            });

        //            lastTaskCompletionSource = firstTaskCompletionSource;
        //            allDoneCompletionSource = new TaskCompletionSource<bool>();

        //        }
        //    }

        //    async Task RecursiveCheckRulesQueue()
        //    {
        //        var token = CancellationToken.None;

        //        var guild = Guid.NewGuid();
        //        var ruleQueueCopy = ruleQueue.ToArray();

        //        while (ruleQueue.TryDequeue(out var ruleIndex))
        //        {
        //            // System.Diagnostics.Debug.WriteLine($"Dequeue {propertyName}");

        //            var rule = Rules[ruleIndex];

        //            var ruleManagerTask = RunRule(Rules[ruleIndex], token);

        //            if (!ruleManagerTask.IsCompleted)
        //            {

        //                // Really important
        //                // If there there is not an asyncronous fork all of the async methods will run synchronously
        //                // Which is great! Because we are likely within a property change
        //                // However, if there was an asyncronous fork we need to handle it's completion
        //                // WPF is an executable so this will continue "hands off"
        //                // In request response the WaitForRules needs to be awaited!
        //                await ruleManagerTask.ConfigureAwait(false);


        //                //return; // Let the ContinueWith call CheckRulesQueue again
        //            }
        //        }
        //    }

        //    var task = RecursiveCheckRulesQueue();

        //    void Completed()
        //    {
        //        lock (isRunningRulesLock)
        //        {
        //            isRunningRules = false;


        //            if (Results.Any(r => r.Value.Exception != null))
        //            {
        //                firstTaskCompletionSource.SetException(new AggregateException(Results.Where(r => r.Value.Exception != null).Select(r => r.Value.Exception)));
        //            }
        //            else
        //            {
        //                firstTaskCompletionSource.SetResult();
        //            }
        //        }
        //    }

        //    // No async for occured - keep the sync going
        //    if (task.IsCompleted)
        //    {
        //        Completed();

        //        Debug.Assert(!isRunningRules, "All rules should have executed in the loop");

        //        OnPropertyChanged(nameof(IsBusy));

        //        return Task.CompletedTask;
        //    }
        //    else
        //    {
        //        return task.ContinueWith(t =>
        //        {
        //            Completed();

        //            Debug.Assert(!isRunningRules, "All rules should have executed in the loop");

        //            OnPropertyChanged(nameof(IsBusy));
        //        });
        //    }
        //}

        private async Task RunRule(IRule r, CancellationToken token)
        {
            IRuleResult result = null;

            try
            {
                if (r is IRule<T> rule)
                {
                    result = await rule.Execute(Target, token);
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

            catch (Exception ex)
            {
                // If there is an error mark all properties as failed
                foreach (var p in r.TriggerProperties)
                {
                    result = RuleResult.PropertyError(p, ex.Message, ex);
                }

                throw;
            }
            finally
            {
                if (result.IsError)
                {
                    result.TriggerProperties = r.TriggerProperties;
                    Results[(int)r.UniqueIndex] = result;
                }
                else if (Results.ContainsKey((int)r.UniqueIndex))
                {
                    // Optimized approach for when/if this is serialized
                    Results.Remove((int)r.UniqueIndex);
                }
            }



        }


        //public event PropertyChangedEventHandler PropertyChanged;

        //protected virtual void OnPropertyChanged(string propertyName)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        void ISetTarget.SetTarget(IBase target)
        {
            if (target is T tt)
            {
                Target = tt;
            }
            else
            {
                throw new InvalidTargetTypeException(target.GetType().FullName);
            }
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            if (Results.Any())
            {
                Results.SetKeysToNegative();
                TransferredResults = true;
            }
        }

        private void SetSerializedResults(IRuleResultList transfferedResults, IRuleResult overrideResult)
        {
            if (transfferedResults.Any())
            {
                if (overrideResult == null)
                {
                    Results = transfferedResults;
                    Results.SetKeysToNegative();
                    TransferredResults = true;
                }
                else
                {
                    OverrideResult = overrideResult;
                }
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

