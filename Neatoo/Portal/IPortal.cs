using System.Threading.Tasks;

namespace Neatoo.Portal
{

    // Note: A non generic IObjectPortal with generic functions
    // is a service locator pattern which is bad!!

    public interface IReadPortal<T> where T : IPortalTarget
    {
        Task<T> Create();
        Task<T> Create<C0>(C0 criteria0);
        Task<T> Create<C0, C1>(C0 criteria0, C1 criteria1);
        Task<T> Create<C0, C1, C2>(C0 criteria0, C1 criteria1, C2 criteria2);
        Task<T> Create<C0, C1, C2, C3>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3);
        Task<T> Create<C0, C1, C2, C3, C4>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4);
        Task<T> Create<C0, C1, C2, C3, C4, C5>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5);
        Task<T> Create<C0, C1, C2, C3, C4, C5, C6>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6);
        Task<T> Create<C0, C1, C2, C3, C4, C5, C6, C7>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6, C7 criteria7);
        Task<T> Fetch();
        Task<T> Fetch<C0>(C0 criteria0);
        Task<T> Fetch<C0, C1>(C0 criteria0, C1 criteria1);
        Task<T> Fetch<C0, C1, C2>(C0 criteria0, C1 criteria1, C2 criteria2);
        Task<T> Fetch<C0, C1, C2, C3>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3);
        Task<T> Fetch<C0, C1, C2, C3, C4>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4);
        Task<T> Fetch<C0, C1, C2, C3, C4, C5>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5);
        Task<T> Fetch<C0, C1, C2, C3, C4, C5, C6>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6);
        Task<T> Fetch<C0, C1, C2, C3, C4, C5, C6, C7>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6, C7 criteria7);

    }

    public interface IReadPortalChild<T> where T : IPortalTarget
    {
        Task<T> CreateChild();
        Task<T> CreateChild<C0>(C0 criteria0);
        Task<T> CreateChild<C0, C1>(C0 criteria0, C1 criteria1);
        Task<T> CreateChild<C0, C1, C2>(C0 criteria0, C1 criteria1, C2 criteria2);
        Task<T> CreateChild<C0, C1, C2, C3>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3);
        Task<T> CreateChild<C0, C1, C2, C3, C4>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4);
        Task<T> CreateChild<C0, C1, C2, C3, C4, C5>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5);
        Task<T> CreateChild<C0, C1, C2, C3, C4, C5, C6>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6);
        Task<T> CreateChild<C0, C1, C2, C3, C4, C5, C6, C7>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6, C7 criteria7);
        Task<T> FetchChild();
        Task<T> FetchChild<C0>(C0 criteria0);
        Task<T> FetchChild<C0, C1>(C0 criteria0, C1 criteria1);
        Task<T> FetchChild<C0, C1, C2>(C0 criteria0, C1 criteria1, C2 criteria2);
        Task<T> FetchChild<C0, C1, C2, C3>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3);
        Task<T> FetchChild<C0, C1, C2, C3, C4>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4);
        Task<T> FetchChild<C0, C1, C2, C3, C4, C5>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5);
        Task<T> FetchChild<C0, C1, C2, C3, C4, C5, C6>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6);
        Task<T> FetchChild<C0, C1, C2, C3, C4, C5, C6, C7>(C0 criteria0, C1 criteria1, C2 criteria2, C3 criteria3, C4 criteria4, C5 criteria5, C6 criteria6, C7 criteria7);
    }

    public interface IReadWritePortal<T> : IReadPortal<T> where T : IPortalEditTarget, IEditMetaProperties
    {
        Task Update(T target);
    }

    public interface IReadWritePortalChild<T> : IReadPortalChild<T> where T : IPortalEditTarget, IEditMetaProperties
    {
        Task UpdateChild(T target);

    }

}