using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.ObjectPortal;


[TestClass]
public class ReadWritePortalListTests
{
    private IServiceScope scope = UnitTestServices.GetLifetimeScope(true);
    private INeatooPortal<IEditObjectList> portal;
    private IEditObjectList editObjectList;

    [TestInitialize]
    public void TestInitialize()
    {
        portal = scope.GetRequiredService<INeatooPortal<IEditObjectList>>();
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
        Assert.IsFalse(editObjectList.IsNew);
        Assert.IsFalse(editObjectList.IsChild);
    }

    [TestMethod]
    public async Task ReadWritePortalList_CreateGuidCriteriaCalled()
    {
        var crit = Guid.NewGuid();
        editObjectList = await portal.Create(crit);
        Assert.IsTrue(editObjectList.CreateCalled);
    }

    [TestMethod]
    public async Task ReadWritePortalList_CreateIntCriteriaCalled()
    {
        int crit = DateTime.Now.Millisecond;
        editObjectList = await portal.Create(crit);
        Assert.IsTrue(editObjectList.CreateCalled);
    }


    [TestMethod]
    public async Task ReadWritePortalList_Fetch()
    {
        editObjectList = await portal.Fetch();
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
        Assert.IsTrue(editObjectList.FetchCalled);
    }

    [TestMethod]
    public async Task ReadWritePortalList_FetchIntCriteriaCalled()
    {
        int crit = DateTime.Now.Millisecond;
        editObjectList = await portal.Fetch(crit);
        Assert.IsTrue(editObjectList.FetchCalled);
    }



    [TestMethod]
    public async Task ReadWritePortalList_Update()
    {
        editObjectList = await portal.Fetch();
        await portal.Update(editObjectList);
        Assert.IsTrue(editObjectList.UpdateCalled);
        Assert.IsFalse(editObjectList.IsNew);
        Assert.IsFalse(editObjectList.IsChild);
        Assert.IsFalse(editObjectList.IsModified);
    }



    [TestMethod]
    public async Task ReadWritePortalList_Insert()
    {
        editObjectList = await portal.Create();
        await portal.Update(editObjectList);
        Assert.IsTrue(editObjectList.UpdateCalled);
        Assert.IsFalse(editObjectList.IsNew);
        Assert.IsFalse(editObjectList.IsChild);
        Assert.IsFalse(editObjectList.IsModified);
    }
}
