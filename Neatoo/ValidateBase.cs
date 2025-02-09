using Neatoo.Core;
using Neatoo.Portal;
using Neatoo.Rules;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Neatoo
{

    public interface IValidateBase : IBase, IValidateMetaProperties, INotifyPropertyChanged, INotifyNeatooPropertyChanged
    {
        IReadOnlyList<string> BrokenRuleMessages { get; }

        /// <summary>
        /// Pause events, rules and ismodified
        /// Only affects the Setter method
        /// Not SetProperty or LoadProperty
        /// </summary>
        bool IsPaused { get; }

        internal string ObjectInvalid { get; }

        internal new IValidateProperty GetProperty(string propertyName);

        new internal IValidateProperty this[string propertyName] { get => GetProperty(propertyName); }
    }

    public abstract class ValidateBase<T> : Base<T>, IValidateBase, INotifyPropertyChanged, IPortalTarget
        where T : ValidateBase<T>
    {
        protected new IValidatePropertyManager<IValidateProperty> PropertyManager => (IValidatePropertyManager<IValidateProperty>)base.PropertyManager;

        protected IRuleManager<T> RuleManager { get; }

        public ValidateBase(IValidateBaseServices<T> services) : base(services)
        {
            this.RuleManager = services.CreateRuleManager((T)(IValidateBase)this);



            ResetMetaState();
        }

        public bool IsValid => PropertyManager.IsValid;

        public bool IsSelfValid => PropertyManager.IsSelfValid;

        protected (bool IsValid, bool IsSelfValid, bool IsBusy, bool IsSelfBusy) MetaState { get; private set; }

        protected override void CheckIfMetaPropertiesChanged(bool raiseBusy = false)
        {
            base.CheckIfMetaPropertiesChanged();

            if (MetaState.IsValid != IsValid)
            {
                RaisePropertyChanged(nameof(IsValid));
            }
            if (MetaState.IsSelfValid != IsSelfValid)
            {
                RaisePropertyChanged(nameof(IsSelfValid));
            }
            if (raiseBusy && IsSelfBusy || MetaState.IsSelfBusy != IsSelfBusy)
            {
                RaisePropertyChanged(nameof(IsSelfBusy));
            }
            if (raiseBusy && IsBusy || MetaState.IsBusy != IsBusy)
            {
                RaisePropertyChanged(nameof(IsBusy));
            }

            ResetMetaState();
        }

        protected virtual void ResetMetaState()
        {
            MetaState = (IsValid, IsSelfValid, IsBusy, IsSelfBusy);
        }

        protected override async Task HandleNeatooPropertyChanged(PropertyChangedBreadCrumbs breadCrumbs)
        {
            await base.HandleNeatooPropertyChanged(breadCrumbs);

            if (!IsPaused)
            {
                await CheckRules(breadCrumbs.FullPropertyName);
            }

            CheckIfMetaPropertiesChanged();
        }

        protected virtual Task AddAsyncMethod(Func<Task, Task> method, bool runOnException = false)
        {
            return AsyncTaskSequencer.AddTask(method, runOnException);
        }

        public IReadOnlyList<string> BrokenRuleMessages => PropertyManager.ErrorMessages;

        /// <summary>
        /// Permanently mark invalid
        /// Running all rules will reset this
        /// </summary>
        /// <param name="message"></param>
        protected virtual void MarkInvalid(string message)
        {
            ObjectInvalid = message;
            this[nameof(ObjectInvalid)].SetErrorsForRule(0, new ReadOnlyCollection<string>(new List<string> { message }));
            CheckIfMetaPropertiesChanged();
        }

        public string ObjectInvalid { get => Getter<string>(); protected set => Setter(value); }

        new protected IValidateProperty GetProperty(string propertyName)
        {
            return PropertyManager[propertyName];
        }

        new protected IValidateProperty this[string propertyName] { get => GetProperty(propertyName); }

        public bool IsPaused { get; protected set; }

        public virtual IDisposable PauseAllActions()
        {
            if (IsPaused) { return null; } // You are a nested using; You get nothing!!
            IsPaused = true;
            return new Core.Paused(this);
        }

        public virtual void ResumeAllActions()
        {
            if (IsPaused)
            {
                IsPaused = false;
                ResetMetaState();
            }
        }

        IDisposable IPortalTarget.PauseAllActions()
        {
            return PauseAllActions();
        }

        void IPortalTarget.ResumeAllActions()
        {
            ResumeAllActions();
        }

        public virtual Task PostPortalConstruct()
        {
            return Task.CompletedTask;
        }

        public Task CheckRules(string propertyName)
        {
            if (this[nameof(ObjectInvalid)].IsSelfValid)
            {
                var task = RuleManager.CheckRulesForProperty(propertyName);

                CheckIfMetaPropertiesChanged();

                return task;
            }

            return Task.CompletedTask;
        }

        public async Task RunSelfRules(CancellationToken token = new CancellationToken())
        {
            this[nameof(ObjectInvalid)].ClearAllErrors();

            await AddAsyncMethod((t) => RuleManager.CheckAllRules(token));
            await AsyncTaskSequencer.AllDone;
        }

        public async Task RunAllRules(CancellationToken token = new CancellationToken())
        {
            ClearAllErrors();

            await PropertyManager.RunAllRules(token);
            await RuleManager.CheckAllRules(token);
            await AsyncTaskSequencer.AllDone;

            //this.AddAsyncMethod((t) => PropertyManager.RunAllRules(token));
            // TODO - This isn't raising the 'IsValid' property changed event
            //await base.WaitForTasks();
            Debug.Assert(!IsBusy, "Should not be busy after running all rules");
        }

        public virtual void ClearSelfErrors()
        {
            this[nameof(ObjectInvalid)].ClearAllErrors();
            PropertyManager.ClearSelfErrors();
        }

        public virtual void ClearAllErrors()
        {
            this[nameof(ObjectInvalid)].ClearAllErrors();
            PropertyManager.ClearAllErrors();
        }

        [Create]
        protected async Task Create()
        {
            await RunSelfRules();
        }

        [CreateChild]
        protected async Task CreateChild()
        {
            await RunSelfRules();
        }

        IValidateProperty IValidateBase.GetProperty(string propertyName)
        {
            return GetProperty(propertyName);
        }

    }
}


[Serializable]
public class AddRulesNotDefinedException<T> : Exception
{
    public AddRulesNotDefinedException() : base($"AddRules not defined for {typeof(T).Name}") { }
    public AddRulesNotDefinedException(string message) : base(message) { }
    public AddRulesNotDefinedException(string message, Exception inner) : base(message, inner) { }
}