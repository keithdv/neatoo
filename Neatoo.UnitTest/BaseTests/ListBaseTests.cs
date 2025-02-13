using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Neatoo.Portal;
using Neatoo.UnitTest.BaseTests.Objects;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.BaseTests;

[TestClass]
public class ListBaseTests
{
    private IBaseObjectList list;
    private MockReadPortalChild<IBaseObject> mock;

    [TestInitialize]
    public void TestInitialize()
    {
        mock = new MockReadPortalChild<IBaseObject>();
        list = new BaseObjectList(new ListBaseServices<BaseObjectList, IBaseObject>(mock.MockPortal.Object));
    }

    [TestMethod]
    public async Task ListBase_CreateAdd()
    {
        mock.MockPortal.Setup(x => x.CreateChild()).ReturnsAsync(new BaseObject());

        var result = await list.CreateAdd();
        Assert.IsTrue(list.Count == 1);
        Assert.AreSame(result, list.Single());

        mock.MockPortal.Verify(x => x.CreateChild(), Times.Once);
    }
}