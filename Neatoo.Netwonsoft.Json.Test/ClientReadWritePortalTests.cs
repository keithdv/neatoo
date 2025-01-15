using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neatoo.Netwonsoft.Json.Test.EditTests;
using Neatoo.Portal;
using Neatoo.Portal.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neatoo.Netwonsoft.Json.Test
{
    [TestClass]
    public class ClientReadWritePortalTests
    {
        IServiceScope scope;
        IEditObject editObject;
        Guid Id = Guid.NewGuid();
        string Name = Guid.NewGuid().ToString();

        [TestInitialize]
        public void TestInitailize()
        {
            scope = AutofacContainer.GetLifetimeScope( Autofac.Portal.Client).Resolve<IServiceScope>();

        }

        [TestMethod]
        public async Task ClientReadWritePortal_Create()
        {
            var portal = scope.Resolve<IClientReadWritePortal<IEditObject>>();

            var editObject = await portal.Create(Id, Name);

            Assert.IsInstanceOfType<EditObject>(editObject);

            var newEditObject = await editObject.SaveRetrieve<IEditObject>();
        }

        [TestMethod]
        public async Task ClientReadWritePortal_Update()
        {
            var portal = scope.Resolve<IClientReadWritePortal<IEditObject>>();

            var editObject = await portal.Create(Id, Name);

            Assert.IsInstanceOfType<EditObject>(editObject);

            var newEditObject = await editObject.SaveRetrieve<IEditObject>();

            Assert.AreEqual("Updated", newEditObject.Name);

        }
    }
}
