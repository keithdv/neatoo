﻿using Neatoo.Attributes;
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

        void ISetParent.SetParent(IBase parent)
        {
            Parent = parent;
        }

        protected IRegisteredProperty<PV> GetRegisteredProperty<PV>(string name)
        {
            return PropertyValueManager.GetRegisteredProperty<PV>(name);
        }

        protected virtual P Getter<P>([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            return ReadProperty<P>(GetRegisteredProperty<P>(propertyName));
        }

        protected virtual void Setter<P>(P value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            LoadProperty(GetRegisteredProperty<P>(propertyName), value);
        }

        protected virtual P ReadProperty<P>(IRegisteredProperty<P> property)
        {
            return PropertyValueManager.ReadProperty<P>(property);
        }

        protected virtual void LoadProperty<P>(IRegisteredProperty<P> registeredProperty, P value)
        {
            PropertyValueManager.LoadProperty(registeredProperty, value);
        }

        public bool IsStopped { get; protected set; }

        public virtual Task<IDisposable> StopAllActions()
        {
            if (IsStopped) { return Task.FromResult<IDisposable>(null); } // You are a nested using; You get nothing!!
            IsStopped = true;
            return Task.FromResult<IDisposable>(new Core.Stopped(this));
        }

        public void StartAllActions()
        {
            if (IsStopped)
            {
                IsStopped = false;
            }
        }

        Task<IDisposable> IPortalTarget.StopAllActions()
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

        P IRegisteredPropertyAccess.ReadProperty<P>(IRegisteredProperty<P> registeredProperty)
        {
            return PropertyValueManager.ReadProperty(registeredProperty);
        }
               
        void IRegisteredPropertyAccess.SetProperty<P>(IRegisteredProperty<P> registeredProperty, P value)
        {
            PropertyValueManager.LoadProperty(registeredProperty, value);
        }

        void IRegisteredPropertyAccess.LoadProperty<P>(IRegisteredProperty<P> registeredProperty, P value)
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
