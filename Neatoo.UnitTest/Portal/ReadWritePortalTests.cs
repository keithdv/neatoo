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
    public void ReadWritePortal_Create()
    {
        editObject = portal.Create();
        Assert.IsTrue(editObject.CreateCalled);
        Assert.IsTrue(editObject.IsNew);
        Assert.IsFalse(editObject.IsChild);
    }

    [TestMethod]
    public void ReadWritePortal_CreateGuidCriteriaCalled()
    {
        var crit = Guid.NewGuid();
        editObject = portal.Create(crit);
        Assert.AreEqual(crit, editObject.GuidCriteria);
        Assert.IsTrue(editObject.CreateCalled);
    }

    [TestMethod]
    public async Task ReadWritePortal_CreateIntCriteriaCalled()
    {
        int crit = DateTime.Now.Millisecond;
        editObject = await portal.CreateAsync(crit);
        Assert.AreEqual(crit, editObject.IntCriteria);
        Assert.IsTrue(editObject.CreateCalled);
    }


    [TestMethod]
    public void ReadWritePortal_Fetch()
    {
        editObject = portal.Fetch();
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
    public void ReadWritePortal_FetchGuidCriteriaCalled()
    {
        var crit = Guid.NewGuid();
        editObject = portal.Fetch(crit);
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
        editObject = portal.Fetch();
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
    public void ReadWritePortal_Insert()
    {
        editObject = portal.Create();
        editObject.ID = Guid.Empty;
        portal.Save(editObject);
        Assert.AreNotEqual(Guid.Empty, editObject.ID);
        Assert.IsTrue(editObject.InsertCalled);
        Assert.IsFalse(editObject.IsNew);
        Assert.IsFalse(editObject.IsChild);
        Assert.IsFalse(editObject.IsModified);
    }


    [TestMethod]
    public void ReadWritePortal_Delete()
    {
        editObject = portal.Fetch();
        editObject.Delete();
        portal.Save(editObject);
        Assert.IsTrue(editObject.DeleteCalled);
    }


}
