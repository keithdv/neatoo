using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Neatoo.Core
{
    public interface IEditPropertyManager : IValidatePropertyManager<IEditProperty>
    {
        bool IsModified { get; }
        bool IsSelfModified { get; }

        IEnumerable<string> ModifiedProperties { get; }
        void MarkSelfUnmodified();

        void PauseAllActions();
        void ResumeAllActions();
    }

    public interface IEditProperty : IValidateProperty
    {
        bool IsPaused { get; set; }
        bool IsModified { get; }
        bool IsSelfModified { get; }
        void MarkSelfUnmodified();
    }

    public interface IEditProperty<T> : IEditProperty, IValidateProperty<T>
    {

    }

    public class EditProperty<T> : ValidateProperty<T>, IEditProperty<T>
    {

        public EditProperty(string name) : base(name)
        {
        }

        [JsonConstructor]
        public EditProperty(string name, T value, bool isSelfModified, string[] serializedErrorMessages) : base(name, value, serializedErrorMessages)
        {
            IsSelfModified = isSelfModified;
        }

        [JsonIgnore]
        public IEditMetaProperties EditChild => Value as IEditMetaProperties;

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(Value))
            {
                if (!IsPaused)
                {
                    IsSelfModified = true && EditChild == null; // Never consider ourself modified if holding a Neatoo object
                }
            }
        }

        public bool IsModified => IsSelfModified || (EditChild?.IsModified ?? false);

        public bool IsSelfModified { get; protected set; } = false;

        public bool IsPaused { get; set; } = false;

        public void MarkSelfUnmodified()
        {
            IsSelfModified = false;
        }

        public override void LoadValue(object value)
        {
            base.LoadValue(value);
            IsSelfModified = false;
        }
    }


    public delegate IEditPropertyManager CreateEditPropertyManager(IRegisteredPropertyManager registeredPropertyManager);

    public class EditPropertyManager : ValidatePropertyManager<IEditProperty>, IEditPropertyManager
    {


        public EditPropertyManager(IRegisteredPropertyManager registeredPropertyManager, IFactory factory) : base(registeredPropertyManager, factory)
        {

        }

        protected new IProperty CreateProperty<PV>(IRegisteredProperty registeredProperty)
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

        public void PauseAllActions()
        {
            foreach (var fd in fieldData.Values)
            {
                fd.IsPaused = true;
            }
        }

        public void ResumeAllActions()
        {
            foreach (var fd in fieldData.Values)
            {
                fd.IsPaused = false;
            }
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
