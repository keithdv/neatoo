using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

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
        public IEditMetaProperties? EditChild => Value as IEditMetaProperties;

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

        public override void LoadValue(object? value)
        {
            base.LoadValue(value);
            IsSelfModified = false;
        }
    }


    public delegate IEditPropertyManager CreateEditPropertyManager(IPropertyInfoList propertyInfoList);

    public class EditPropertyManager : ValidatePropertyManager<IEditProperty>, IEditPropertyManager
    {


        public EditPropertyManager(IPropertyInfoList propertyInfoList, IFactory factory) : base(propertyInfoList, factory)
        {

        }

        protected new IProperty CreateProperty<PV>(IPropertyInfo propertyInfo)
        {
            var property = Factory.CreateEditProperty<PV>(propertyInfo);
            property.IsPaused = IsPaused;
            return property;
        }

        public bool IsModified => PropertyBag.Values.Any(p => p.IsModified);
        public bool IsSelfModified => PropertyBag.Values.Any(p => p.IsSelfModified);
        public bool IsPaused = false;

        public IEnumerable<string> ModifiedProperties => PropertyBag.Values.Where(f => f.IsModified).Select(f => f.Name);

        public void MarkSelfUnmodified()
        {
            foreach (var fd in PropertyBag.Values)
            {
                fd.MarkSelfUnmodified();
            }
        }



        public void PauseAllActions()
        {
            IsPaused = true;
            foreach (var fd in PropertyBag.Values)
            {
                fd.IsPaused = true;
            }
        }

        public void ResumeAllActions()
        {
            IsPaused = false;
            foreach (var fd in PropertyBag.Values)
            {
                fd.IsPaused = false;
            }
        }   
    }


    [Serializable]
    public class PropertyInfoEditChildDataWrongTypeException : Exception
    {
        public PropertyInfoEditChildDataWrongTypeException() { }
        public PropertyInfoEditChildDataWrongTypeException(string message) : base(message) { }
        public PropertyInfoEditChildDataWrongTypeException(string message, Exception inner) : base(message, inner) { }

    }

}
