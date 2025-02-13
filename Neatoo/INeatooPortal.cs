using Neatoo.Internal;
using Neatoo.Portal;
using System.Threading.Tasks;

namespace Neatoo;

public delegate Task<RemoteDataMapperResponse> ServerHandlePortalRequest(RemoteDataMapperRequest portalRequest);

// Note: A non generic IObjectPortal with generic functions
// is a service locator pattern which is bad!!

public interface INeatooPortal<T>
{
    Task<T> Create();
    Task<T> Create(params object[] criteria);
    Task<T> Fetch();
    Task<T> Fetch(params object[] criteria);
    Task<T> CreateChild();
    Task<T> CreateChild(params object[] criteria);
    Task<T> FetchChild();
    Task<T> FetchChild(params object[] criteria);
    Task<T> Update<T1>(T1 target) where T1 : T, IEditMetaProperties;
    Task<T> Update<T1>(T1 target, params object[] criteria) where T1 : T, IEditMetaProperties;
}