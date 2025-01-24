using Neatoo.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Neatoo.Rules
{

    public interface IRuleResult
    {
        bool IsError { get; }

        IReadOnlyDictionary<string, string> PropertyErrorMessages { get; }

        IReadOnlyList<string> TriggerProperties { get; set; }

        Exception Exception { get; }
    }

    [PortalDataContract]
    public class RuleResult : IRuleResult
    {
        [PortalDataMember]
        protected Dictionary<string, string> PropertyErrorMessages { get; } = new Dictionary<string, string>();

        IReadOnlyDictionary<string, string> IRuleResult.PropertyErrorMessages => new ReadOnlyDictionary<string, string>(PropertyErrorMessages);

        public bool IsError { get { return PropertyErrorMessages.Any(); } }

        public Exception Exception { get; private set; }

        [PortalDataMember]
        public IReadOnlyList<string> TriggerProperties { get; set; }

        public static RuleResult Empty()
        {
            return new RuleResult();
        }
       
        public static RuleResult PropertyError(string propertyName, string message, Exception exception = null)
        {
            // TODO - Make a DI Delegate so that a custom RuleResult can be created

            var result = new RuleResult();
            // TODO - Bad logic?
            // I don't like the approac: create then AddPropertyError to be a clear approach to multiple errors
            result.PropertyErrorMessages.Add(propertyName, message);
            result.Exception = exception;
            return result;
        }

        internal void AddPropertyErrorMessage(string propertyName, string message)
        {
            PropertyErrorMessages.Add(propertyName, message);
        }

        [OnSerializing]
        public void OnSerializing(StreamingContext context)
        {
            // Readonly list cannot be serialized
            TriggerProperties = TriggerProperties?.ToList();
        }

    }

    public static class RuleResultExtensions
    {
        public static RuleResult AddPropertyError(this RuleResult rr, string propertyName, string message)
        {
            rr.AddPropertyErrorMessage(propertyName, message);
            return rr;
        }

    }

}
