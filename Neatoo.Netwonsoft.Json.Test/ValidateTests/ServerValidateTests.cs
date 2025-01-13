using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neatoo.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Neatoo.Netwonsoft.Json.Test.ValidateTests
{
    [TestClass]
    public class ServerValidateTests
    {
        IServiceScope scope;
        IValidateObject target;
        Guid Id = Guid.NewGuid();
        string Name = Guid.NewGuid().ToString();
        IPortalJsonSerializer resolver;
        IPortalOperationManager portal;

        [TestInitialize]
        public void TestInitailize()
        {
            scope = AutofacContainer.GetLifetimeScope().Resolve<IServiceScope>();
            target = scope.Resolve<IValidateObject>();
            target.ID = Id;
            target.Name = Name;
            resolver = scope.Resolve<FatClientContractResolver>();
            portal = scope.Resolve<IPortalOperationManager<ValidateObject>>(); 
        }

        [TestMethod]
        public async Task ServerValidate_Create()
        {
            var portalRequest = new PortalRequest()
            {
                PortalOperation = PortalOperation.Create,
                Target = new ObjectTypeJson() { AssemblyType = typeof(IValidateObject).FullName }
            };


            var result = await portal.HandlePortalRequest(portalRequest);

            Assert.IsInstanceOfType<ValidateObject>(result);

        }

        [TestMethod]
        public async Task ServerValidate_CreateCriteria()
        {

            var portalRequest = new PortalRequest()
            {
                PortalOperation = PortalOperation.Create,
                Target = resolver.ToObjectTypeJson<IValidateObject>(),
                Criteria = [resolver.ToObjectTypeJson(Id), resolver.ToObjectTypeJson(Name)]
            };


            var result = await portal.HandlePortalRequest(portalRequest) as IValidateObject;

            Assert.IsInstanceOfType<ValidateObject>(result);
            Assert.AreEqual(Id, result.ID);
            Assert.AreEqual(Name, result.Name);
        }

        [TestMethod]
        public async Task ServerValidate_Update()
        {

            var portalRequest = new PortalRequest()
            {
                PortalOperation = PortalOperation.Update,
                Target = resolver.ToObjectTypeJson(target),
            };

            var result = await portal.HandlePortalRequest(portalRequest) as IValidateObject;

            Assert.IsInstanceOfType<ValidateObject>(result);
            Assert.AreEqual("Updated", result.Name);
        }
    }
}
