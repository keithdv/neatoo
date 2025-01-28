using Neatoo.Attributes;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Neatoo.Core
{
    public interface IEditPropertyManager : IValidatePropertyManager
    {
        bool IsModified { get; }
        bool IsSelfModified { get; }

        IEnumerable<string> ModifiedProperties { get; }
        void MarkSelfUnmodified();


        new IEditProperty GetProperty(string propertyName);
        new IEditProperty GetProperty(IRegisteredProperty registeredProperty);

        public new IEditProperty this[string propertyName] { get => GetProperty(propertyName); }
        public new IEditProperty this[IRegisteredProperty registeredProperty] { get => GetProperty(registeredProperty); }

    }

    public interface IEditProperty : IValidateProperty
    {
        bool IsStopped { get; }
        bool IsModified { get; }
        bool IsSelfModified { get; }
        void MarkSelfUnmodified();


    }

    public interface IEditProperty<T> : IEditProperty, IValidateProperty<T>
    {

    }

    [PortalDataContract]
    public class EditProperty<T> : ValidateProperty<T>, IEditProperty<T>
    {

        public EditProperty(string name) : base(name)
        {
        }

        public IEditMetaProperties EditChild => Value as IEditMetaProperties;

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(Value))
            {
                if (!IsStopped)
                {
                    IsSelfModified = true && EditChild == null; // Never consider ourself modified if holding a Neatoo object
                }
            }
        }

        public bool IsModified => IsSelfModified || (EditChild?.IsModified ?? false);

        [PortalDataMember]
        public bool IsSelfModified { get; private set; } = false;

        public bool IsStopped { get; set; } = false;

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

    public class EditPropertyManager : ValidatePropertyManagerBase<IEditProperty>, IEditPropertyManager
    {

        IRegisteredPropertyManager IPropertyManager.RegisteredPropertyManager => RegisteredPropertyManager;

        public EditPropertyManager(IRegisteredPropertyManager registeredPropertyManager, IFactory factory) : base(registeredPropertyManager, factory)
        {

        }

        protected override IEditProperty CreateProperty<PV>(IRegisteredProperty registeredProperty)
        {
            return Factory.CreateEditProperty<PV>(registeredProperty);
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

        public IEditProperty this[string propertyName]
        {
            get => GetProperty(propertyName);
        }

        public IEditProperty this[IRegisteredProperty registeredProperty]
        {
            get => GetProperty(registeredProperty);
        }


        public virtual IEditProperty GetProperty(string propertyName)
        {
            return GetProperty(RegisteredPropertyManager.GetRegisteredProperty(propertyName));
        }

        IValidateProperty IValidatePropertyManager.GetProperty(string propertyName)
        {
            return GetProperty(propertyName);
        }

        IValidateProperty IValidatePropertyManager.GetProperty(IRegisteredProperty registeredProperty)
        {
            return GetProperty(registeredProperty);
        }

        IProperty IPropertyManager.GetProperty(string propertyName)
        {
            return GetProperty(propertyName);
        }

        IProperty IPropertyManager.GetProperty(IRegisteredProperty registeredProperty)
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
