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
            
            if (!AsyncTaskSequencer.IsRunning)
            {
                var state = (IsValid, IsSelfValid, IsBusy, IsSelfBusy);

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
                };
            }

            if (PropertyValueManager.SetProperty(propertyName, value))
            {
                // TODO: Allow the consumer more control of PropertyValue<> and ValidatePropertyValue<>
                // Assume that if they're working directly with ValidatePropertyValue
                // That they're listening to ValidatePropertyValues PropertyChanged event
                // We don't want to raise the property changed event if the property is a IValidatePropertyValue
                //if (!typeof(P).IsAssignableTo(typeof(IValidatePropertyValue)))
                //{
                PropertyHasChanged(propertyName);
                //}

                var isRunning = AsyncTaskSequencer.IsRunning;

                // Sync Rules Only - Sync Properties Only
                // Sync Rules Only - Async Properties (IsBusy = true, WaitForRules works)
                // Async Rule - Sync Property Only (IsBusy = true, WaitForRules Works)
                // Async Rule - Async Property (IsBusy = true, WaitForRules works)
                AsyncTaskSequencer.AddTask(() => CheckRules(propertyName));

                if (!isRunning && AsyncTaskSequencer.IsRunning)
                {
                    PropertyHasChanged(nameof(IsSelfBusy));
                    PropertyHasChanged(nameof(IsBusy));
                }
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
        }

        protected virtual Task CheckRules(string propertyName)
        {
            return RuleManager.CheckRulesForProperty(propertyName);
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

        public Task CheckAllSelfRules(CancellationToken token = new CancellationToken())
        {
            return RuleManager.CheckAllRules();
        }

        public Task CheckAllRules(CancellationToken token = new CancellationToken())
        {
            // TODO - This isn't raising the 'IsValid' property changed event
            return Task.WhenAll(RuleManager.CheckAllRules(token), PropertyValueManager.CheckAllRules(token));
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