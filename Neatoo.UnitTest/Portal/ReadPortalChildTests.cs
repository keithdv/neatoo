using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neatoo.Portal;
using Neatoo.UnitTest.Objects;
using System;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.ObjectPortal
{

    [TestClass]
    public class ReadPortalChildTests
    {
        private ILifetimeScope scope = AutofacContainer.GetLifetimeScope(true);
        private IReadPortalChild<IBaseObject> portal;
        private IBaseObject domainObject;

        [TestInitialize]
        public void TestInitialize()
        {
            portal = scope.Resolve<IReadPortalChild<IBaseObject>>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // Make sure only what  is expected to be called was called
            Assert.IsNotNull(domainObject);
            scope.Dispose();
        }

        [TestMethod]
        public async Task ReadPortalChild_CreateChild()
        {
            domainObject = await portal.CreateChild();
            Assert.IsTrue(domainObject.CreateChildCalled);
        }

        [TestMethod]
        public async Task ReadPortalChild_CreateChildGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            domainObject = await portal.CreateChild(crit);
            Assert.AreEqual(crit, domainObject.GuidCriteria);
        }

        [TestMethod]
        public async Task ReadPortalChild_CreateChildIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            domainObject = await portal.CreateChild(crit);
            Assert.AreEqual(crit, domainObject.IntCriteria);
        }

        [TestMethod]
        public async Task ReadPortalChild_FetchChild()
        {
            domainObject = await portal.FetchChild();
            Assert.IsTrue(domainObject.FetchChildCalled);
        }

        [TestMethod]
        public async Task ReadPortalChild_FetchChildGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            domainObject = await portal.FetchChild(crit);
            Assert.AreEqual(crit, domainObject.GuidCriteria);
        }

        [TestMethod]
        public async Task ReadPortalChild_FetchChildIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            domainObject = await portal.FetchChild(crit);
            Assert.AreEqual(crit, domainObject.IntCriteria);
        }

    }
}
