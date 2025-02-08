using Neatoo.Core;
using Neatoo.Portal;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Neatoo
{

    public interface IReadOnlyListBase<I> : INeatooObject, INotifyCollectionChanged, INotifyPropertyChanged, IReadOnlyCollection<I>, IReadOnlyList<I>
        where I : IBase
    {

    }


    public interface IListBase : INeatooObject, INotifyCollectionChanged, INotifyPropertyChanged, IEnumerable, ICollection, IList, INotifyNeatooPropertyChanged, IBaseMetaProperties
    {
        IBase Parent { get; }
    }

    public interface IListBase<I> : IListBase, IReadOnlyListBase<I>, IEnumerable<I>, ICollection<I>, IList<I>
        where I : IBase
    {
        Task<I> CreateAdd();
        Task<I> CreateAdd(params object[] criteria);

        new int Count { get; }
    }


    public abstract class ListBase<T, I> : ObservableCollection<I>, INeatooObject, IListBase<I>, IListBase, IReadOnlyListBase<I>, ISetParent, IJsonOnDeserialized, IJsonOnDeserializing, IBaseMetaProperties
        where T : ListBase<T, I>
        where I : IBase
    {
        protected IReadPortalChild<I> ItemPortal { get; }

        public ListBase(IListBaseServices<T, I> services)
        {
            ItemPortal = services.ReadPortal;
        }

        public IBase Parent { get; protected set; }

        public bool IsBusy => this.Any(c => c.IsBusy);
        public bool IsSelfBusy => false;
        public event NeatooPropertyChanged NeatooPropertyChanged;

        #region "Match Base"
        void ISetParent.SetParent(IBase parent)
        {
            Parent = parent;

            foreach (var item in this)
            {
                if (item is ISetParent setParent)
                {
                    setParent.SetParent(parent);
                }
            }
        }


        #endregion


        protected override void InsertItem(int index, I item)
        {
            ((ISetParent)item).SetParent(this.Parent);

            base.InsertItem(index, item);

            item.NeatooPropertyChanged += _ChildNeatooPropertyChanged;

            RaiseNeatooPropertyChanged(new PropertyNameBreadCrumbs(nameof(Count), this));
        }

        protected override void RemoveItem(int index)
        {
            this[index].NeatooPropertyChanged -= _ChildNeatooPropertyChanged;

            base.RemoveItem(index);

            RaiseNeatooPropertyChanged(new PropertyNameBreadCrumbs(nameof(Count), this));
        }

        public async Task<I> CreateAdd()
        {
            var item = await ItemPortal.CreateChild();
            Add(item);
            return item;
        }

        public async Task<I> CreateAdd(params object[] criteria)
        {
            var item = await ItemPortal.CreateChild(criteria);
            Add(item);
            return item;
        }

        public virtual void OnDeserializing()
        {
        }

        public virtual void OnDeserialized()
        {
            foreach (var item in this)
            {
                item.NeatooPropertyChanged += _ChildNeatooPropertyChanged;
                if (item is ISetParent setParent)
                {
                    setParent.SetParent(this.Parent);
                }
            }
        }

        protected virtual Task RaiseNeatooPropertyChanged(PropertyNameBreadCrumbs breadCrumbs)
        {
            return NeatooPropertyChanged?.Invoke(breadCrumbs) ?? Task.CompletedTask;
        }

        protected virtual Task OnChildNeatooPropertyChanged(PropertyNameBreadCrumbs breadCrumbs)
        {
            // Lists don't add to the breadcrumbs
            return RaiseNeatooPropertyChanged(breadCrumbs);
        }

        private Task _ChildNeatooPropertyChanged(PropertyNameBreadCrumbs propertyNameBreadCrumbs)
        {
            return OnChildNeatooPropertyChanged(propertyNameBreadCrumbs);
        }

        public async Task WaitForTasks()
        {
            foreach(var i in this)
            {
                await i.WaitForTasks();
            }
        }
        
    }
}
