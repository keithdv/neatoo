using System;
using System.Collections.Generic;
using System.Text;

namespace Neatoo.AuthorizationRules
{
    [System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class ExecuteAttribute : Attribute
    {
        public AuthorizeOperation AuthorizeOperation { get; }
        public ExecuteAttribute(AuthorizeOperation operation)
        {
            this.AuthorizeOperation = operation;
        }
    }
}
