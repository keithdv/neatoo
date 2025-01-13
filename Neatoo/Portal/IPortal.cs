using System.Threading.Tasks;

namespace Neatoo.Portal
{

    // Note: A non generic IObjectPortal with generic functions
    // is a service locator pattern which is bad!!

    public interface IReadPortal<T>
    {
        Task<T> Create();
        Task<T> Create(params object[] criteria);

        Task<T> Fetch();
        Task<T> Fetch(params object[] criteria);

    }

    public interface IReadPortalChild<T>
    {
        Task<T> CreateChild();
        Task<T> CreateChild(params object[] criteria);
        Task<T> FetchChild();
        Task<T> FetchChild(params object[] criteria);
    }

    public interface IReadWritePortal<T> : IReadPortal<T>
    {
        Task Update(T target);
    }

    public interface IReadWritePortalChild<T> : IReadPortalChild<T>
    {
        Task UpdateChild(T target);

    }

}