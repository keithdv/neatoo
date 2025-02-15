using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neatoo.Portal;
using Neatoo.Portal.Internal;
using Neatoo.UnitTest.ObjectPortal;
using System;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.Portal;

[TestClass]
public class DataMapperOperationTests
{
    IServiceScope scope;
    IEditObject target;
    INeatooJsonSerializer resolver;
    IDataMapper portal;

    [TestInitialize]
    public void TestInitailize()
    {
        scope = UnitTestServices.GetLifetimeScope();
        target = scope.GetRequiredService<IEditObject>();
        target.ID = Guid.NewGuid();
        resolver = scope.GetRequiredService<INeatooJsonSerializer>();
        portal = scope.GetRequiredService<IDataMapper<EditObject>>();
    }

    [TestMethod]
    public async Task ServerValidate_Create()
    {
        var portalRequest = new RemoteRequest()
        {
            DataMapperOperation = DataMapperMethod.Create,
            Target = new ObjectTypeJson() { AssemblyType = typeof(IEditObject).FullName }
        };


        var result = await portal.HandlePortalRequest(portalRequest);

        Assert.IsInstanceOfType<EditObject>(result);

    }

    [TestMethod]
    public async Task ServerValidate_CreateCriteria()
    {
        var portalRequest = resolver.ToRemoteRequest(DataMapperMethod.Create, typeof(IEditObject), target.ID);

        var result = await portal.HandlePortalRequest(portalRequest) as IEditObject;

        Assert.IsInstanceOfType<EditObject>(result);

        Assert.AreEqual(target.ID, result.GuidCriteria);

    }

    [TestMethod]
    public async Task ServerValidate_Update()
    {

        var portalRequest = resolver.ToRemoteRequest(DataMapperMethod.Update, target);

        var result = await portal.HandlePortalRequest(portalRequest) as IEditObject;

        Assert.IsInstanceOfType<EditObject>(result);
        

        result.UpdateCalled = true;

    }
}