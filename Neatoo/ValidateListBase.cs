using Neatoo.Core;
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

    public abstract class ValidateListBase<T, I> : ListBase<T, I>, IValidateListBase<I>, IValidateListBase, INotifyPropertyChanged, IRegisteredPropertyAccess
        where T : ValidateListBase<T, I>
        where I : IValidateBase
    {
        protected new IValidatePropertyValueManager<T> PropertyValueManager => (IValidatePropertyValueManager<T>)base.PropertyValueManager;

        protected IRuleManager<T> RuleManager { get; private set; }

        public ValidateListBase(IValidateListBaseServices<T, I> services) : base(services)
        {
            this.RuleManager = services.RuleManager;
            ((ISetTarget)this.RuleManager).SetTarget(this);
        }

        public bool IsValid => RuleManager.IsValid && PropertyValueManager.IsValid && !this.Any(c => !c.IsValid);
        public bool IsSelfValid => RuleManager.IsValid;
        public bool IsBusy => _asyncTaskSequencer.IsRunning ||  PropertyValueManager.IsBusy || this.Any(c => c.IsBusy);
        public bool IsSelfBusy => _asyncTaskSequencer.IsRunning || PropertyValueManager.IsBusy;

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


        private AsyncTaskSequencer _asyncTaskSequencer = new AsyncTaskSequencer();

        protected virtual void SetProperty<P>(string propertyName, P value)
        {

            if (!_asyncTaskSequencer.IsRunning)
            {
                var state = (IsValid, IsSelfValid, IsBusy, IsSelfBusy);

                _asyncTaskSequencer.OnCompleted = () =>
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

                var isRunning = _asyncTaskSequencer.IsRunning;

                // Sync Rules Only - Sync Properties Only
                // Sync Rules Only - Async Properties (IsBusy = true, WaitForRules works)
                // Async Rule - Sync Property Only (IsBusy = true, WaitForRules Works)
                // Async Rule - Async Property (IsBusy = true, WaitForRules works)
                _asyncTaskSequencer.AddTask(() => CheckRules(propertyName));

                if (!isRunning && _asyncTaskSequencer.IsRunning)
                {
                    PropertyHasChanged(nameof(IsSelfBusy));
                    PropertyHasChanged(nameof(IsBusy));
                }
            }
        }


        protected void PropertyHasChanged(string propertyName)
        {
            base.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected virtual async Task CheckRules(string propertyName)
        {
            await RuleManager.CheckRulesForProperty(propertyName);
        }

        public virtual Task WaitForRules()
        {
            return Task.WhenAll([PropertyValueManager.WaitForRules(), Task.WhenAll(this.Where(x => x.IsBusy).Select(x => x.WaitForRules()))]); // TODO: Rules.IsBusy
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


        public override IDisposable StopAllActions()
        {
            var result = base.StopAllActions();
            return result;
        }

        IRuleResultReadOnlyList IValidateBase.RuleResultList => RuleManager.Results;
        IEnumerable<string> IValidateBase.BrokenRuleMessages => RuleManager.Results.Where(x => x.IsError).SelectMany(x => x.PropertyErrorMessages).Select(x => x.Value);


        void IRegisteredPropertyAccess.SetProperty<P>(IRegisteredProperty registeredProperty, P value)
        {
            PropertyValueManager.SetProperty(registeredProperty, value);
        }

        public Task CheckAllSelfRules(CancellationToken token = new CancellationToken())
        {
            return RuleManager.CheckAllRules(token);
        }

        public Task CheckAllRules(CancellationToken token = new CancellationToken())
        {
            return Task.WhenAll(RuleManager.CheckAllRules(token), Task.WhenAll(this.Select(t => t.CheckAllRules(token))));
        }
    }
}
