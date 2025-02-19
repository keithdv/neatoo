//using Microsoft.Extensions.DependencyInjection;
//using Neatoo.AuthorizationRules;
//using Neatoo.Portal.Internal;
//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Reflection;
//using System.Threading.Tasks;

//namespace Neatoo.Portal.Internal;

//public interface IDataMapper
//{
//    void RegisterOperation(DataMapperMethod operation, string methodName);
//    void RegisterOperation(DataMapperMethod operation, MethodInfo method);
//    Task<bool> TryCallOperation(object target, DataMapperMethod operation);
//    Task<bool> TryCallOperation(object target, DataMapperMethod operation, object[] criteria);
//    Task<object> HandlePortalRequest(RemoteRequest portalRequest);
//}
//public interface IDataMapper<T> : IDataMapper
//{


//}

//public class DataMapper<T> : IDataMapper<T>
//{
//    protected static IDictionary<DataMapperMethod, List<MethodInfo>> RegisteredOperations { get; } = new ConcurrentDictionary<DataMapperMethod, List<MethodInfo>>();
//    protected static bool IsRegistered { get; set; }
//    protected static object lockIsRegistered = new object();
//    private readonly IServiceProviderIsService serviceProviderIsService;

//    private IServiceProvider Scope { get; }

//    protected IAuthorizationRuleManager AuthorizationRuleManager { get; }

//    protected INeatooJsonSerializer jsonSerializer { get; }

//    public DataMapper(IServiceProvider scope, INeatooJsonSerializer jsonSerializer, IServiceProviderIsService serviceProviderIsService)
//    {
//#if DEBUG
//        if (typeof(T).IsInterface) { throw new Exception($"PortalOperationManager should be service type not an interface. {typeof(T).FullName}"); }
//#endif
//        RegisterPortalOperations();
//        Scope = scope;
//        this.jsonSerializer = jsonSerializer;
//        this.serviceProviderIsService = serviceProviderIsService;
//        AuthorizationRuleManager = scope.GetRequiredService<IAuthorizationRuleManager<T>>();
//    }


//    protected virtual void RegisterPortalOperations()
//    {
//        lock (lockIsRegistered)
//        {
//            if (!IsRegistered)
//            {
//                var methods = typeof(T).GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic)
//                    .Where(m => m.GetCustomAttributes<DataMapperMethodAttribute>().Any()).ToList();

//                foreach (var m in methods)
//                {
//                    var attributes = m.GetCustomAttributes<DataMapperMethodAttribute>().ToList();
//                    foreach (var o in attributes)
//                    {
//                        RegisterOperation(o.Operation, m);
//                    }
//                }
//                IsRegistered = true;
//            }
//        }
//    }

//    public void RegisterOperation(DataMapperMethod operation, string methodName)
//    {
//        var method = typeof(T).GetMethod(methodName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance) ?? throw new Exception("No method found");
//        RegisterOperation(operation, method);
//    }

//    public void RegisterOperation(DataMapperMethod operation, MethodInfo method)
//    {
//        var returnType = method.ReturnType;

//        if (!(returnType == typeof(void) || returnType == typeof(Task)))
//        {
//            throw new OperationMethodException($"{method.Name} must be void or return Task");
//        }

//        if (!RegisteredOperations.TryGetValue(operation, out var methodList))
//        {
//            RegisteredOperations.Add(operation, methodList = new List<MethodInfo>());
//        }

//        methodList.Add(method);
//    }

//    public IEnumerable<MethodInfo>? MethodsForOperation(DataMapperMethod operation)
//    {
//        if (!RegisteredOperations.TryGetValue(operation, out var methods))
//        {
//            return null;
//        }

//        return methods.AsReadOnly();
//    }


//    public MethodInfo? MethodForOperation(DataMapperMethod operation, IEnumerable<Type> criteriaTypes)
//    {
//        var methods = MethodsForOperation(operation);
//        MethodInfo? matchingMethod = null;

//        if (methods != null)
//        {

//            foreach (var m in methods)
//            {
//                var parameters = m.GetParameters();

//                // No criteria
//                if (!criteriaTypes.Any() && !parameters.Any())
//                {
//                    return m;
//                }
//                else if (parameters.Count() == 1 && parameters[0].ParameterType == typeof(object[]))
//                {
//                    matchingMethod = m;
//                }
//                else if (criteriaTypes.Any() && parameters.Any() && parameters.Count() >= criteriaTypes.Count())
//                {

