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
        /// <summary>
        /// Stop events, rules and ismodified
        /// Only affects the Setter method
        /// Not SetProperty or LoadProperty
        /// </summary>
        bool IsStopped { get; }

        IBase Parent { get; }

    }

    [PortalDataContract]
    public abstract class Base<T> : INeatooObject, IBase, IPortalTarget, IRegisteredPropertyAccess, ISetParent
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

        protected virtual void Setter<P>(PropertyValue<P> value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            //I can't throw the propertyvalue away in case they are using it for something else
            // I just need to ensure it is the right case and if it is not use CreatePropertyInfo in the factory
            LoadProperty(GetRegisteredProperty(propertyName), value);
        }

        protected virtual P ReadProperty<P>(IRegisteredProperty property)
        {
            return PropertyValueManager.ReadProperty<P>(property);
        }

        protected virtual void LoadProperty<P>(string propertyName, P value)
        {
            LoadProperty(GetRegisteredProperty(propertyName), value);
        }

        protected virtual void LoadProperty<P>(IRegisteredProperty registeredProperty, P value)
        {
            PropertyValueManager.LoadProperty(registeredProperty, value);
        }

        protected virtual void LoadProperty<P>(IRegisteredProperty registeredProperty, PropertyValue<P> value)
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

        Task IPortalTarget.PostPortalConstruct()
        {
            return this.PostPortalConstruct();
        }

        protected virtual Task PostPortalConstruct()
        {
            return Task.CompletedTask;
        }

        P IRegisteredPropertyAccess.ReadProperty<P>(IRegisteredProperty registeredProperty)
        {
            return PropertyValueManager.ReadProperty<P>(registeredProperty);
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
