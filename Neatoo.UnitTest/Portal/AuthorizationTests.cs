using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Neatoo.AuthorizationRules;
using Neatoo.Portal;
using Neatoo.Portal.Internal;
using Neatoo.UnitTest.ObjectPortal;
using Neatoo.UnitTest.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.Portal;

[TestClass]
public class AuthorizationClassTests
{


    public interface IAuthorization
    {
        [Authorize(DataMapperMethodType.Write)]
        bool AnyAccess();

        [Authorize(DataMapperMethodType.Write)]
        string? AnyAccess(int p);
    }
    internal class Authorization : IAuthorization
    {
        public bool AnyAccess()
        {
            return true;
        }


        public string? AnyAccess(int p)
        {
            return "This means auth failed";
        }
    }

    public interface IAuthorizedClass    {    }

    [Factory]
    [Authorize<IAuthorization>]
    public class AuthorizedClass : IAuthorizedClass
    {

        [Create]
        public void Create(int p)
        {

        }


        [Remote]
        [Insert]
        public void Insert(int p, string onno, [Service] IDisposableDependency disposableDependency)
        {

        }

        [Remote]
        [Update]
        public async Task<bool> Update(int p, string onno, [Service] IDisposableDependency disposableDependency)
        {
            await Task.Delay(1);
            return false;
        }

        [Delete]
        public void Delete(int p, [Service] IDisposableDependency disposableDependency)
        {

        }
    }


    //private IServiceScope clientScope;
    //private IAuthorizationClass authorizationClassClient;
    //private IAuthorizationClass authorizationClassServer;

    //[TestInitialize]
    //public void TestIntialize()
    //{
    //    var scopes = FactoryContainers.Scopes();
    //    clientScope = scopes.client;
    //    authorizationClassClient = clientScope.ServiceProvider.GetRequiredService<IAuthorizationClass>();
    //    authorizationClassServer = scopes.server.ServiceProvider.GetRequiredService<IAuthorizationClass>();
    //}

    //[TestMethod]
    //public void AuthorizationClassTests_Create()
    //{
    //    var factory = clientScope.GetRequiredService<IAuthorizedClassFactory>();

    //    var result = factory.Create();

    //    Assert.IsNotNull(result);
    //    Assert.AreEqual(1, authorizationClassClient.AnyAccessCount);
    //    Assert.AreEqual(1, authorizationClassClient.CanReadCount);
    //    Assert.AreEqual(0, authorizationClassServer.CanReadCount);
    //}

    //[TestMethod]
    //public void AuthorizationClassTests_CreateInt()
    //{
    //    var factory = clientScope.GetRequiredService<IAuthorizedClassFactory>();

    //    var result = factory.CreateInt(1);

    //    Assert.IsNotNull(result);
    //    Assert.AreEqual(1, authorizationClassClient.AnyAccessCount);
    //    Assert.AreEqual(2, authorizationClassClient.CanReadCount);
    //    Assert.AreEqual(0, authorizationClassServer.CanReadCount);
    //}

    //[TestMethod]
    //public async Task AuthorizationClassTests_CreateAuthRemote()
    //{
    //    var factory = clientScope.GetRequiredService<IAuthorizedClassFactory>();

    //    var result = await factory.CreateString(Guid.NewGuid().ToString());

    //    Assert.IsNotNull(result);
    //    Assert.AreEqual(0, authorizationClassClient.AnyAccessCount);
    //    Assert.AreEqual(1, authorizationClassServer.AnyAccessCount);
    //    Assert.AreEqual(0, authorizationClassClient.CanReadCount);
    //    Assert.AreEqual(2, authorizationClassServer.CanReadCount);
    //}

    //[TestMethod]
    //public async Task AuthorizationClassTests_CreateAuthRemote_Fail()
    //{
    //    var factory = clientScope.GetRequiredService<IAuthorizedClassFactory>();

    //    var result = await factory.CreateString("FAIL");

    //    Assert.IsNull(result);
    //}

    //[TestMethod]
    //public async Task AuthorizationClassTests_TryCreateAuthRemote_Fail()
    //{
    //    var factory = clientScope.GetRequiredService<IAuthorizedClassFactory>();

    //    var result = await factory.TryCreateString("FAIL");

    //    Assert.IsFalse(result.HasAccess);
    //    Assert.AreEqual("FAILMESSAGE", result.Message);
    //}

    //[TestMethod]
    //public void AuthorizationClassTests_Write()
    //{
    //    var factory = clientScope.GetRequiredService<IAuthorizedClassFactory>();

    //    var result = factory.Create();
    //    result = factory.Save(result);

    //    Assert.IsNotNull(result);
    //    Assert.AreEqual(2, authorizationClassClient.AnyAccessCount);
    //    Assert.AreEqual(0, authorizationClassServer.AnyAccessCount);
    //    Assert.AreEqual(1, authorizationClassClient.CanWriteCount);
    //    Assert.AreEqual(0, authorizationClassServer.CanWriteCount);
    //}

    //[TestMethod]
    //public async Task AuthorizationClassTests_WriteRemote()
    //{
    //    var factory = clientScope.GetRequiredService<IAuthorizedClassFactory>();

    //    IAuthorizedClass? result = factory.Create();
    //    result = await factory.Save(result, "string");

    //    Assert.AreEqual(1, authorizationClassClient.AnyAccessCount);
    //    Assert.AreEqual(1, authorizationClassServer.AnyAccessCount);
    //    Assert.AreEqual(0, authorizationClassClient.CanWriteCount);
    //    Assert.AreEqual(2, authorizationClassServer.CanWriteCount);
    //}

    //[TestMethod]
    //public async Task AuthorizationClassTests_WriteRemote_Fail()
    //{
    //    var factory = clientScope.GetRequiredService<IAuthorizedClassFactory>();

    //    var result = factory.Create();

    //    await Assert.ThrowsExceptionAsync<NotAuthorizedException>(() => factory.Save(result, "FAIL"));
    //}

    //[TestMethod]
    //public async Task AuthorizationClassTests_TryWriteRemote_Fail()
    //{
    //    var factory = clientScope.GetRequiredService<IAuthorizedClassFactory>();

    //    var result = factory.Create();

    //    var authorized = await factory.TrySave(result, "FAIL");

    //    Assert.IsFalse(authorized.HasAccess);
    //    Assert.IsNull(authorized.Result);
    //    Assert.IsNull(authorized.Message);
    //}
}
