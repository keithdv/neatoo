using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neatoo.AuthorizationRules;
using Neatoo.Portal;
using Neatoo.UnitTest.Objects;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.ObjectPortal
{

    [TestClass]
    public class ReadPortalListTests
    {
        private IServiceScope scope = UnitTestServices.GetLifetimeScope(true);
        private IReadPortal<IBaseObjectList> portal;
        private IBaseObjectList list;

        [TestInitialize]
        public void TestInitialize()
        {
            portal = scope.GetRequiredService<IReadPortal<IBaseObjectList>>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // Make sure only what  is expected to be called was called
            Assert.IsNotNull(list);
            scope.Dispose();
        }

        [TestMethod]
        public async Task ReadPortalList_Create()
        {
            list = await portal.Create();
            Assert.IsTrue(list.CreateCalled);
            Assert.IsTrue(list.Single().CreateChildCalled);
        }

        [TestMethod]
        public async Task ReadPortalList_CreateGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            list = await portal.Create(crit);
            Assert.AreEqual(crit, list.GuidCriteria);
            Assert.AreEqual(crit, list.Single().GuidCriteria);
        }

        [TestMethod]
        public async Task ReadPortalList_CreateIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            list = await portal.Create(crit);
            Assert.AreEqual(crit, list.IntCriteria);
            Assert.AreEqual(crit, list.Single().IntCriteria);
        }

        [TestMethod]
        public async Task ReadPortalList_Fetch()
        {
            list = await portal.Fetch();
            Assert.IsTrue(list.FetchCalled);
            Assert.IsTrue(list.Single().FetchChildCalled);
        }

        [TestMethod]
        public async Task ReadPortalList_FetchGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            list = await portal.Fetch(crit);
            Assert.AreEqual(crit, list.GuidCriteria);
            Assert.AreEqual(crit, list.Single().GuidCriteria);
        }

        [TestMethod]
        public async Task ReadPortalList_FetchIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            list = await portal.Fetch(crit);
            Assert.AreEqual(crit, list.IntCriteria);
            Assert.AreEqual(crit, list.Single().IntCriteria);
        }

    }
}
