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
        public bool IsBusy => AsyncTaskSequencer.IsRunning ||  PropertyValueManager.IsBusy || this.Any(c => c.IsBusy);
        public bool IsSelfBusy => AsyncTaskSequencer.IsRunning || PropertyValueManager.IsBusy;

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


        protected AsyncTaskSequencer AsyncTaskSequencer = new AsyncTaskSequencer();
        protected virtual void SetProperty<P>(string propertyName, P value)
        {
            if (PropertyValueManager.SetProperty(propertyName, value))
            {
                PropertyHasChanged(propertyName);
                AddAsyncMethod(() => RuleManager.CheckRulesForProperty(propertyName));
            }
        }

        protected virtual void PropertyHasChanged(string propertyName, object source = null)
        {
            base.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
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

        public virtual Task CheckRules(string propertyName)
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

    }
}
