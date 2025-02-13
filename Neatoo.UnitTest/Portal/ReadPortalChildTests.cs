using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.ObjectPortal;


[TestClass]
public class ReadPortalChildTests
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
