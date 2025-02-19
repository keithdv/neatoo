using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.ObjectPortal;


[TestClass]
public class ReadWritePortalTests
{
    private IServiceScope scope = UnitTestServices.GetLifetimeScope(true);
    private EditObjectFactory portal;
    private IEditObject editObject;

    [TestInitialize]
    public void TestInitialize()
    {
        portal = scope.GetRequiredService<EditObjectFactory>();
    }

    [TestCleanup]
    public void TestCleanup()
    {
        // Make sure only what  is expected to be called was called
        Assert.IsNotNull(editObject);
        scope.Dispose();
    }

    [TestMethod]
    public async Task ReadWritePortal_Create()
    {
        editObject = await portal.Create();
        Assert.IsTrue(editObject.CreateCalled);
        Assert.IsTrue(editObject.IsNew);
        Assert.IsFalse(editObject.IsChild);
    }

    [TestMethod]
    public async Task ReadWritePortal_CreateGuidCriteriaCalled()
    {
        var crit = Guid.NewGuid();
        editObject = await portal.Create(crit);
        Assert.AreEqual(crit, editObject.GuidCriteria);
        Assert.IsTrue(editObject.CreateCalled);
    }

    [TestMethod]
    public async Task ReadWritePortal_CreateIntCriteriaCalled()
    {
        int crit = DateTime.Now.Millisecond;
        editObject = await portal.Create(crit);
        Assert.AreEqual(crit, editObject.IntCriteria);
        Assert.IsTrue(editObject.CreateCalled);
    }


    [TestMethod]
    public async Task ReadWritePortal_Fetch()
    {
        editObject = await portal.Fetch();
        Assert.IsTrue(editObject.ID.HasValue);
        Assert.IsTrue(editObject.FetchCalled);
        Assert.IsFalse(editObject.IsNew);
        Assert.IsFalse(editObject.IsChild);
        Assert.IsFalse(editObject.IsModified);
        Assert.IsFalse(editObject.IsSelfModified);
        Assert.IsFalse(editObject.IsBusy);
        Assert.IsFalse(editObject.IsSelfBusy);
    }

    [TestMethod]
    public async Task ReadWritePortal_FetchGuidCriteriaCalled()
    {
        var crit = Guid.NewGuid();
        editObject = await portal.Fetch(crit);
        Assert.AreEqual(crit, editObject.GuidCriteria);
        Assert.IsTrue(editObject.FetchCalled);
    }

    [TestMethod]
    public async Task ReadWritePortal_FetchIntCriteriaCalled()
    {
        int crit = DateTime.Now.Millisecond;
        editObject = await portal.Fetch(crit);
        Assert.AreEqual(crit, editObject.IntCriteria);
        Assert.IsTrue(editObject.FetchCalled);
    }



    [TestMethod]
    public async Task ReadWritePortal_Update()
    {
        editObject = await portal.Fetch();
        var id = Guid.NewGuid();
        editObject.ID = Guid.NewGuid();
        await portal.Save(editObject);
        Assert.AreNotEqual(id, editObject.ID);
        Assert.IsTrue(editObject.UpdateCalled);
        Assert.IsFalse(editObject.IsNew);
        Assert.IsFalse(editObject.IsChild);
        Assert.IsFalse(editObject.IsModified);
    }



    [TestMethod]
    public async Task ReadWritePortal_Insert()
    {
        editObject = await portal.Create();
        editObject.ID = Guid.Empty;
        await portal.Save(editObject);
        Assert.AreNotEqual(Guid.Empty, editObject.ID);
        Assert.IsTrue(editObject.InsertCalled);
        Assert.IsFalse(editObject.IsNew);
        Assert.IsFalse(editObject.IsChild);
        Assert.IsFalse(editObject.IsModified);
    }


    [TestMethod]
    public async Task ReadWritePortal_Delete()
    {
        editObject = await portal.Fetch();
        editObject.Delete();
        await portal.Save(editObject);
        Assert.IsTrue(editObject.DeleteCalled);
    }


}
