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

    [TestInitialize]
    public void TestInitialize()
    {
        list = new BaseObjectList(new ListBaseServices<BaseObjectList, IBaseObject>());
    }

}