//using Microsoft.Extensions.DependencyInjection;
//using Neatoo.Internal;
//using System;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Neatoo.Portal.Internal;

///// <summary>
///// Provide Authorization Check
///// </summary>
///// <typeparam name="T"></typeparam>
//public class NeatooPortalHost<T> : INeatooPortal<T>
//    where T: notnull
//{

//    protected IServiceProvider Scope { get; }
//    public NeatooPortalHost(IServiceProvider scope)
//    {
//        Scope = scope;
//    }

//    public Task<T> Create()
//    {
//        return CallReadOperationMethod(DataMapperMethod.Create, false);
//    }

//    public Task<T> Fetch()
//    {
//        return CallReadOperationMethod(DataMapperMethod.Fetch, true);
//    }

//    public Task<T> CreateChild()
//    {
//        return CallReadOperationMethod(DataMapperMethod.CreateChild, false);
//    }

//    public Task<T> FetchChild()
//    {
//        return CallReadOperationMethod(DataMapperMethod.FetchChild, true);
//    }

//    public Task<T> Create(object[] criteria)
//    {
//        return CallReadOperationMethod(DataMapperMethod.Create, criteria);
//    }

//    public Task<T> Fetch(object[] criteria)
//    {
//        return CallReadOperationMethod(DataMapperMethod.Fetch, criteria);
//    }

//    public Task<T> CreateChild(object[] criteria)
//    {
//        return CallReadOperationMethod(DataMapperMethod.CreateChild, criteria);
//    }

//    public Task<T> FetchChild(object[] criteria)
//    {
//        return CallReadOperationMethod(DataMapperMethod.FetchChild, criteria);
//    }

//    public async Task<T> Update<T1>(T1 target) where T1 : T, IEditMetaProperties
//    {
//        await CallWriteOperationMethod(target);

//        return target;
//    }

//    public async Task<T> Update<T1>(T1 target, params object[] criteria) where T1 : T, IEditMetaProperties
//    {
//        await CallWriteOperationMethod(target, criteria);

//        return target;
//    }

//    public async Task<T> CallReadOperationMethod(DataMapperMethod operation, bool throwException)
//    {
//        var target = Scope.GetRequiredService<T>();
//        await CallOperationMethod(target, operation, throwException);
//        if(target is IDataMapperTarget editTarget)
//        {
//            await editTarget.PostPortalConstruct();
//        }
//        return target;
//    }

//    public Task CallWriteOperationMethod<E>(E target) where E : IEditMetaProperties
//    {
//        var operation = target.GetDataMapperOperation();

//        if(operation == null)
//        {
//            return Task.CompletedTask;
//        }

//        return CallOperationMethod(target, operation.Value);
//    }

//    public async Task CallOperationMethod<E>(E target, DataMapperMethod operation, bool throwException = true)
//    {
//        // Concrete type can vary since an interface can have more than one implementation
//        // So need to load the actual concrete type
//        var operationManager = (IDataMapper)Scope.GetRequiredService(typeof(IDataMapper<>).MakeGenericType(target.GetType()));
     
//        var success = await operationManager.TryCallOperation(target, operation);

//        if (!success && throwException)
//        {
//            throw new DataMapperMethodCallFailedException($"{operation.ToString()} method with no criteria not found on {target.GetType().FullName}.");
//        }
//    }

//    public async Task<T> CallReadOperationMethod(DataMapperMethod operation, object[] criteria)
//    {
//        var target = Scope.GetRequiredService<T>();
//        await CallOperationMethod(target, operation, criteria);
//        if (target is IDataMapperTarget editTarget)
//        {
//            await editTarget.PostPortalConstruct();
//        }
//        return target;
//    }

//    public Task CallWriteOperationMethod<E>(E target, object[] criteria) where E : IEditMetaProperties
//    {
//        var operation = target.GetDataMapperOperation();

//        if (operation == null)
//        {
//            return Task.CompletedTask;
//        }

//        return CallOperationMethod(target, operation.Value, criteria);        
//    }

//    public async Task CallOperationMethod<E>(E target, DataMapperMethod operation, object[] criteria)
//    {
//        if (target == null) { throw new ArgumentNullException(nameof(target)); }
//        if (criteria == null) { throw new ArgumentNullException(nameof(criteria)); }

//        // Concrete type can vary since an interface can have more than one implementation
//        // So need to load the actual concrete type
//        var operationManager = (IDataMapper)Scope.GetRequiredService(typeof(IDataMapper<>).MakeGenericType(target.GetType()));

//        var success = await operationManager.TryCallOperation(target, operation, criteria);

//        if (!success)
//        {
//            throw new DataMapperMethodCallFailedException($"{operation.ToString()} method on {target.GetType().FullName} with criteria [{string.Join(", ", criteria.Select(x => x.GetType().FullName))}] not found.");
//        }
//    }

//    public Task<T> Execute<T1>() where T1 : IRemoteMethodPortal<T>, T
//    {
//        throw new NotImplementedException();
//    }

//    public Task<T> Execute<T1, P>(P parameters)
//        where T1 : IRemoteMethodPortal<T, P>, T
//        where P : notnull
//    {
//        throw new NotImplementedException();
//    }
//}


//[Serializable]
//public class DataMapperMethodCallFailedException : Exception
//{
//    public DataMapperMethodCallFailedException() { }
//    public DataMapperMethodCallFailedException(string message) : base(message) { }
//    public DataMapperMethodCallFailedException(string message, Exception inner) : base(message, inner) { }
//}
