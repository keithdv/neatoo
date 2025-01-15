using Neatoo.Portal;
using Moq;
using System.Threading.Tasks;
using System;

namespace Neatoo.UnitTest
{
    public class MockReadWritePortalChild<T> : IReadWritePortalChild<T>
        where T : IEditMetaProperties
    {
        public MockReadWritePortalChild()
        {
            MockPortal = new Mock<IReadWritePortalChild<T>>(MockBehavior.Strict);
        }

        public Mock<IReadWritePortalChild<T>> MockPortal { get; }

        public Task<T> CreateChild()
        {
            return MockPortal.Object.CreateChild();
        }
        public Task<T> FetchChild()
        {
            return MockPortal.Object.FetchChild();
        }
        public Task<T> CreateChild(object[] criteria)
        {
            return MockPortal.Object.CreateChild(criteria);
        }
        public Task<T> FetchChild(object[] criteria)
        {
            return MockPortal.Object.FetchChild(criteria);
        }
        public Task<T> UpdateChild(T target)
        {
            return MockPortal.Object.UpdateChild(target);
        }

        public Task<T> UpdateChild(T target, params object[] criteria)
        {
            return MockPortal.Object.UpdateChild(target, criteria);
        }
    }
}
