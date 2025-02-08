using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neatoo.Portal;
using System;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.ObjectPortal
{

    [TestClass]
    public class ReadWritePortalChildTests
    {
        private IServiceScope scope = UnitTestServices.GetLifetimeScope(true);
        private IReadWritePortalChild<IEditObject> portal;
        private IEditObject editObject;

        [TestInitialize]
        public void TestInitialize()
        {
            portal = scope.GetRequiredService<IReadWritePortalChild<IEditObject>>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // Make sure only what  is expected to be called was called
            Assert.IsNotNull(editObject);
            scope.Dispose();
        }

        [TestMethod]
        public async Task ReadWritePortalChild_CreateChild()
        {
            editObject = await portal.CreateChild();
            Assert.IsTrue(editObject.CreateChildCalled);
            Assert.IsTrue(editObject.IsNew);
            Assert.IsTrue(editObject.IsChild);
        }

        [TestMethod]
        public async Task ReadWritePortalChild_CreateChildGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            editObject = await portal.CreateChild(crit);
            Assert.IsTrue(editObject.CreateChildCalled);
            Assert.AreEqual(crit, editObject.GuidCriteria);
        }

        [TestMethod]
        public async Task ReadWritePortalChild_CreateChildIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            editObject = await portal.CreateChild(crit);
            Assert.IsTrue(editObject.CreateChildCalled);
            Assert.AreEqual(crit, editObject.IntCriteria);
        }




        [TestMethod]
        public async Task ReadWritePortalChild_FetchChild()
        {
            editObject = await portal.FetchChild();
            Assert.IsTrue(editObject.FetchChildCalled);
            Assert.IsFalse(editObject.IsNew);
            Assert.IsTrue(editObject.IsChild);
        }

        [TestMethod]
        public async Task ReadWritePortalChild_FetchChildGuidCriteriaCalled()
        {
            var crit = Guid.NewGuid();
            editObject = await portal.FetchChild(crit);
            Assert.IsTrue(editObject.FetchChildCalled);
            Assert.AreEqual(crit, editObject.GuidCriteria);
        }

        [TestMethod]
        public async Task ReadWritePortalChild_FetchChildIntCriteriaCalled()
        {
            int crit = DateTime.Now.Millisecond;
            editObject = await portal.FetchChild(crit);
            Assert.IsTrue(editObject.FetchChildCalled);
            Assert.AreEqual(crit, editObject.IntCriteria);
        }

        [TestMethod]
        public async Task ReadWritePortalChild_UpdateChild()
        {
            editObject = await portal.FetchChild();
            editObject.ID = Guid.NewGuid();
            await portal.UpdateChild(editObject);
            Assert.IsTrue(editObject.UpdateChildCalled);
            Assert.IsFalse(editObject.IsNew);
            Assert.IsTrue(editObject.IsChild);
        }

        [TestMethod]
        public async Task ReadWritePortalChild_InsertChild()
        {
            editObject = await portal.CreateChild();
            await portal.UpdateChild(editObject);
            Assert.IsTrue(editObject.InsertChildCalled);
            Assert.IsFalse(editObject.IsNew);
            Assert.IsTrue(editObject.IsChild);
        }



        [TestMethod]
        public async Task ReadWritePortalChild_DeleteChild()
        {
            editObject = await portal.FetchChild();
            editObject.Delete();
            await portal.UpdateChild(editObject);
            Assert.IsTrue(editObject.DeleteChildCalled);
        }

        [TestMethod]
        public async Task ReadWritePortalChild_DeleteChild_Create()
        {
            // Want it to do nothing
            editObject = await portal.CreateChild();
            editObject.Delete();
            await portal.UpdateChild(editObject);
            Assert.IsFalse(editObject.DeleteChildCalled);
        }


    }
}
