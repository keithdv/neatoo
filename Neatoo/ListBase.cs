using Neatoo.Attributes;
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
using System.Threading.Tasks;

namespace Neatoo
{

    public interface IReadOnlyListBase<I> : IBase, INeatooObject, INotifyCollectionChanged, INotifyPropertyChanged, IReadOnlyCollection<I>, IReadOnlyList<I>
        where I : IBase
    {
        
    }


    public interface IListBase : IBase, INeatooObject, INotifyCollectionChanged, INotifyPropertyChanged, IEnumerable, ICollection, IList
    {

    }

    public interface IListBase<I> : IReadOnlyListBase<I>, IEnumerable<I>, ICollection<I>, IList<I>
        where I : IBase
    {
        Task<I> CreateAdd();
        Task<I> CreateAdd(params object[] criteria);

        new int Count { get; }
    }

    public abstract class ListBase<T, I> : ObservableCollection<I>, INeatooObject, IListBase<I>, IListBase, IReadOnlyListBase<I>, IPortalTarget, ISetParent
        where T : ListBase<T, I>
        where I : IBase
    {

        protected IPropertyValueManager<T> PropertyValueManager { get; private set; } // Private setter for Deserialization

        protected IReadPortalChild<I> ItemPortal { get; }

        public ListBase(IListBaseServices<T, I> services)
        {
            PropertyValueManager = services.PropertyValueManager;
            ((ISetTarget)PropertyValueManager).SetTarget(this);
            ItemPortal = services.ReadPortal;
        }

        public IBase Parent { get; protected set; }


        #region "Match Base"
        void ISetParent.SetParent(IBase parent)
        {
            Parent = parent;
        }


        protected virtual P Getter<P>([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            return (P)PropertyValueManager[propertyName].Value;
        }


        protected virtual void Setter<P>(P value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            PropertyValueManager[propertyName].SetValue(value);
        }

        protected virtual Task HandlePropertyChanged(string propertyName, IBase source)
        {
            Parent?.HandlePropertyChanged(propertyName, this);
            return Task.CompletedTask;
        }

        Task IBase.HandlePropertyChanged(string propertyName, IBase source)
        {
            return HandlePropertyChanged(propertyName, source);
        }
        protected IRegisteredProperty GetRegisteredProperty(string propertyName)
        {
            return PropertyValueManager.RegisteredPropertyManager.GetRegisteredProperty(propertyName);
        }

        public IPropertyValue GetProperty(string propertyName)
        {
            return PropertyValueManager[propertyName];
        }

        public IPropertyValue GetProperty(IRegisteredProperty registeredProperty)
        {
            return PropertyValueManager[registeredProperty];
        }

        IDisposable IPortalTarget.StopAllActions()
        {
            return null;
        }

        void IPortalTarget.StartAllActions()
        {
        }

        Task IPortalTarget.PostPortalConstruct()
        {
            return this.PostPortalConstruct();
        }

        protected virtual Task PostPortalConstruct()
        {
            return Task.CompletedTask;
        }

        protected IPropertyValue this[string propertyName] { get => GetProperty(propertyName); }
        protected IPropertyValue this[IRegisteredProperty registeredProperty] { get => GetProperty(registeredProperty); }

        #endregion

        public async Task<I> CreateAdd()
        {
            var item = await ItemPortal.CreateChild();
            base.Add(item);
            return item;
        }

        public async Task<I> CreateAdd(params object[] criteria)
        {
            var item = await ItemPortal.CreateChild(criteria);
            base.Add(item);
            return item;
        }


    }

}
