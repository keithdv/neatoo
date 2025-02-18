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
    private INeatooPortal<IBaseObject> portal;
    private IBaseObject domainObject;

    [TestInitialize]
    public void TestInitialize()
    {
        portal = scope.GetRequiredService<INeatooPortal<IBaseObject>>();
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
    public async Task ReadPortal_CreateInferredCriteriaCalled()
    {
        var crit = new List<int>() { 0, 1, 2 };
        domainObject = await portal.Create(crit);
        Assert.IsTrue(domainObject.CreateCalled);
    }


    [TestMethod]
    public async Task ReadPortal_CreateMultipleCriteriaCalled()
    {
        domainObject = await portal.Create(10, "String");
        CollectionAssert.AreEquivalent(new object[] { 10, "String" }, domainObject.MultipleCriteria);
    }

    [TestMethod]
    public void ReadPortal_CreateMultipleCriteria_Missing_Fail()
    {
        // No such method exists

        Assert.ThrowsException<AggregateException>(() => domainObject = portal.Create(Guid.NewGuid(), "String").Result);

    }

    [TestMethod]
    public void ReadPortal_CreateMultipleCriteria_Duplicate_Fail()
    {
        // Two possibilities exist due to one with a dependency and one without
        Assert.ThrowsException<AggregateException>(() => domainObject = portal.Create(1u).Result);

    }

    [TestMethod]
    public async Task ReadPortal_CreateMultipleCriteriaCalled_Double()
    {
        domainObject = await portal.Create(1, 10d);
        CollectionAssert.AreEquivalent(new object[] { 1, 10d }, domainObject.MultipleCriteria);
    }

    [TestMethod]
    [Ignore]
    public async Task ReadPortal_CreateMultipleCriteria_NullIncluded()
    {
        // Null created the need for generic criteria method
        // If a null criteria value is sent no longer have information
        // to know what method to connect up to

        // 1/12/2025 Keith Voels - Let's not support this, doesn't make much sense

        var param1 = new List<int> { 10, 20 };
        List<int> param2 = null;

        domainObject = await portal.Create(param1, param2);
        CollectionAssert.AreEquivalent(new object[] { param1, param2 }, domainObject.MultipleCriteria);
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
