using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neatoo.UnitTest.ObjectPortal;

namespace Neatoo.UnitTest.Portal;

[TestClass]
public class PortalEditListSaveTests
{

    private IServiceScope scope = UnitTestServices.GetLifetimeScope(true);
    private INeatooPortal<IEditObjectList> portal;
    private IEditObjectList list;
    private IEditObject child;

    [TestInitialize]
    public void TestInitialize()
    {
        portal = scope.GetRequiredService<INeatooPortal<IEditObjectList>>();
        list = portal.Fetch().Result;
        child = list.CreateAdd().Result;
        child.MarkUnmodified();
        child.MarkOld();

        Assert.IsFalse(list.IsModified);

    }

    [TestCleanup]
    public void TestCleanup()
    {
        scope.Dispose();
    }

}
