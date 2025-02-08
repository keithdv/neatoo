using Neatoo.Portal;
using Moq;
using System.Threading.Tasks;

namespace Neatoo.UnitTest
{
    public class MockReadPortal<T> : IReadPortal<T>
        where T : IPortalTarget
    {
        public MockReadPortal()
        {
            MockPortal = new Mock<IReadPortal<T>>(MockBehavior.Strict);
        }

        public Mock<IReadPortal<T>> MockPortal { get; }

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
  
    }
}
