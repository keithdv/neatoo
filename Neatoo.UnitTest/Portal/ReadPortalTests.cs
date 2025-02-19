using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.ObjectPortal;


[TestClass]
public class ReadPortalTests
{
    private IServiceScope scope = UnitTestServices.GetLifetimeScope(true);
    private BaseObjectFactory portal;
    private IBaseObject domainObject;

    [TestInitialize]
    public void TestInitialize()
    {
        portal = scope.GetRequiredService<BaseObjectFactory>();
    }

    [TestCleanup]
    public void TestCleanup()
    {
        scope.Dispose();
    }

    [TestMethod]
    public async Task ReadPortal_Create()
    {
        domainObject = await portal.Create();
        Assert.IsTrue(domainObject.CreateCalled);
    }

    [TestMethod]
    public async Task ReadPortal_CreateGuidCriteriaCalled()
    {
        var crit = Guid.NewGuid();
        domainObject = await portal.Create(crit);
        Assert.AreEqual(crit, domainObject.GuidCriteria);
    }

    [TestMethod]
    public async Task ReadPortal_CreateIntCriteriaCalled()
    {
        int crit = DateTime.Now.Millisecond;
        domainObject = await portal.Create(crit);
        Assert.AreEqual(crit, domainObject.IntCriteria);
    }

    [TestMethod]
    public async Task ReadPortal_CreateMultipleCriteriaCalled()
    {
        domainObject = await portal.Create(10, "String");
        CollectionAssert.AreEquivalent(new object[] { 10, "String" }, domainObject.MultipleCriteria);
    }

    [TestMethod]
    public async Task ReadPortal_Fetch()
    {
        domainObject = await portal.Fetch();
        Assert.IsTrue(domainObject.FetchCalled);
    }

    [TestMethod]
    public async Task ReadPortal_FetchIntCriteriaCalled()
    {
        int crit = DateTime.Now.Millisecond;
        domainObject = await portal.Fetch(crit);
        Assert.AreEqual(crit, domainObject.IntCriteria);
    }

}
