using Neatoo.Portal;
using Moq;
using System.Threading.Tasks;

namespace Neatoo.UnitTest;

public class MockReadWritePortal<T> : IReadWritePortal<T>
    where T : IEditMetaProperties
{
    public MockReadWritePortal()
    {
        MockPortal = new Mock<IReadWritePortal<T>>(MockBehavior.Strict);
    }

    public Mock<IReadWritePortal<T>> MockPortal { get; }

    public Task<T> Create()
    {
        return MockPortal.Object.Create();
    }

    public Task<T> Create(object[] criteria)
    {
        return MockPortal.Object.Create(criteria);
    }

    public Task<T> Fetch()
    {
        return MockPortal.Object.Fetch();
    }

    public Task<T> Fetch(object[] criteria)
    {
        return MockPortal.Object.Fetch(criteria);
    }

    public Task<T> Update(T target)
    {
        return MockPortal.Object.Update(target);
    }

    public Task<T> Update(T target, params object[] criteria)
    {
        return MockPortal.Object.Update(target, criteria);
    }
}
