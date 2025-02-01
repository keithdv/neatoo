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
    public interface IValidateListBase : IListBase
    {

    }

    public interface IValidateListBase<I> : IListBase<I>, IValidateListBase, IValidateMetaProperties
        where I : IValidateBase
    {

    }

    public abstract class ValidateListBase<T, I> : ListBase<T, I>, IValidateListBase<I>, IValidateListBase, INotifyPropertyChanged
        where T : ValidateListBase<T, I>
        where I : IValidateBase
    {
        public ValidateListBase(IValidateListBaseServices<T, I> services) : base(services)
        {

            AsyncTaskSequencer.OnFullSequenceComplete = (t) =>
            {
                RaiseMetaPropertiesChanged(true);
                return Task.CompletedTask;
            };

            ResetMetaState();
        }

        public bool IsValid => !this.Any(c => !c.IsValid);
        public bool IsSelfValid => true;
        public bool IsBusy => AsyncTaskSequencer.IsRunning || this.Any(c => c.IsBusy);
        public bool IsSelfBusy => AsyncTaskSequencer.IsRunning;

        protected (bool IsValid, bool IsSelfValid, bool IsBusy, bool IsSelfBusy) MetaState { get; private set; }

        protected virtual void RaiseMetaPropertiesChanged(bool raiseBusy = false)
        {
            if (MetaState.IsValid != IsValid)
            {
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsValid)));
            }
            if (MetaState.IsSelfValid != IsSelfValid)
            {
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsSelfValid)));
            }
            if (raiseBusy && IsSelfBusy || MetaState.IsSelfBusy != IsSelfBusy)
            {
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsSelfBusy)));
            }
            if (raiseBusy && IsBusy || MetaState.IsBusy != IsBusy)
            {
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsBusy)));
            }

            ResetMetaState();
        }

        protected virtual void ResetMetaState()
        {
            MetaState = (IsValid, IsSelfValid, IsBusy, IsSelfBusy);
        }

        // TODO: Inject
        protected AsyncTaskSequencer AsyncTaskSequencer { get; set; } = new AsyncTaskSequencer();

        protected override void ChildPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaiseMetaPropertiesChanged();
            base.ChildPropertyChanged(sender, e);
        }

        protected virtual Task AddAsyncMethod(Func<Task, Task> method, bool runOnException = false)
        {
            return AsyncTaskSequencer.AddTask(method, runOnException);
        }

        public virtual Task WaitForRules()
        {
            return Task.WhenAll(this.Where(x => x.IsBusy).Select(x => x.WaitForRules()));
        }

        public Task CheckAllRules(CancellationToken token = default)
        {
            return Task.WhenAll(this.Select(x => x.CheckAllRules(token)));
        }

        public Task CheckAllSelfRules(CancellationToken token = default)
        {
            return Task.CompletedTask;
        }
    }
}
