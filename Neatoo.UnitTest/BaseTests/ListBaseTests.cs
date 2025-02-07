using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Neatoo.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.BaseTests
{
    [TestClass]
    public class ListBaseTests
    {
        private IServiceScope scope;
        private IBaseObjectList list;

        [TestInitialize]
        public void TestInitialize()
        {
            scope = UnitTestServices.GetLifetimeScope();
            list = scope.GetRequiredService<IBaseObjectList>();
        }

        [TestMethod]
        public void ListBase_Construct()
        {
            var name = list.FirstName;
        }

        [TestMethod]
        public void ListBase_Set()
        {
            list.Id = Guid.NewGuid();
            list.FirstName = Guid.NewGuid().ToString();
            list.LastName = Guid.NewGuid().ToString();
        }

        [TestMethod]
        public void ListBase_SetGet()
        {
            var id = list.Id = Guid.NewGuid();
            var firstName = list.FirstName = Guid.NewGuid().ToString();
            var lastName = list.LastName = Guid.NewGuid().ToString();

            Assert.AreEqual(id, list.Id);
            Assert.AreEqual(firstName, list.FirstName);
            Assert.AreEqual(lastName, list.LastName);
        }

        [TestMethod]
        public async Task ListBase_CreateAdd()
        {
            var mock = (MockReadPortalChild<IBaseObject>) scope.GetRequiredService<IReadPortalChild<IBaseObject>>();

            mock.MockPortal.Setup(x => x.CreateChild()).ReturnsAsync(scope.GetRequiredService<IBaseObject>());

            var result = await list.CreateAdd();
            Assert.IsTrue(list.Count == 1);
            Assert.AreSame(result, list.Single());

            mock.MockPortal.Verify(x => x.CreateChild(), Times.Once);
        }

    }
}
