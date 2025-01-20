using Neatoo.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neatoo.Core
{
    public interface IEditPropertyValueManager : IValidatePropertyValueManager
    {
        bool IsModified { get; }
        bool IsSelfModified { get; }

        IEnumerable<string> ModifiedProperties { get; }
        void MarkSelfUnmodified();
    }

    public interface IEditPropertyValueManager<T> : IEditPropertyValueManager, IValidatePropertyValueManager<T>
    {

    }

    public interface IEditPropertyValue : IValidatePropertyValue
    {
        bool IsModified { get; }
        bool IsSelfModified { get; }
        void MarkSelfUnmodified();
    }

    public interface IEditPropertyValue<T> : IEditPropertyValue, IValidatePropertyValue<T>
    {

    }

    [PortalDataContract]
    public class EditPropertyValue<T> : ValidatePropertyValue<T>, IEditPropertyValue<T>
    {


        private bool initialValue = true;
        public EditPropertyValue(string name, T value) : base(name, value)
        {
            EditChild = value as IEditBase;
            initialValue = false;
        }

        public IEditBase EditChild { get; protected set; }

        protected override void OnValueChanged(T newValue)
        {
            base.OnValueChanged(newValue);
            EditChild = newValue as IEditBase;
            if (!initialValue)
            {
                IsSelfModified = true && EditChild == null; // Never consider ourself modified if Neatoo object
            }
        }

        public bool IsModified => IsSelfModified || (EditChild?.IsModified ?? false);

        [PortalDataMember]
        public bool IsSelfModified { get; private set; } = false;

        public void MarkSelfUnmodified()
        {
            IsSelfModified = false;
        }
        //public static implicit operator T(EditPropertyValue<T> value) => value.Value;
        //public static implicit operator EditPropertyValue<T>(T value) => new EditPropertyValue<T>(value);
        //public static implicit operator Task(EditPropertyValue<T> value) => value.Task;
    }

    public class EditPropertyValueManager<T> : ValidatePropertyValueManagerBase<T, IEditPropertyValue>, IEditPropertyValueManager<T>
        where T : IBase
    {

        IRegisteredPropertyManager<T> IPropertyValueManager<T>.RegisteredPropertyManager => RegisteredPropertyManager;

        public EditPropertyValueManager(IRegisteredPropertyManager<T> registeredPropertyManager, IFactory factory, IValuesDiffer valuesDiffer) : base(registeredPropertyManager, factory, valuesDiffer)
        {

        }

        protected override IEditPropertyValue CreatePropertyValue<PV>(IRegisteredProperty<PV> registeredProperty, PV value)
        {
            return Factory.CreateEditPropertyValue(registeredProperty, value);
        }

        public bool IsModified => fieldData.Values.Any(p => p.IsModified);
        public bool IsSelfModified => fieldData.Values.Any(p => p.IsSelfModified);

        public IEnumerable<string> ModifiedProperties => fieldData.Values.Where(f => f.IsModified).Select(f => f.Name);

        public void MarkSelfUnmodified()
        {
            foreach (var fd in fieldData.Values)
            {
                fd.MarkSelfUnmodified();
            }
        }

        public virtual void LoadProperty<PV>(IRegisteredProperty<PropertyValue<PV>> registeredProperty, PropertyValue<PV> newValue)
        {
            if (!fieldData.ContainsKey(registeredProperty.Index))
            {
                // TODO Destroy and Delink to old value
                // TODO - If they've created an event link to the PropertyValue.NotifyPropertyChanged event
                // Does just create a new one create a memory leak?
            }
            newValue.Name = registeredProperty.Name;
            fieldData[registeredProperty.Index] = (IEditPropertyValue)newValue;

            SetParent(newValue.Value);
        }

    }


    [Serializable]
    public class RegisteredPropertyEditChildDataWrongTypeException : Exception
    {
        public RegisteredPropertyEditChildDataWrongTypeException() { }
        public RegisteredPropertyEditChildDataWrongTypeException(string message) : base(message) { }
        public RegisteredPropertyEditChildDataWrongTypeException(string message, Exception inner) : base(message, inner) { }
        protected RegisteredPropertyEditChildDataWrongTypeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

}
