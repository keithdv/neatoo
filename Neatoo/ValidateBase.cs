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

    public interface IValidateBase : IBase, IValidateMetaProperties, INotifyPropertyChanged, INotifyNeatooPropertyChanged
    {

        IReadOnlyList<string> BrokenRuleMessages { get; }

        /// <summary>
        /// Stop events, rules and ismodified
        /// Only affects the Setter method
        /// Not SetProperty or LoadProperty
        /// </summary>
        bool IsStopped { get; }

        internal string ObjectInvalid { get; }

        internal Task AddSequencedTask(Func<Task, Task> task, bool runOnException = false);

        internal new IValidateProperty GetProperty(string propertyName);
        internal new IValidateProperty GetProperty(IRegisteredProperty registeredProperty);

        new internal IValidateProperty this[string propertyName] { get => GetProperty(propertyName); }
        new internal IValidateProperty this[IRegisteredProperty registeredProperty] { get => GetProperty(registeredProperty); }
    }

    [PortalDataContract]
    public abstract class ValidateBase<T> : Base<T>, IValidateBase, INotifyPropertyChanged, IPortalTarget
        where T : IValidateBase
    {

        [PortalDataMember]
        protected new IValidatePropertyManager PropertyManager => (IValidatePropertyManager)base.PropertyManager;

        [PortalDataMember]
        protected IRuleManager<T> RuleManager { get; }

        public ValidateBase(ValidateBaseServices<T> services) : base(services)
        {

            if (PropertyManager is IValidatePropertyManager)
            {
                PropertyManager.NeatooPropertyChanged += PropertyManagerNeatooPropertyChange;
                PropertyManager.PropertyChanged += PropertyManager_PropertyChanged;
            }

            this.RuleManager = services.CreateRuleManager((T)(IValidateBase)this);

            AsyncTaskSequencer.OnFullSequenceComplete = async (t) =>
            {
                await PropertyManager.WaitForTasks();
                RaiseMetaPropertiesChanged(true);
            };

            ResetMetaState();
        }



        public bool IsValid => PropertyManager.IsValid;

        public bool IsSelfValid => PropertyManager.IsSelfValid;

        public bool IsSelfBusy => !AsyncTaskSequencer.AllDone.IsCompleted;

        public bool IsBusy => IsSelfBusy || PropertyManager.IsBusy;

        protected (bool IsValid, bool IsSelfValid, bool IsBusy, bool IsSelfBusy) MetaState { get; private set; }

        protected virtual void RaiseMetaPropertiesChanged(bool raiseBusy = false)
        {
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

        protected override void PropertyManager_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.PropertyManager_PropertyChanged(sender, e);
            RaiseMetaPropertiesChanged();
        }

        protected new Task PropertyManagerNeatooPropertyChange(string propertyName, object source)
        {
            if (source == this.PropertyManager)
            {
                if (!IsStopped)
                {
                    RaisePropertyChanged(propertyName);
                    return CheckRules(propertyName);
                }

                // TODO - Support async PropertyChanged
                // AsyncTaskSequencer.AddTask((t) => RaiseNeatooPropertyChanged(propertyName, source) ?? Task.CompletedTask);
            }

            RaiseMetaPropertiesChanged();

            return Task.CompletedTask;
        }

        protected override void Child_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaiseMetaPropertiesChanged();
        }

        protected virtual Task AddAsyncMethod(Func<Task, Task> method, bool runOnException = false)
        {
            return AsyncTaskSequencer.AddTask(method, runOnException);
        }

        Task IValidateBase.AddSequencedTask(System.Func<Task, Task> task, bool runOnException = false)
        {
            return AddAsyncMethod(task, runOnException);
        }

        public virtual Task WaitForRules()
        {
            return Task.WhenAll([AsyncTaskSequencer.AllDone, PropertyManager.WaitForRules()]);
        }


        public IReadOnlyList<string> BrokenRuleMessages => PropertyManager.ErrorMessages;

        /// <summary>
        /// Permantatly mark invalid
        /// Running all rules will reset this
        /// </summary>
        /// <param name="message"></param>
        protected virtual void MarkInvalid(string message)
        {
            ObjectInvalid = message;
            this[nameof(ObjectInvalid)].SetErrorsForRule(0, new ReadOnlyCollection<string>(new List<string> { message }));
            RaiseMetaPropertiesChanged();
        }

        public string ObjectInvalid { get => Getter<string>(); protected set => Setter(value); }

        new protected IValidateProperty GetProperty(string propertyName)
        {
            return PropertyManager[propertyName];
        }

        new protected IValidateProperty GetProperty(IRegisteredProperty registeredProperty)
        {
            return PropertyManager[registeredProperty];
        }

        new protected IValidateProperty this[string propertyName] { get => GetProperty(propertyName); }
        new protected IValidateProperty this[IRegisteredProperty registeredProperty] { get => GetProperty(registeredProperty); }

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
            if (this[nameof(ObjectInvalid)].IsSelfValid)
            {
                var t = AddAsyncMethod((t) => RuleManager.CheckRulesForProperty(propertyName));

                if (!t.IsCompleted || t.IsFaulted)
                {
                    RaiseMetaPropertiesChanged();
                }
                return t;
            }
            return Task.CompletedTask;
        }


        public Task CheckAllSelfRules(CancellationToken token = new CancellationToken())
        {
            this[nameof(ObjectInvalid)].ClearAllErrors();

            AddAsyncMethod((t) => RuleManager.CheckAllRules(token));
            return AsyncTaskSequencer.AllDone;
        }

        public Task CheckAllRules(CancellationToken token = new CancellationToken())
        {
            this[nameof(ObjectInvalid)].ClearAllErrors();

            AddAsyncMethod((t) => RuleManager.CheckAllRules(token));
            AddAsyncMethod((t) => PropertyManager.CheckAllRules(token));
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

        IValidateProperty IValidateBase.GetProperty(string propertyName)
        {
            return GetProperty(propertyName);
        }

        IValidateProperty IValidateBase.GetProperty(IRegisteredProperty registeredProperty)
        {
            return GetProperty(registeredProperty);
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