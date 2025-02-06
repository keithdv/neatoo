using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neatoo.Portal;
using Neatoo.UnitTest.ObjectPortal;
using Neatoo.UnitTest.ValidateBaseTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Neatoo.UnitTest.Portal
{
    [TestClass]
    public class PortalOperationManagerTests
    {
        IServiceScope scope;
        IEditObject target;
        IPortalJsonSerializer resolver;
        IPortalOperationManager portal;

        [TestInitialize]
        public void TestInitailize()
        {
            scope = UnitTestServices.GetLifetimeScope();
            target = scope.GetRequiredService<IEditObject>();
            target.ID = Guid.NewGuid();
            resolver = scope.GetRequiredService<IPortalJsonSerializer>();
            portal = scope.GetRequiredService<IPortalOperationManager<EditObject>>();
        }

        [TestMethod]
        public async Task ServerValidate_Create()
        {
            var portalRequest = new PortalRequest()
            {
                PortalOperation = PortalOperation.Create,
                Target = new ObjectTypeJson() { AssemblyType = typeof(IEditObject).FullName }
            };


            var result = await portal.HandlePortalRequest(portalRequest);

            Assert.IsInstanceOfType<EditObject>(result);

        }

        [TestMethod]
        public async Task ServerValidate_CreateCriteria()
        {
            var portalRequest = resolver.ToPortalRequest(PortalOperation.Create, typeof(IEditObject), target.ID);

            var result = await portal.HandlePortalRequest(portalRequest) as IEditObject;

            Assert.IsInstanceOfType<EditObject>(result);

            Assert.AreEqual(target.ID, result.GuidCriteria);

        }

        [TestMethod]
        public async Task ServerValidate_Update()
        {

            var portalRequest = resolver.ToPortalRequest(PortalOperation.Update, target);

            var result = await portal.HandlePortalRequest(portalRequest) as IEditObject;

            Assert.IsInstanceOfType<EditObject>(result);
            

            result.UpdateCalled = true;

        }
    }
}