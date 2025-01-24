using Neatoo.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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


        new IEditPropertyValue GetProperty(string propertyName);
        new IEditPropertyValue GetProperty(IRegisteredProperty registeredProperty);

        public new IEditPropertyValue this[string propertyName] { get => GetProperty(propertyName); }
        public new IEditPropertyValue this[IRegisteredProperty registeredProperty] { get => GetProperty(registeredProperty); }
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

        public EditPropertyValue(string name) : base(name)
        {
        }

        public IEditBase EditChild => Value as IEditBase;

        protected override void OnValueChanged(T newValue)
        {
            base.OnValueChanged(newValue);

            if (Parent is IEditBase edit)
            {
                if (!edit.IsStopped)
                {
                    IsSelfModified = true && EditChild == null; // Never consider ourself modified if holding a Neatoo object
                }
            }
        }


        public bool IsModified => IsSelfModified || (EditChild?.IsModified ?? false);

        [PortalDataMember]
        public bool IsSelfModified { get; private set; } = false;

        public void MarkSelfUnmodified()
        {
            IsSelfModified = false;
        }

        public override void LoadProperty(object value)
        {
            base.LoadProperty(value);
            IsSelfModified = false;
        }
    }

    public class EditPropertyValueManager<T> : ValidatePropertyValueManagerBase<T, IEditPropertyValue>, IEditPropertyValueManager<T>
        where T : IBase
    {

        IRegisteredPropertyManager<T> IPropertyValueManager<T>.RegisteredPropertyManager => RegisteredPropertyManager;

        public EditPropertyValueManager(IRegisteredPropertyManager<T> registeredPropertyManager, IFactory factory) : base(registeredPropertyManager, factory)
        {

        }

        protected override IEditPropertyValue CreatePropertyValue<PV>(IRegisteredProperty registeredProperty, IBase parent)
        {
            return Factory.CreateEditPropertyValue<PV>(registeredProperty, parent);
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

        public IEditPropertyValue this[string propertyName]
        {
            get => GetProperty(propertyName);
        }

        public IEditPropertyValue this[IRegisteredProperty registeredProperty]
        {
            get => GetProperty(registeredProperty);
        }


        public virtual IEditPropertyValue GetProperty(string propertyName)
        {
            return GetProperty(RegisteredPropertyManager.GetRegisteredProperty(propertyName));
        }

        public virtual IEditPropertyValue GetProperty(IRegisteredProperty registeredProperty)
        {
            if (fieldData.TryGetValue(registeredProperty.Index, out var fd))
            {
                return fd;
            }

            var newPropertyValue = (IEditPropertyValue)this.GetType().GetMethod(nameof(this.CreatePropertyValue), System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).MakeGenericMethod(registeredProperty.Type).Invoke(this, new object[] { registeredProperty, Target });

            fieldData[registeredProperty.Index] = newPropertyValue;

            return newPropertyValue;
        }

        IValidatePropertyValue IValidatePropertyValueManager.GetProperty(string propertyName)
        {
            return GetProperty(propertyName);
        }

        IValidatePropertyValue IValidatePropertyValueManager.GetProperty(IRegisteredProperty registeredProperty)
        {
            return GetProperty(registeredProperty);
        }

        IPropertyValue IPropertyValueManager.GetProperty(string propertyName)
        {
            return GetProperty(propertyName);
        }

        IPropertyValue IPropertyValueManager.GetProperty(IRegisteredProperty registeredProperty)
        {
            return GetProperty(registeredProperty);
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
