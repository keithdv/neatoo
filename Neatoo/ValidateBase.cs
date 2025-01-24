using Neatoo.Attributes;
using Neatoo.Core;
using Neatoo.Portal;
using Neatoo.Rules;
using Neatoo.Rules.Rules;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Neatoo
{

    public interface IValidateBase : IBase, IValidateMetaProperties, INotifyPropertyChanged
    {
        Task WaitForRules();
        Task CheckAllRules(CancellationToken token = new CancellationToken());
        Task CheckAllSelfRules(CancellationToken token = new CancellationToken());
        IRuleResultReadOnlyList RuleResultList { get; }
        IEnumerable<string> BrokenRuleMessages { get; }

        /// <summary>
        /// Stop events, rules and ismodified
        /// Only affects the Setter method
        /// Not SetProperty or LoadProperty
        /// </summary>
        bool IsStopped { get; }
    }

    [PortalDataContract]
    public abstract class ValidateBase<T> : Base<T>, IValidateBase, INotifyPropertyChanged, IPortalTarget
        where T : ValidateBase<T>
    {

        [PortalDataMember]
        protected new IValidatePropertyValueManager<T> PropertyValueManager => (IValidatePropertyValueManager<T>)base.PropertyValueManager;

        [PortalDataMember]
        protected IRuleManager<T> RuleManager { get; }

        public ValidateBase(IValidateBaseServices<T> services) : base(services)
        {
            this.RuleManager = services.RuleManager;
            ((ISetTarget)this.RuleManager).SetTarget(this);
            AsyncTaskSequencer.OnEachComplete.Add(RaiseMetaPropertiesChanged);
            ResetMetaState();
        }

        public bool IsValid => RuleManager.IsValid && PropertyValueManager.IsValid;

        public bool IsSelfValid => RuleManager.IsValid;

        public bool IsSelfBusy => !AsyncTaskSequencer.AllDone.IsCompleted;

        public bool IsBusy => IsSelfBusy || PropertyValueManager.IsBusy;

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


        protected override async Task HandlePropertyChanged(string propertyName, IBase source)
        {

            if (source == this && !IsStopped && PropertyValueManager.HasProperty(propertyName))
            {
                await CheckRules(propertyName);
                PropertyHasChanged(propertyName);
            }

            RaiseMetaPropertiesChanged();
        }

        public event PropertyChangedEventHandler PropertyChanged;


        protected virtual void PropertyHasChanged(string propertyName, object source = null)
        {
            PropertyChanged?.Invoke(source ?? this, new PropertyChangedEventArgs(propertyName));
            Parent?.HandlePropertyChanged(propertyName, this);
        }

        protected virtual Task AddAsyncMethod(Func<Task> method)
        {
            return AsyncTaskSequencer.AddTask(method);
        }

        public virtual Task WaitForRules()
        {
            return Task.WhenAll([AsyncTaskSequencer.AllDone, PropertyValueManager.WaitForRules()]);
        }

        public IRuleResultReadOnlyList RuleResultList => RuleManager.Results;

        public IEnumerable<string> BrokenRuleMessages => RuleManager.Results.Where(x => x.IsError).SelectMany(x => x.PropertyErrorMessages).Select(x => x.Value);

        /// <summary>
        /// Permantatly mark invalid
        /// Note: not associated with any specific property
        /// </summary>
        /// <param name="message"></param>
        protected virtual void MarkInvalid(string message)
        {
            RuleManager.MarkInvalid(message);
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
            return AddAsyncMethod(() => RuleManager.CheckRulesForProperty(propertyName));
        }

        public Task CheckAllSelfRules(CancellationToken token = new CancellationToken())
        {
            AddAsyncMethod(() => RuleManager.CheckAllRules());
            return AsyncTaskSequencer.AllDone;
        }

        public Task CheckAllRules(CancellationToken token = new CancellationToken())
        {
            AddAsyncMethod(() => RuleManager.CheckAllRules(token));
            AddAsyncMethod(() => PropertyValueManager.CheckAllRules(token));
            // TODO - This isn't raising the 'IsValid' property changed event
            return AsyncTaskSequencer.AllDone;
        }

        [Create]
        protected async Task Create()
        {
            await CheckAllSelfRules();
        }

        [CreateChild]
        protected async Task CreateChild()
        {
            await CheckAllSelfRules();
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