using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neatoo.Portal.Internal
{
    public class FactoryBase
    {

        protected async Task DoMapperMethodCall(object target, DataMapperMethod operation, Func<Task> mapperMethodCall)
        {
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
                    case DataMapperMethod.CreateChild:
                        editTarget.MarkAsChild();
                        editTarget.MarkNew();
                        break;
                    case DataMapperMethod.Fetch:
                        break;
                    case DataMapperMethod.FetchChild:
                        editTarget.MarkAsChild();
                        break;
                    case DataMapperMethod.Delete:
                        break;
                    case DataMapperMethod.DeleteChild:
                        break;
                    case DataMapperMethod.Insert:
                    case DataMapperMethod.InsertChild:
                    case DataMapperMethod.Update:
                    case DataMapperMethod.UpdateChild:
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
