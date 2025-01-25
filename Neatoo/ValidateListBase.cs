using Neatoo.Core;
using Neatoo.Portal;
using Neatoo.Rules;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Neatoo
{
    public interface IValidateListBase : IListBase, IValidateBase, IValidateMetaProperties
    {

    }

    public interface IValidateListBase<I> : IListBase<I>, IValidateBase, IValidateMetaProperties
        where I : IValidateBase
    {

    }

    public abstract class ValidateListBase<T, I> : ListBase<T, I>, IValidateListBase<I>, IValidateListBase, INotifyPropertyChanged, IPortalTarget
        where T : ValidateListBase<T, I>
        where I : IValidateBase
    {
        protected new IValidatePropertyValueManager<T> PropertyValueManager => (IValidatePropertyValueManager<T>)base.PropertyValueManager;

        protected IRuleManager<T> RuleManager { get; private set; }

        public ValidateListBase(IValidateListBaseServices<T, I> services) : base(services)
        {
            this.RuleManager = services.RuleManager;
            ((ISetTarget)this.RuleManager).SetTarget(this);

            AsyncTaskSequencer.OnFullSequenceComplete.Add(() => RaiseMetaPropertiesChanged(true));

            ResetMetaState();
        }

        public bool IsValid => RuleManager.IsValid && PropertyValueManager.IsValid && !this.Any(c => !c.IsValid);
        public bool IsSelfValid => RuleManager.IsValid;
        public bool IsBusy => AsyncTaskSequencer.IsRunning ||  PropertyValueManager.IsBusy || this.Any(c => c.IsBusy);
        public bool IsSelfBusy => AsyncTaskSequencer.IsRunning || PropertyValueManager.IsBusy;

        protected (bool IsValid, bool IsSelfValid, bool IsBusy, bool IsSelfBusy) MetaState { get; private set; }

        protected virtual void RaiseMetaPropertiesChanged(bool raiseBusy = false)
        {
            if (MetaState.IsValid != IsValid)
            {
                PropertyHasChanged(nameof(IsValid));
            }
            if (MetaState.IsSelfValid != IsSelfValid)
            {
                PropertyHasChanged(nameof(IsSelfValid));
            }
            if (raiseBusy && IsSelfBusy || MetaState.IsSelfBusy != IsSelfBusy)
            {
                PropertyHasChanged(nameof(IsSelfBusy));
            }
            if (raiseBusy && IsBusy || MetaState.IsBusy != IsBusy)
            {
                PropertyHasChanged(nameof(IsBusy));
            }

            ResetMetaState();
        }

        protected virtual void ResetMetaState()
        {
            MetaState = (IsValid, IsSelfValid, IsBusy, IsSelfBusy);
        }

        // TODO: Inject
        protected AsyncTaskSequencer AsyncTaskSequencer { get; set; } = new AsyncTaskSequencer();

        protected override Task HandlePropertyChanged(string propertyName, IBase source)
        {
            if (source == this && !IsStopped && PropertyValueManager.HasProperty(propertyName))
            {
                PropertyHasChanged(propertyName);

                var t = CheckRules(propertyName);

                if (!t.IsCompleted || t.IsFaulted)
                {
                    RaiseMetaPropertiesChanged();
                }

                return t;
            }

            RaiseMetaPropertiesChanged();

            return Task.CompletedTask;
        }

        protected virtual Task AddAsyncMethod(Func<Task, Task> method, bool runOnException = false)
        {
            return AsyncTaskSequencer.AddTask(method, runOnException);
        }

        Task IValidateBase.AddSequencedTask(System.Func<Task, Task> task, bool runOnException = false)
        {
            return AddAsyncMethod(task, runOnException);
        }

        protected virtual void PropertyHasChanged(string propertyName, object source = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
            Parent?.HandlePropertyChanged(propertyName, this);
        }



        new protected IValidatePropertyValue GetProperty(string propertyName)
        {
            return PropertyValueManager[propertyName];
        }

        new protected IValidatePropertyValue GetProperty(IRegisteredProperty registeredProperty)
        {
            return PropertyValueManager[registeredProperty];
        }

        new protected IValidatePropertyValue this[string propertyName] { get => GetProperty(propertyName); }
        new protected IValidatePropertyValue this[IRegisteredProperty registeredProperty] { get => GetProperty(registeredProperty); }

        public bool IsStopped { get; protected set; }

        public virtual IDisposable StopAllActions()
        {
            if (IsStopped) { return null; } // You are a nested using; You get nothing!!
            IsStopped = true;
            return new Core.Stopped(this);
        }

        public void StartAllActions()
        {
            if (IsStopped)
            {
                IsStopped = false;
                ResetMetaState();
            }
        }

        IDisposable IPortalTarget.StopAllActions()
        {
            return StopAllActions();
        }

        void IPortalTarget.StartAllActions()
        {
            StartAllActions();
        }

        public Task CheckRules(string propertyName)
        {
            return AddAsyncMethod((t) => RuleManager.CheckRulesForProperty(propertyName));
        }

        public virtual Task CheckAllSelfRules(CancellationToken token = new CancellationToken())
        {
            AddAsyncMethod((t) => RuleManager.CheckAllRules());
            return AsyncTaskSequencer.AllDone;
        }

        public virtual Task CheckAllRules(CancellationToken token = new CancellationToken())
        {
            AddAsyncMethod((t) => RuleManager.CheckAllRules(token));
            AddAsyncMethod((t) => PropertyValueManager.CheckAllRules(token));
            foreach (var item in this)
            {
                AddAsyncMethod((t) => item.CheckAllRules(token));
            }
            // TODO - This isn't raising the 'IsValid' property changed event
            return AsyncTaskSequencer.AllDone;
        }

        public virtual Task WaitForRules()
        {
            return Task.WhenAll([PropertyValueManager.WaitForRules(), Task.WhenAll(this.Where(x => x.IsBusy).Select(x => x.WaitForRules()))]); // TODO: Rules.IsBusy
        }

        protected override void InsertItem(int index, I item)
        {
            ((ISetParent)item).SetParent(this);

            base.InsertItem(index, item);

            PropertyHasChanged(nameof(Count));
        }

        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);

            PropertyHasChanged(nameof(Count));
        }

        /// <summary>
        /// Permantatly mark invalid
        /// Note: not associated with any specific property
        /// </summary>
        /// <param name="message"></param>
        protected virtual void MarkInvalid(string message)
        {
            RuleManager.MarkInvalid(message);
        }


        IRuleResultReadOnlyList IValidateBase.RuleResultList => RuleManager.Results;
        public IReadOnlyList<string> BrokenRuleMessages => RuleManager.Results.Where(x => x.IsError).SelectMany(x => x.PropertyErrorMessages).Select(x => x.Value).ToList().AsReadOnly();

        IValidatePropertyValue IValidateBase.GetProperty(string propertyName)
        {
            return GetProperty(propertyName);
        }

        IValidatePropertyValue IValidateBase.GetProperty(IRegisteredProperty registeredProperty)
        {
            return GetProperty(registeredProperty);
        }

    }
}
