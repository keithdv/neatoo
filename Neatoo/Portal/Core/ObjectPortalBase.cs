using Neatoo.AuthorizationRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neatoo.Portal.Core
{
    /// <summary>
    /// Provide Authorization Check
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectPortalBase<T>
        where T : IPortalTarget
    {

        protected IServiceScope Scope { get; }
        protected IPortalOperationManager OperationManager { get; }
        public ObjectPortalBase(IServiceScope scope)
        {
            Scope = scope;

            // To find the static method this needs to be the concrete type
            var concreteType = scope.ConcreteType<T>(); // TODO:  ?? throw new Exception($"Type {typeof(T).FullName} is not registered");
            if (concreteType != null)
            {
                OperationManager = (IPortalOperationManager)scope.Resolve(typeof(IPortalOperationManager<>).MakeGenericType(concreteType));
            }
        }

    }

}
