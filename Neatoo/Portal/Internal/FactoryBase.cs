using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neatoo.Portal.Internal
{
    public interface IFactoryBase<in T>
    {
    }

    public abstract class FactoryBase<T> : IFactoryBase<T>
    {
        protected async Task DoMapperMethodCall(object target, DataMapperMethod operation, Func<Task> mapperMethodCall)
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

            using (stopAllActions)
            {
                await mapperMethodCall();
            }

            PostOperation(target, operation);
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
