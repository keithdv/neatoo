using Neatoo.Attributes;
using Neatoo.AuthorizationRules;
using Neatoo.Core;
using Neatoo.Portal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Neatoo
{

    public interface IBase : INeatooObject
    {


        IBase Parent { get; }

        internal void HandlePropertyChanged(string propertyName, IBase source);

        internal IPropertyValue GetProperty(string propertyName);
        internal IPropertyValue GetProperty(IRegisteredProperty registeredProperty);

        internal IPropertyValue this[string propertyName] { get => GetProperty(propertyName); }
        internal IPropertyValue this[IRegisteredProperty registeredProperty] { get => GetProperty(registeredProperty); }

    }

    [PortalDataContract]
    public abstract class Base<T> : INeatooObject, IBase, IPortalTarget, ISetParent
        where T : Base<T>
    {

        [PortalDataMember]
        protected IPropertyValueManager<T> PropertyValueManager { get; }

        public Base(IBaseServices<T> services)
        {
            PropertyValueManager = services.PropertyValueManager;
            ((ISetTarget)PropertyValueManager).SetTarget(this);
        }

        [PortalDataMember]
        public IBase Parent { get; protected set; }

        protected virtual void SetParent(IBase parent)
        {
            Parent = parent;
        }
        void ISetParent.SetParent(IBase parent)
        {
            SetParent(parent);
        }

        protected virtual P Getter<P>([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            return (P) PropertyValueManager[propertyName].Value;
        }

        protected virtual void Setter<P>(P value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            PropertyValueManager[propertyName].SetValue(value);
        }

        protected virtual void HandlePropertyChanged(string propertyName, IBase source)
        {
            Parent?.HandlePropertyChanged(propertyName, this);
        }

        void IBase.HandlePropertyChanged(string propertyName, IBase source)
        {
            HandlePropertyChanged(propertyName, source);
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
    }

}
