//using Microsoft.Extensions.DependencyInjection;
//using Neatoo.Portal.Internal;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;
//using static System.Formats.Asn1.AsnWriter;

//namespace Neatoo.Portal
//{
//    internal interface IDataMapperMethodDelegateBridge
//    {
//        public DataMapperMethod Operation { get; set; }

//        MethodInfo MethodInfo { get; set; }
//        object Target { get; set; }
//    }

//    internal interface IDataMapperMethodDelegateBridge<T> : IDataMapperMethodDelegateBridge
//    {
//    }

//    internal interface IDataMapperMethodDelegateBridge<P, T> : IDataMapperMethodDelegateBridge
//    {
//    }

//    internal interface IDataMapperMethodDelegateBridge<P1, P2, T> : IDataMapperMethodDelegateBridge
//    {
//    }

//    internal interface IDataMapperMethodDelegateBridge<P1, P2, P3, T> : IDataMapperMethodDelegateBridge
//    {
//    }

//    internal interface IDataMapperMethodDelegateBridge<P1, P2, P3, P4, T> : IDataMapperMethodDelegateBridge
//    {
//    }

//    internal interface IDataMapperMethodDelegateBridge<P1, P2, P3, P4, P5, T> : IDataMapperMethodDelegateBridge
//    {
//    }

//    internal class DataMapperMethodDelegateBridge<T> : DataMapperMethodDelegateBridge, IDataMapperMethodDelegateBridge<T>
//    {
//        public DataMapperMethodDelegateBridge(IServiceProvider serviceProvider) : base(serviceProvider) { }

//        public async Task<T> Bridge()
//        {
//            await TryCallOperation(Target, []);
//            return (T)Target;
//        }
//    }

//    internal class DataMapperMethodDelegateBridge<P, T> : DataMapperMethodDelegateBridge, IDataMapperMethodDelegateBridge<P, T>
//    {
//        public DataMapperMethodDelegateBridge(IServiceProvider serviceProvider) : base(serviceProvider) { }

//        public async Task<T> Bridge(P p)
//        {
//            await TryCallOperation(Target, [p]);
//            return (T)Target;
//        }
//    }

//    internal class DataMapperMethodDelegateBridge<P1, P2, T> : DataMapperMethodDelegateBridge, IDataMapperMethodDelegateBridge<P1, P2, T>
//    {
//        public DataMapperMethodDelegateBridge(IServiceProvider serviceProvider) : base(serviceProvider) { }

//        public async Task<T> Bridge(P1 p1, P2 p2)
//        {
//            await TryCallOperation(Target, [p1, p2]);
//            return (T)Target;
//        }
//    }

//    internal class DataMapperMethodDelegateBridge<P1, P2, P3, T> : DataMapperMethodDelegateBridge, IDataMapperMethodDelegateBridge<P1, P2, P3, T>
//    {
//        public DataMapperMethodDelegateBridge(IServiceProvider serviceProvider) : base(serviceProvider) { }

//        public async Task<T> Bridge(P1 p1, P2 p2, P3 p3)
//        {
//            await TryCallOperation(Target, [p1, p2, p3]);
//            return (T)Target;
//        }
//    }

//    internal class DataMapperMethodDelegateBridge<P1, P2, P3, P4, T> : DataMapperMethodDelegateBridge, IDataMapperMethodDelegateBridge<P1, P2, P3, P4, T>
//    {
//        public DataMapperMethodDelegateBridge(IServiceProvider serviceProvider) : base(serviceProvider) { }

//        public async Task<T> Bridge(P1 p1, P2 p2, P3 p3, P4 p4)
//        {
//            await TryCallOperation(Target, [p1, p2, p3, p4]);
//            return (T)Target;
//        }
//    }

//    internal class DataMapperMethodDelegateBridge<P1, P2, P3, P4, P5, T> : DataMapperMethodDelegateBridge, IDataMapperMethodDelegateBridge<P1, P2, P3, P4, P5, T>
//    {
//        public DataMapperMethodDelegateBridge(IServiceProvider serviceProvider) : base(serviceProvider) { }

//        public async Task<T> Bridge(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
//        {
//            await TryCallOperation(Target, [p1, p2, p3, p4, p5]);
//            return (T)Target;
//        }
//    }

//    internal class DataMapperMethodDelegateBridge : IDataMapperMethodDelegateBridge
//    {
//        private readonly IServiceProvider serviceProvider;


//        public DataMapperMethod Operation { get; set; }

//        public MethodInfo MethodInfo { get; set; }
//        public object Target { get; set; }

//        public DataMapperMethodDelegateBridge(IServiceProvider serviceProvider)
//        {
//            this.serviceProvider = serviceProvider;
//        }

//        public async Task<bool> TryCallOperation(object target, object[] criteria)
//        {
//            //await CheckAccess(operation.ToAuthorizationOperation(), criteria);

//            IDisposable? stopAllActions = null;

//            if (target is IDataMapperTarget portalTarget)
//            {
//                stopAllActions = portalTarget.PauseAllActions();
//            }

//            using (stopAllActions)
//            {
//                var method = MethodInfo;

//                if (method == null)
//                {
//                    return false;
//                }

//                var parameters = method.GetParameters().ToList();
//                var parameterValues = new object[parameters.Count()];

//                if (parameters.Count == 1 && parameters[0].ParameterType == typeof(object[]))
//                {
//                    parameterValues = [criteria];
//                }
//                else
//                {


//                    var criteriaE = criteria.GetEnumerator();

//                    for (var i = 0; i < parameterValues.Length; i++)
//                    {
//                        if (criteriaE.MoveNext())
//                        {
//                            // Use up the criteria values first
//                            // Assume MethodForOperation got the types right
//                            parameterValues[i] = criteriaE.Current;
//                        }
//                        else
//                        {
//                            var parameter = parameters[i];
//                            var pv = serviceProvider.GetService(parameter.ParameterType);
//                            if (pv != null)
//                            {
//                                parameterValues[i] = pv;
//                            }
//                        }
//                    }
//                }
//                var result = method.Invoke(target, parameterValues);

//                if (method.ReturnType == typeof(Task))
//                {
//                    await (Task)result;
//                }

//            }

//            PostOperation(target, Operation);

//            return true;
//        }

//        protected virtual void PostOperation(object target, DataMapperMethod operation)
//        {
//            var editTarget = target as IDataMapperEditTarget;
//            if (editTarget != null)
//            {


//                switch (operation)
//                {
//                    case DataMapperMethod.Create:
//                        editTarget.MarkNew();
//                        break;
//                    case DataMapperMethod.CreateChild:
//                        editTarget.MarkAsChild();
//                        editTarget.MarkNew();
//                        break;
//                    case DataMapperMethod.Fetch:
//                        break;
//                    case DataMapperMethod.FetchChild:
//                        editTarget.MarkAsChild();
//                        break;
//                    case DataMapperMethod.Delete:
//                        break;
//                    case DataMapperMethod.DeleteChild:
//                        break;
//                    case DataMapperMethod.Insert:
//                    case DataMapperMethod.InsertChild:
//                    case DataMapperMethod.Update:
//                    case DataMapperMethod.UpdateChild:
//                        editTarget.MarkUnmodified();
//                        editTarget.MarkOld();
//                        break;
//                    default:
//                        break;
//                }
//            }
//        }
//    }
//}
