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
    public abstract class ValidateBase<T> : Base<T>, IValidateBase, INotifyPropertyChanged,  IRegisteredPropertyAccess
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
            this.RuleManager.PropertyChanged += OnRuleManagerPropertyChanged;
        }

        public bool IsValid => RuleManager.IsValid && PropertyValueManager.IsValid;

        public bool IsSelfValid => RuleManager.IsValid;

        public bool IsSelfBusy => RuleManager.IsBusy;

        public bool IsBusy => RuleManager.IsBusy || PropertyValueManager.IsBusy;

        protected override void Setter<P>(P value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            if (!IsStopped)
            {
                SetProperty(propertyName, value);
            }
            else
            {
                LoadProperty(GetRegisteredProperty<P>(propertyName), value);
            }
        }

        protected virtual async Task SetProperty<P>(string propertyName, P value)
        {
            if(await PropertyValueManager.SetProperty(GetRegisteredProperty<P>(propertyName), value))
            {
                // TODO: Allow the consumer more control of PropertyValue<> and ValidatePropertyValue<>
                // Assume that if they're working directly with ValidatePropertyValue
                // That they're listening to ValidatePropertyValues PropertyChanged event
                // We don't want to raise the property changed event if the property is a IValidatePropertyValue
                //if (!typeof(P).IsAssignableTo(typeof(IValidatePropertyValue)))
                //{
                PropertyHasChanged(propertyName);
                //}

                // Sync Rules Only - Sync Properties Only
                // Sync Rules Only - Async Properties (IsBusy = true, WaitForRules works)
                // Async Rule - Sync Property Only (IsBusy = true, WaitForRules Works)
                // Async Rule - Async Property (IsBusy = true, WaitForRules works)
                await CheckRules(propertyName);
            }


            //if (await PropertyValueManager.SetProperty(GetRegisteredProperty<P>(propertyName), value))
            //{

            //    // Make sure if there's a UI thread the task is continued on the UI thread
            //    // What's happening is multiple threads are awaiting the RulesManager.waitForRulesSource.Task
            //    // So when that's completed multiple threads take off and raise the PropertyChanged event

            //    // To see, remove it and play with Cart.NumberOfHorses in the Horse and Cart example
            //    // Parrallel exection of property changed event causes havoc

            //    // See https://blog.stephencleary.com/2023/11/configureawait-in-net-8.html
            //    // Since I'm not awaiting the task, I need to specify ConfigureAwait(true) to ensure the continuation is on the UI thread

            //    // This also means that it's possible to get a different behavior in a unit test
            //    // If you do, please let me (Keith Voels) know!!
            //    var task = CheckRules(propertyName);

            //        task
            //        .ConfigureAwait(true).GetAwaiter()
            //        .OnCompleted(async () =>
            //        {
            //            if (task.IsFaulted)
            //            {
            //                MarkInvalid(task.Exception.Message);
            //            }
            //            Debug.Assert(!IsBusy, "All rules should have executed by the time we raise any async forked property change events");
            //            await PropertyHasChanged(propertyName);
            //        });
            //}
        }

        void IRegisteredPropertyAccess.SetProperty<P>(IRegisteredProperty<P> registeredProperty, P value)
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
            return Task.WhenAll([RuleManager.WaitForRules, PropertyValueManager.WaitForRules()]);
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

        public override async Task<IDisposable> StopAllActions()
        {
            var result = await base.StopAllActions();
            await WaitForRules();
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


        public void OnRuleManagerPropertyChanged(object sender, PropertyChangedEventArgs eventArgs)
        {
            PropertyHasChanged(nameof(IsSelfValid));
            PropertyHasChanged(nameof(IsValid));
            PropertyHasChanged(nameof(IsSelfBusy));
            PropertyHasChanged(nameof(IsBusy));
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