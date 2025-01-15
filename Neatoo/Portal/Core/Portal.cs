using Neatoo.AuthorizationRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Neatoo.Portal.Core
{
    public interface IPortal
    {

    }

    public interface IPortal<T> : IPortal
    {
        Task<T> CallReadOperationMethod(PortalOperation operation, bool throwException);
        Task<T> CallReadOperationMethod(PortalOperation operation, object[] criteria);
        Task CallWriteChildOperationMethod<E>(E target) where E : IEditMetaProperties;
        Task CallWriteChildOperationMethod<E>(E target, object[] criteria) where E : IEditMetaProperties;
        Task CallWriteOperationMethod<E>(E target) where E : IEditMetaProperties;
        Task CallWriteOperationMethod<E>(E target, object[] criteria) where E : IEditMetaProperties;
        Task CallOperationMethod<E>(E target, PortalOperation operation, bool throwException = true);
        Task CallOperationMethod<E>(E target, PortalOperation operation, object[] criteria);
    }

    /// <summary>
    /// Provide Authorization Check
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Portal<T> : IPortal<T>
    {

        protected IServiceScope Scope { get; }
        public Portal(IServiceScope scope)
        {
            Scope = scope;


        }


        public async Task<T> CallReadOperationMethod(PortalOperation operation, bool throwException)
        {
            var target = Scope.Resolve<T>();
            await CallOperationMethod(target, operation, throwException);
            if(target is IPortalTarget editTarget)
            {
                await editTarget.PostPortalConstruct();
            }
            return target;
        }

        public async Task CallWriteOperationMethod<E>(E target) where E : IEditMetaProperties
        {
            if (target.IsDeleted)
            {
                if (!target.IsNew)
                {
                    await CallOperationMethod(target, PortalOperation.Delete);
                }
            }
            else if (target.IsNew)
            {
                await CallOperationMethod(target, PortalOperation.Insert);
            }
            else
            {
                await CallOperationMethod(target, PortalOperation.Update);
            }
        }
        public async Task CallWriteChildOperationMethod<E>(E target) where E : IEditMetaProperties
        {
            if (target.IsDeleted)
            {
                if (!target.IsNew)
                {
                    await CallOperationMethod(target, PortalOperation.DeleteChild);
                }
            }
            else if (target.IsNew)
            {
                await CallOperationMethod(target, PortalOperation.InsertChild);
            }
            else
            {
                await CallOperationMethod(target, PortalOperation.UpdateChild);
            }
        }
        public async Task CallOperationMethod<E>(E target, PortalOperation operation, bool throwException = true)
        {
            // Concrete type can vary since an interface can have more than one implementation
            // So need to load the actual concrete type
            var operationManager = (IPortalOperationManager)Scope.Resolve(typeof(IPortalOperationManager<>).MakeGenericType(target.GetType()));
         
            var success = await operationManager.TryCallOperation(target, operation);

            if (!success && throwException)
            {
                throw new OperationMethodCallFailedException($"{operation.ToString()} method with no criteria not found on {target.GetType().FullName}.");
            }

        }

        public async Task<T> CallReadOperationMethod(PortalOperation operation, object[] criteria)
        {
            var target = Scope.Resolve<T>();
            await CallOperationMethod(target, operation, criteria);
            if (target is IPortalTarget editTarget)
            {
                await editTarget.PostPortalConstruct();
            }
            return target;
        }

        public async Task CallWriteOperationMethod<E>(E target, object[] criteria) where E : IEditMetaProperties
        {
            if (target.IsDeleted)
            {
                if (!target.IsNew)
                {
                    await CallOperationMethod(target, PortalOperation.Delete, criteria);
                }
            }
            else if (target.IsNew)
            {
                await CallOperationMethod(target, PortalOperation.Insert, criteria);
            }
            else
            {
                await CallOperationMethod(target, PortalOperation.Update, criteria);
            }
        }

        public async Task CallWriteChildOperationMethod<E>(E target, object[] criteria) where E : IEditMetaProperties
        {
            if (target.IsDeleted)
            {
                if (!target.IsNew)
                {
                    await CallOperationMethod(target, PortalOperation.DeleteChild, criteria);
                }
            }
            else if (target.IsNew)
            {
                await CallOperationMethod(target, PortalOperation.InsertChild, criteria);
            }
            else
            {
                await CallOperationMethod(target, PortalOperation.UpdateChild, criteria);
            }
        }

        public async Task CallOperationMethod<E>(E target, PortalOperation operation, object[] criteria)
        {
            if (target == null) { throw new ArgumentNullException(nameof(target)); }
            if (criteria == null) { throw new ArgumentNullException(nameof(criteria)); }

            // Concrete type can vary since an interface can have more than one implementation
            // So need to load the actual concrete type
            var operationManager = (IPortalOperationManager)Scope.Resolve(typeof(IPortalOperationManager<>).MakeGenericType(target.GetType()));

            var success = await operationManager.TryCallOperation(target, operation, criteria);

            if (!success)
            {
                throw new OperationMethodCallFailedException($"{operation.ToString()} method on {target.GetType().FullName} with criteria [{string.Join(", ", criteria.Select(x => x.GetType().FullName))}] not found.");
            }

        }
    }

}
