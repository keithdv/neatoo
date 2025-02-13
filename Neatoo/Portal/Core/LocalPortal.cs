using System;
using System.Threading.Tasks;

namespace Neatoo.Portal.Core;


public interface ILocalReadPortal<T> : IReadPortal<T>, IReadPortalChild<T>
{

}


public delegate Task<PortalResponse> ServerHandlePortalRequest(PortalRequest portalRequest);

public class LocalReadPortal<T> : Portal<T>, ILocalReadPortal<T>
{
    public LocalReadPortal(IServiceProvider scope)
        : base(scope)
    {
    }

    public Task<T> Create()
    {
        return CallReadOperationMethod(PortalOperation.Create, false);
    }
    public Task<T> Fetch()
    {
        return CallReadOperationMethod(PortalOperation.Fetch, true);
    }
    public Task<T> CreateChild()
    {
        return CallReadOperationMethod(PortalOperation.CreateChild, false);
    }
    public Task<T> FetchChild()
    {
        return CallReadOperationMethod(PortalOperation.FetchChild, true);
    }

    public Task<T> Create(object[] criteria)
    {
        return CallReadOperationMethod(PortalOperation.Create, criteria);
    }

    public Task<T> Fetch(object[] criteria)
    {
        return CallReadOperationMethod(PortalOperation.Fetch, criteria);
    }

    public Task<T> CreateChild(object[] criteria)
    {
        return CallReadOperationMethod(PortalOperation.CreateChild, criteria);
    }

    public Task<T> FetchChild(object[] criteria)
    {
        return CallReadOperationMethod(PortalOperation.FetchChild, criteria);
    }
}

public interface ILocalReadWritePortal<T> : ILocalReadPortal<T>, IReadWritePortal<T>, IReadWritePortalChild<T>
    where T : IEditMetaProperties
{
}

public class LocalReadWritePortal<T> : LocalReadPortal<T>, ILocalReadWritePortal<T>
    where T : IEditMetaProperties
{

    public LocalReadWritePortal(IServiceProvider scope)
        : base(scope)
    {
    }

    public async Task<T> Update(T target)
    {
        await CallWriteOperationMethod(target);

        return target;
    }

    public async Task<T> Update(T target, params object[] criteria)
    {
        await CallWriteOperationMethod(target, criteria);

        return target;
    }

    public async Task<T> UpdateChild(T target)
    {
        await CallWriteChildOperationMethod(target);

        return target;
    }
    public async Task<T> UpdateChild(T target, params object[] criteria)
    {
        await CallWriteChildOperationMethod(target, criteria);

        return target;
    }
}



[Serializable]
public class OperationMethodCallFailedException : Exception
{
    public OperationMethodCallFailedException() { }
    public OperationMethodCallFailedException(string message) : base(message) { }
    public OperationMethodCallFailedException(string message, Exception inner) : base(message, inner) { }
}
