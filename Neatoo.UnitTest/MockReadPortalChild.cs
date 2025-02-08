using Neatoo.Portal;
using Moq;
using System.Threading.Tasks;

namespace Neatoo.UnitTest
{
    public class MockReadPortalChild<T> : IReadPortalChild<T>
    {
        public MockReadPortalChild()
        {
            MockPortal = new Mock<IReadPortalChild<T>>(MockBehavior.Strict);
        }

        public Mock<IReadPortalChild<T>> MockPortal { get; }

        public Task<T> CreateChild()
        {
            return MockPortal.Object.CreateChild();
        }

        public Task<T> CreateChild(object[] criteria)
        {
            return MockPortal.Object.CreateChild(criteria);
        }

        public Task<T> FetchChild()
        {
            return MockPortal.Object.FetchChild();
        }

        public Task<T> FetchChild(object[] criteria)
        {
            return MockPortal.Object.FetchChild(criteria);
        }
    }
}
