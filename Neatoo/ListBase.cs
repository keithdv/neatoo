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

    public abstract class ListBase<T, I> : ObservableCollection<I>, INeatooObject, IListBase<I>, IListBase, IReadOnlyListBase<I>, IPortalTarget, IRegisteredPropertyAccess, ISetParent
        where T : ListBase<T, I>
        where I : IBase
    {

        protected IPropertyValueManager<T> PropertyValueManager { get; private set; } // Private setter for Deserialization

        protected IReadPortalChild<I> ItemPortal { get; }

        public ListBase(IListBaseServices<T, I> services)
        {
            PropertyValueManager = services.PropertyValueManager;
            ItemPortal = services.ReadPortal;
        }

        public IBase Parent { get; protected set; }

        void ISetParent.SetParent(IBase parent)
        {
            Parent = parent;
        }

        Task IPortalTarget.PostPortalConstruct()
        {
            return this.PostPortalConstruct();
        }

        protected virtual Task PostPortalConstruct()
        {
            return Task.CompletedTask;
        }

        protected IRegisteredProperty GetRegisteredProperty(string name)
        {
            return PropertyValueManager.GetRegisteredProperty(name);
        }

        protected virtual P Getter<P>([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            return ReadProperty<P>(GetRegisteredProperty(propertyName));
        }

        protected virtual void Setter<P>(P value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            LoadProperty(GetRegisteredProperty(propertyName), value);
        }

        protected virtual P ReadProperty<P>(IRegisteredProperty property)
        {
            return PropertyValueManager.ReadProperty<P>(property);
        }

        protected virtual void LoadProperty<P>(IRegisteredProperty registeredProperty, P value)
        {
            PropertyValueManager.LoadProperty(registeredProperty, value);
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

        P IRegisteredPropertyAccess.ReadProperty<P>(IRegisteredProperty registeredProperty)
        {
            return (P) PropertyValueManager.ReadProperty(registeredProperty);
        }

        void IRegisteredPropertyAccess.SetProperty<P>(IRegisteredProperty registeredProperty, P value)
        {
            PropertyValueManager.LoadProperty(registeredProperty, value);
        }

        void IRegisteredPropertyAccess.LoadProperty<P>(IRegisteredProperty registeredProperty, P value)
        {
            PropertyValueManager.LoadProperty(registeredProperty, value);
        }

        IPropertyValue IRegisteredPropertyAccess.ReadPropertyValue(string propertyName)
        {
            return PropertyValueManager.ReadProperty(propertyName);
        }

        IPropertyValue IRegisteredPropertyAccess.ReadPropertyValue(IRegisteredProperty registeredProperty)
        {
            return PropertyValueManager.ReadProperty(registeredProperty);
        }

    }

}
