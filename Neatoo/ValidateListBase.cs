using Neatoo.Core;
using System.ComponentModel;
using System.Linq;
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
            ResetMetaState();
        }

        public bool IsValid => !this.Any(c => !c.IsValid);
        public bool IsSelfValid => true;

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

        protected override Task OnChildNeatooPropertyChanged(PropertyNameBreadCrumbs breadCrumbs)
        {
            RaiseMetaPropertiesChanged();
            return base.OnChildNeatooPropertyChanged(breadCrumbs);
        }

        public async Task RunAllRules(CancellationToken token = default)
        {
            foreach (var item in this)
            {
                await item.RunAllRules();
            }
        }

        public Task RunSelfRules(CancellationToken token = default)
        {
            return Task.CompletedTask;
        }

        public void ClearAllErrors()
        {
            foreach (var item in this)
            {
                item.ClearAllErrors();
            }
        }

        public void ClearSelfErrors()
        {
            foreach (var item in this)
            {
                item.ClearSelfErrors();
            }
        }
    }
}
