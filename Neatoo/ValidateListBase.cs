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

            AsyncTaskSequencer.OnEachComplete.Add(RaiseMetaPropertiesChanged);

            ResetMetaState();
        }

        public bool IsValid => RuleManager.IsValid && PropertyValueManager.IsValid && !this.Any(c => !c.IsValid);
        public bool IsSelfValid => RuleManager.IsValid;
        public bool IsBusy => AsyncTaskSequencer.IsRunning ||  PropertyValueManager.IsBusy || this.Any(c => c.IsBusy);
        public bool IsSelfBusy => AsyncTaskSequencer.IsRunning || PropertyValueManager.IsBusy;

        protected (bool IsValid, bool IsSelfValid, bool IsBusy, bool IsSelfBusy) MetaState { get; private set; }

        protected virtual void RaiseMetaPropertiesChanged()
        {
            if (MetaState.IsValid != IsValid)
            {
                PropertyHasChanged(nameof(IsValid));
            }
            if (MetaState.IsSelfValid != IsSelfValid)
            {
                PropertyHasChanged(nameof(IsSelfValid));
            }
            if (MetaState.IsSelfBusy)
            {
                PropertyHasChanged(nameof(IsSelfBusy));
            }
            if (MetaState.IsBusy)
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

        
        protected override void HandlePropertyChanged(string propertyName, IBase source)
        {
            if (source == this && !IsStopped && PropertyValueManager.HasProperty(propertyName))
            {
                CheckRules(propertyName);
                PropertyHasChanged(propertyName);
            }

            RaiseMetaPropertiesChanged();
        }


        protected virtual void PropertyHasChanged(string propertyName, object source = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
            Parent?.HandlePropertyChanged(propertyName, this);
        }

        protected virtual void AddAsyncMethod(Func<Task> method)
        {
            AsyncTaskSequencer.AddTask(method);

            RaiseMetaPropertiesChanged();
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
            AddAsyncMethod(() => RuleManager.CheckRulesForProperty(propertyName));
            return AsyncTaskSequencer.AllDone;
        }

        public virtual Task CheckAllSelfRules(CancellationToken token = new CancellationToken())
        {
            AddAsyncMethod(() => RuleManager.CheckAllRules());
            return AsyncTaskSequencer.AllDone;
        }

        public virtual Task CheckAllRules(CancellationToken token = new CancellationToken())
        {
            AddAsyncMethod(() => RuleManager.CheckAllRules(token));
            AddAsyncMethod(() => PropertyValueManager.CheckAllRules(token));
            foreach (var item in this)
            {
                AddAsyncMethod(() => item.CheckAllRules(token));
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
        IEnumerable<string> IValidateBase.BrokenRuleMessages => RuleManager.Results.Where(x => x.IsError).SelectMany(x => x.PropertyErrorMessages).Select(x => x.Value);

    }
}
