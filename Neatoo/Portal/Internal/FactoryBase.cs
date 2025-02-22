using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neatoo.Portal.Internal
{

    public abstract class FactoryBase
    {
        protected virtual T DoMapperMethodCall<T>(T target, DataMapperMethod operation, Action mapperMethodCall)
        {
            return DoMapperMethodCallBoolAsync<T>(target, operation, () =>
            {
                mapperMethodCall();
                return Task.FromResult(true);
            }).Result!;  // Safe because I know there is no async fork
        }

        protected virtual T? DoMapperMethodCallBool<T>(T target, DataMapperMethod operation, Func<bool> mapperMethodCall)
        {
            return DoMapperMethodCallBoolAsync<T>(target, operation, () =>
            {
                return Task.FromResult(mapperMethodCall());
            }).Result; // Safe because I know there is no async fork
        }

        protected virtual Task<T> DoMapperMethodCallAsync<T>(T target, DataMapperMethod operation, Action mapperMethodCall)
        {
            return DoMapperMethodCallBoolAsync<T>(target, operation, () =>
            {
                mapperMethodCall();
                return Task.FromResult(true);
            })!;
        }

        protected virtual Task<T> DoMapperMethodCallAsync<T>(T target, DataMapperMethod operation, Func<Task> mapperMethodCall)
        {
            return DoMapperMethodCallBoolAsync(target, operation, async () =>
            {
                await mapperMethodCall();
                return true;
            })!;
        }

        protected virtual Task<T?> DoMapperMethodCallBoolAsync<T>(T target, DataMapperMethod operation, Func<bool> mapperMethodCall)
        {
            return DoMapperMethodCallBoolAsync<T>(target, operation, () =>
            {
                return Task.FromResult(mapperMethodCall());
            });
        }

        protected virtual async Task<T?> DoMapperMethodCallBoolAsync<T>(T target, DataMapperMethod operation, Func<Task<bool>> mapperMethodCall)
        {
            ArgumentNullException.ThrowIfNull(target, nameof(target));
            ArgumentNullException.ThrowIfNull(mapperMethodCall, nameof(mapperMethodCall));

            // TODO : Rosyln authorization
            //await CheckAccess(operation.ToAuthorizationOperation(), criteria);

            IDisposable? stopAllActions = null;

            if (target is IDataMapperTarget portalTarget)
            {
                stopAllActions = portalTarget.PauseAllActions();
            }

            var succeeded = true;
            using (stopAllActions)
            {
                succeeded = await mapperMethodCall();
            }

            if (!succeeded)
            {
                return default;
            }

            PostOperation(target, operation);

            return target;
        }

        protected virtual void PostOperation(object target, DataMapperMethod operation)
        {
            var editTarget = target as IDataMapperEditTarget;
            if (editTarget != null)
            {


                switch (operation)
                {
                    case DataMapperMethod.Create:
                        editTarget.MarkNew();
                        break;
                    case DataMapperMethod.Fetch:
                        break;
                    case DataMapperMethod.Delete:
                        break;
                    case DataMapperMethod.Insert:
                    case DataMapperMethod.Update:
                        editTarget.MarkUnmodified();
                        editTarget.MarkOld();
                        break;
                    default:
                        break;
                }
            }
        }


    }
}
