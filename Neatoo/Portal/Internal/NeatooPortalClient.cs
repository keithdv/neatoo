using Neatoo.Internal;
using System.Threading.Tasks;

namespace Neatoo.Portal.Internal;


public delegate Task<RemoteDataMapperResponse> RequestFromServerDelegate(RemoteDataMapperRequest request);

public class NeatooPortalClient<T> : INeatooPortal<T>
    where T : notnull
{
    private readonly INeatooJsonSerializer portalJsonSerializer;
    private readonly RequestFromServerDelegate requestFromServerDelegate;

    public NeatooPortalClient(INeatooJsonSerializer portalJsonSerializer, RequestFromServerDelegate requestFromServerDelegate)
    {
        this.portalJsonSerializer = portalJsonSerializer;
        this.requestFromServerDelegate = requestFromServerDelegate;
    }

    public async Task<T> Create()
    {
        return await RequestFromServer(DataMapperMethod.Create);
    }
    public async Task<T> Fetch()
    {
        return await RequestFromServer(DataMapperMethod.Fetch);
    }
    public async Task<T> CreateChild()
    {
        return await RequestFromServer(DataMapperMethod.CreateChild);
    }
    public async Task<T> FetchChild()
    {
        return await RequestFromServer(DataMapperMethod.FetchChild);
    }

    public Task<T> Create(object[] criteria)
    {
        return RequestFromServer(DataMapperMethod.Create, criteria);
    }

    public Task<T> Fetch(object[] criteria)
    {
        return RequestFromServer(DataMapperMethod.Fetch, criteria);
    }

    public Task<T> CreateChild(object[] criteria0)
    {
        return RequestFromServer(DataMapperMethod.CreateChild, criteria0);
    }

    public Task<T> FetchChild(object[] criteria0)
    {
        return RequestFromServer(DataMapperMethod.FetchChild, criteria0);
    }

    protected async Task<T> RequestFromServer(RemoteDataMapperRequest request)
    {
        var result = await this.requestFromServerDelegate(request);

        return (T) portalJsonSerializer.FromPortalResponse(result);
    }

    protected Task<T> RequestFromServer(DataMapperMethod portalOperation)
    {
        var portalRequest = portalJsonSerializer.ToDataMapperHostRequest(portalOperation, typeof(T));

        return RequestFromServer(portalRequest);
    }

    protected Task<T> RequestFromServer(DataMapperMethod portalOperation, object[] criteria)
    {
        var portalRequest = portalJsonSerializer.ToDataMapperHostRequest(portalOperation, typeof(T), criteria);     

        return RequestFromServer(portalRequest);
    }

    protected Task<T> RequestFromServer(DataMapperMethod portalOperation, T target)
    {
        var portalRequest = portalJsonSerializer.ToDataMapperHostRequest(portalOperation, target);

        return RequestFromServer(portalRequest);
    }

    protected Task<T> RequestFromServer(DataMapperMethod portalOperation, T target, object[] criteria)
    {
        var portalRequest = portalJsonSerializer.ToDataMapperHostRequest(portalOperation, target, criteria);

        return RequestFromServer(portalRequest);
    }


    protected Task<T?> RequestFromServer<T1>(T1 target, object[]? criteria = null) where T1 : T, IEditMetaProperties
    {
        var operation = target.GetDataMapperOperation();
        if (operation == null) { return Task.FromResult<T?>(default); }
        var portalRequest = portalJsonSerializer.ToDataMapperHostRequest(operation.Value, target);
        return RequestFromServer(portalRequest);
    }


    public Task<T?> Update<T1>(T1 target) where T1 : T, IEditMetaProperties
    {
        return RequestFromServer(target);
    }

    public Task<T?> Update<T1>(T1 target, params object[] criteria) where T1 : T, IEditMetaProperties
    {
        return RequestFromServer(target, criteria);
    }
}
