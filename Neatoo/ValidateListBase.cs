﻿using Neatoo.Attributes;
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

    public abstract class ValidateListBase<I> : ListBase<I>, IValidateListBase<I>, IValidateListBase, INotifyPropertyChanged, IPortalTarget
        where I : IValidateBase
    {
        public ValidateListBase(ValidateListBaseServices<I> services) : base(services)
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

        protected override void Child_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaiseMetaPropertiesChanged();
            base.Child_PropertyChanged(sender, e);
        }

        protected virtual Task AddAsyncMethod(Func<Task, Task> method, bool runOnException = false)
        {
            return AsyncTaskSequencer.AddTask(method, runOnException);
        }

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
