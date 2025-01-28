using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neatoo.Portal;
using Neatoo.UnitTest.Objects;
using System;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.ObjectPortal
{

    [TestClass]
    public class ReadWritePortalListTests
    {
        private ILifetimeScope scope = AutofacContainer.GetLifetimeScope(true);
        private IReadWritePortal<IEditObjectList> portal;
        private IEditObjectList editObjectList;

        [TestInitialize]
        public void TestInitialize()
        {
            portal = scope.Resolve<IReadWritePortal<IEditObjectList>>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // Make sure only what  is expected to be called was called
            Assert.IsNotNull(editObjectList);
            scope.Dispose();
        }

        [TestMethod]
        public async Task ReadWritePortalList_Create()
        {
            editObjectList = await portal.Create();
            Assert.IsTrue(editObjectList.CreateCalled);
            Assert.IsTrue(editObjectList.IsNew);
            Assert.IsFalse(editObjectList.IsChild);
        }

        [TestMethod]
        public async Task ReadWritePortalList_CreateGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            editObjectList = await portal.Create(crit);
            Assert.AreEqual(crit, editObjectList.GuidCriteria);
            Assert.IsTrue(editObjectList.CreateCalled);
        }

        [TestMethod]
        public async Task ReadWritePortalList_CreateIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            editObjectList = await portal.Create(crit);
            Assert.AreEqual(crit, editObjectList.IntCriteria);
            Assert.IsTrue(editObjectList.CreateCalled);
        }


        [TestMethod]
        public async Task ReadWritePortalList_Fetch()
        {
            editObjectList = await portal.Fetch();
            Assert.IsTrue(editObjectList.ID.HasValue);
            Assert.IsTrue(editObjectList.FetchCalled);
            Assert.IsFalse(editObjectList.IsNew);
            Assert.IsFalse(editObjectList.IsChild);
            Assert.IsFalse(editObjectList.IsModified);
            Assert.IsFalse(editObjectList.IsSelfModified);
            Assert.IsFalse(editObjectList.IsBusy);
            Assert.IsFalse(editObjectList.IsSelfBusy);
        }

        [TestMethod]
        public async Task ReadWritePortalList_FetchGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            editObjectList = await portal.Fetch(crit);
            Assert.AreEqual(crit, editObjectList.GuidCriteria);
            Assert.IsTrue(editObjectList.FetchCalled);
        }

        [TestMethod]
        public async Task ReadWritePortalList_FetchIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            editObjectList = await portal.Fetch(crit);
            Assert.AreEqual(crit, editObjectList.IntCriteria);
            Assert.IsTrue(editObjectList.FetchCalled);
        }



        [TestMethod]
        public async Task ReadWritePortalList_Update()
        {
            editObjectList = await portal.Fetch();
            var id = Guid.NewGuid();
            editObjectList.ID = Guid.NewGuid();
            await portal.Update(editObjectList);
            Assert.AreNotEqual(id, editObjectList.ID);
            Assert.IsTrue(editObjectList.UpdateCalled);
            Assert.IsFalse(editObjectList.IsNew);
            Assert.IsFalse(editObjectList.IsChild);
            Assert.IsFalse(editObjectList.IsModified);
        }



        [TestMethod]
        public async Task ReadWritePortalList_Insert()
        {
            editObjectList = await portal.Create();
            editObjectList.ID = Guid.Empty;
            await portal.Update(editObjectList);
            Assert.AreNotEqual(Guid.Empty, editObjectList.ID);
            Assert.IsTrue(editObjectList.InsertCalled);
            Assert.IsFalse(editObjectList.IsNew);
            Assert.IsFalse(editObjectList.IsChild);
            Assert.IsFalse(editObjectList.IsModified);
        }
    }
}
