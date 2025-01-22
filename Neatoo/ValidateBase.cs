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
    }

    [PortalDataContract]
    public abstract class ValidateBase<T> : Base<T>, IValidateBase, INotifyPropertyChanged, IRegisteredPropertyAccess
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
        }

        public bool IsValid => RuleManager.IsValid && PropertyValueManager.IsValid;

        public bool IsSelfValid => RuleManager.IsValid;

        public bool IsSelfBusy => !AsyncTaskSequencer.AllDone.IsCompleted;

        public bool IsBusy => IsSelfBusy || PropertyValueManager.IsBusy;

        protected override void Setter<P>(P value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            if (!IsStopped)
            {
                SetProperty(propertyName, value);
            }
            else
            {
                LoadProperty(GetRegisteredProperty(propertyName), value);
            }
        }

        // TODO: Inject
        protected AsyncTaskSequencer AsyncTaskSequencer { get; set; } = new AsyncTaskSequencer();

        protected virtual void SetProperty<P>(string propertyName, P value)
        {
            if (PropertyValueManager.SetProperty(propertyName, value))
            {
                PropertyHasChanged(propertyName);
                AddAsyncMethod(() => RuleManager.CheckRulesForProperty(propertyName));
            }
        }

        void IRegisteredPropertyAccess.SetProperty<P>(IRegisteredProperty registeredProperty, P value)
        {
            PropertyValueManager.SetProperty(registeredProperty, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;


        protected virtual void PropertyHasChanged(string propertyName, object source = null)
        {
            PropertyChanged?.Invoke(source ?? this, new PropertyChangedEventArgs(propertyName));
            Parent?.ChildPropertyChanged(propertyName, this);
        }

        protected override void ChildPropertyChanged(string propertyName, IBase source)
        {
            if (new string[] { nameof(IsValid), nameof(IsBusy) }.Contains(propertyName))
            {
                PropertyHasChanged(propertyName, this);
            }
        }

        protected virtual void AddAsyncMethod(Func<Task> method)
        {
            if (!AsyncTaskSequencer.IsRunning)
            {
                var state = (IsValid, IsSelfValid, IsBusy, IsSelfBusy);

                var curOnCompleted = AsyncTaskSequencer.OnCompleted;

                AsyncTaskSequencer.OnCompleted = () =>
                {
                    if (state.IsValid != IsValid)
                    {
                        PropertyHasChanged(nameof(IsValid));
                    }
                    if (state.IsSelfValid != IsSelfValid)
                    {
                        PropertyHasChanged(nameof(IsSelfValid));
                    }
                    if (state.IsSelfBusy)
                    {
                        PropertyHasChanged(nameof(IsSelfBusy));
                    }
                    if (state.IsBusy)
                    {
                        PropertyHasChanged(nameof(IsBusy));
                    }

                    curOnCompleted?.Invoke();
                };
            }

            var isRunning = AsyncTaskSequencer.IsRunning;

            AsyncTaskSequencer.AddTask(method);

            if (!isRunning && AsyncTaskSequencer.IsRunning)
            {
                PropertyHasChanged(nameof(IsSelfBusy));
                PropertyHasChanged(nameof(IsBusy));
            }
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
        }

        public override IDisposable StopAllActions()
        {
            var result = base.StopAllActions();
            return result;
        }

        public Task CheckRules(string propertyName)
        {
            AddAsyncMethod(() => RuleManager.CheckRulesForProperty(propertyName));
            return AsyncTaskSequencer.AllDone;
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