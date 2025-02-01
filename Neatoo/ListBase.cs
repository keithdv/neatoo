using Neatoo.AuthorizationRules;
using Neatoo.Core;
using Neatoo.Portal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Neatoo
{

    public interface IReadOnlyListBase<I> : INeatooObject, INotifyCollectionChanged, INotifyPropertyChanged, IReadOnlyCollection<I>, IReadOnlyList<I>
        where I : IBase
    {

    }


    public interface IListBase : INeatooObject, INotifyCollectionChanged, INotifyPropertyChanged, IEnumerable, ICollection, IList
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


    public abstract class ListBase<T, I> : ObservableCollection<I>, INeatooObject, IListBase<I>, IListBase, IReadOnlyListBase<I>, ISetParent, IJsonOnDeserialized, IJsonOnDeserializing
        where T : ListBase<T, I>
        where I : IBase
    {
        protected IReadPortalChild<I> ItemPortal { get; }

        public ListBase(IListBaseServices<T, I> services)
        {
            ItemPortal = services.ReadPortal;
        }

        public IBase Parent { get; protected set; }

        #region "Match Base"
        void ISetParent.SetParent(IBase parent)
        {
            Parent = parent;
        }


        #endregion


        protected override void InsertItem(int index, I item)
        {
            ((ISetParent)item).SetParent(this.Parent);

            base.InsertItem(index, item);

            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
            item.PropertyChanged += _ChildPropertyChanged;
        }

        protected override void RemoveItem(int index)
        {
            this[index].PropertyChanged -= _ChildPropertyChanged;

            base.RemoveItem(index);

            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
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

        private void _ChildPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ChildPropertyChanged(sender, e);
        }

        protected virtual void ChildPropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        public virtual void OnDeserialized()
        {
            foreach (var item in this)
            {
                item.PropertyChanged += _ChildPropertyChanged;
                if(item is ISetParent setParent)
                {
                    setParent.SetParent(this.Parent);
                }
            }
        }

        public virtual void OnDeserializing()
        {
            
        }
    }

}