//                    var match = true;
//                    var critEnum = criteriaTypes.GetEnumerator();
//                    var paramEnum = parameters.Cast<ParameterInfo>().Select(p => p.ParameterType).GetEnumerator();

//                    // With Array's Current doesn't become null
//                    var paramHasValue = paramEnum.MoveNext();
//                    var critHasValue = critEnum.MoveNext();

//                    // All of the criteria parameter types match up
//                    // And any left over are registered
//                    while (match && paramHasValue)
//                    {

//                        if (critHasValue && !paramEnum.Current.IsAssignableFrom(critEnum.Current))
//                        {
//                            match = false;
//                        }
//                        else if (!critHasValue && !serviceProviderIsService.IsService(paramEnum.Current)) // For recognizing multiple positives for the same criteria
//                        {
//                            match = false;
//                        }

//                        paramHasValue = paramEnum.MoveNext();
//                        critHasValue = critEnum.MoveNext();

//                    }

//                    // At the end of the Crit list
//                    // The parameter list can 
//                    if (match)
//                    {
//                        if (matchingMethod != null) { throw new Exception($"More then one method for {operation.ToString()} with criteria [{string.Join(",", criteriaTypes)}] on {typeof(T).FullName}"); }

//                        matchingMethod = m;
//                    }
//                }
//            }
//        }
//        return matchingMethod;
//    }

//    protected async Task CheckAccess(AuthorizeOperation operation)
//    {
//        await AuthorizationRuleManager.CheckAccess(operation);
//    }

//    protected async Task CheckAccess(AuthorizeOperation operation, params object[] criteria)
//    {
//        if (criteria == null) { throw new ArgumentNullException(nameof(criteria)); }

//        await AuthorizationRuleManager.CheckAccess(operation, criteria);
//    }

//    public async Task<bool> TryCallOperation(object target, DataMapperMethod operation)
//    {
//        await CheckAccess(operation.ToAuthorizationOperation());

//        var invoked = false;
//        var methods = MethodsForOperation(operation) ?? new List<MethodInfo>();

//        IDisposable? stopAllActions = null;

//        if (target is IDataMapperTarget portalTarget)
//        {
//            stopAllActions = portalTarget.PauseAllActions();
//        }

//        using (stopAllActions)
//        {
//            foreach (var method in methods)
//            {
//                var success = true;
//                var parameters = method.GetParameters().ToList();
//                var parameterValues = new object[parameters.Count()];

//                for (var i = 0; i < parameterValues.Length; i++)
//                {
//                    var parameter = parameters[i];
//                    if (!serviceProviderIsService.IsService(parameter.ParameterType))
//                    {
//                        // Assume it's a criteria not a dependency
//                        success = false;
//                        break;
//                    }
//                }

//                if (success)
//                {
//                    // No parameters or all of the parameters are dependencies
//                    for (var i = 0; i < parameterValues.Length; i++)
//                    {
//                        var parameter = parameters[i];
//                        parameterValues[i] = Scope.GetRequiredService(parameter.ParameterType);
//                    }

//                    invoked = true;

//                    var result = method.Invoke(target, parameterValues) as Task;

//                    if (result != null)
//                    {
//                        await result;
//                    }

//                    break;
//                }
//            }
//        }

//        PostOperation(target, operation);

//        return invoked;
        
//    }
//    public async Task<bool> TryCallOperation(object target, DataMapperMethod operation, object[] criteria)
//    {
//        await CheckAccess(operation.ToAuthorizationOperation(), criteria);

//        IDisposable? stopAllActions = null;

//        if (target is IDataMapperTarget portalTarget)
//        {
//            stopAllActions = portalTarget.PauseAllActions();
//        }

//        using (stopAllActions)
//        {
//            var method = MethodForOperation(operation, criteria.Select(c => c.GetType()).ToArray());

//            if (method == null)
//            {
//                return false;
//            }

//            var parameters = method.GetParameters().ToList();
//            var parameterValues = new object[parameters.Count()];

//            if (parameters.Count == 1 && parameters[0].ParameterType == typeof(object[]))
//            {
//                parameterValues = [criteria];
//            }
//            else
//            {


//                var criteriaE = criteria.GetEnumerator();

//                for (var i = 0; i < parameterValues.Length; i++)
//                {
//                    if (criteriaE.MoveNext())
//                    {
//                        // Use up the criteria values first
//                        // Assume MethodForOperation got the types right
//                        parameterValues[i] = criteriaE.Current;
//                    }
//                    else
//                    {
//                        var parameter = parameters[i];
//                        var pv = Scope.GetService(parameter.ParameterType);
//                        if (pv != null)
//                        {
//                            parameterValues[i] = pv;
//                        }
//                    }
//                }
//            }
//            var result = method.Invoke(target, parameterValues);

//            if (method.ReturnType == typeof(Task))
//            {
//                await (Task)result;
//            }

//        }

//        PostOperation(target, operation);

//        return true;
//    }

//    protected virtual void PostOperation(object target, DataMapperMethod operation)
//    {
//        var editTarget = target as IDataMapperEditTarget;
//        if (editTarget != null)
//        {


//            switch (operation)
//            {
//                case DataMapperMethod.Create:
//                    editTarget.MarkNew();
//                    break;
//                case DataMapperMethod.CreateChild:
//                    editTarget.MarkAsChild();
//                    editTarget.MarkNew();
//                    break;
//                case DataMapperMethod.Fetch:
//                    break;
//                case DataMapperMethod.FetchChild:
//                    editTarget.MarkAsChild();
//                    break;
//                case DataMapperMethod.Delete:
//                    break;
//                case DataMapperMethod.DeleteChild:
//                    break;
//                case DataMapperMethod.Insert:
//                case DataMapperMethod.InsertChild:
//                case DataMapperMethod.Update:
//                case DataMapperMethod.UpdateChild:
//                    editTarget.MarkUnmodified();
//                    editTarget.MarkOld();
//                    break;
//                default:
//                    break;
//            }
//        }
//    }

//    public async Task<object> HandlePortalRequest(RemoteRequest portalRequest)
//    {
//        Debug.Assert(portalRequest.Target != null, "PortalRequest.Target is null");
//        Debug.Assert(!string.IsNullOrEmpty(portalRequest.Target.AssemblyType), "PortalRequest.Target.Type is null");

//        object? target = null;

//        var request = jsonSerializer.FromDataMapperRequest(portalRequest);

//        if (((int)portalRequest.DataMapperOperation & (int)DataMapperMethodType.Read) == (int)DataMapperMethodType.Read)
//        {
//            Debug.Assert(string.IsNullOrEmpty(portalRequest.Target.Json), "PortalRequest.Target.Json should not be defined for PortalOperationType.Create");
//            target = Scope.GetRequiredService(INeatooJsonSerializer.ToType(portalRequest.Target.AssemblyType)) ?? throw new InvalidOperationException("Type is not an IPortalTarget");
//        }
//        else
//        {
//            target = request.target;
//        }


//        if (((int)portalRequest.DataMapperOperation & (int)DataMapperMethodType.Read) == (int)DataMapperMethodType.Read
//            && portalRequest.Criteria != null)
//        {
//            var criteria = request.criteria;

//            var success = await TryCallOperation(target, portalRequest.DataMapperOperation, criteria);

//            if (!success)
//            {
//                throw new Exception($"Failed on Server - Operation {portalRequest.DataMapperOperation.ToString()} with criteria {string.Join(',', criteria.Select(c => c.GetType().FullName))} not found");
//            }
//        }
//        else
//        {
//            var success = await TryCallOperation(target, portalRequest.DataMapperOperation);

//            if (!success && !((((int)portalRequest.DataMapperOperation) & ((int)DataMapperMethodType.Read)) == ((int)DataMapperMethodType.Read)))
//            {
//                throw new Exception($"Failed on Server - Operation {portalRequest.DataMapperOperation.ToString()} not found on {target.GetType().FullName}");
//            }
//        }



//        return target;
//    }

//}


//[Serializable]
//public class OperationMethodException : Exception
//{
//    public OperationMethodException() { }
//    public OperationMethodException(string message) : base(message) { }
//    public OperationMethodException(string message, Exception inner) : base(message, inner) { }
//    protected OperationMethodException(
//      System.Runtime.Serialization.SerializationInfo info,
//      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
//}
