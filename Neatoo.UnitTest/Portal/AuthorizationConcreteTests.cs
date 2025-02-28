//using Microsoft.CodeAnalysis.CSharp.Syntax;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;
//using Neatoo.AuthorizationRules;
//using Neatoo.Portal;
//using Neatoo.Portal.Internal;
//using Neatoo.UnitTest.ObjectPortal;
//using Neatoo.UnitTest.Objects;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static Neatoo.UnitTest.Portal.AuthorizationClassTests;

//namespace Neatoo.UnitTest.Portal;

//[TestClass]
//public class AuthorizationConcreteClassTests
//{

//    public class AuthorizationConcreteClass
//    {
//        private readonly IDisposableDependency disposableDependency;

//        public int AnyAccessCount { get; set; } = 0;
//        public int CanReadCount { get; set; } = 0;
//        public int CanWriteCount { get; set; } = 0;

//        public AuthorizationConcreteClass(IDisposableDependency disposableDependency)
//        {
//            this.disposableDependency = disposableDependency;
//        }


//        [Authorize(DataMapperMethodType.Read | DataMapperMethodType.Write)]
//        public string? AnyAccess()
//        {
//            Assert.IsNotNull(disposableDependency);
//            AnyAccessCount++;
//            return string.Empty;
//        }


//        [Authorize(DataMapperMethodType.Read)]
//        public bool CanRead()
//        {
//            Assert.IsNotNull(disposableDependency);
//            CanReadCount++;
//            return true;
//        }

//        [Remote]
//        [Authorize(DataMapperMethodType.Read)]
//        public string? CanReadStringRemote(string param)
//        {
//            CanReadCount++;
//            if (param == "FAIL")
//            {
//                return "FAILMESSAGE";
//            }
//            return string.Empty;
//        }

//        [Authorize(DataMapperMethodType.Write)]
//        public bool CanWrite()
//        {
//            CanWriteCount++;
//            return true;
//        }

//        [Remote]
//        [Authorize(DataMapperMethodType.Write)]
//        public bool CanWriteStringRemote(string param)
//        {
//            CanWriteCount++;
//            if (param == "FAIL")
//            {
//                return false;
//            }
//            return true;
//        }
//    }

//    [Factory]
//    [Authorize<AuthorizationConcreteClass>]
//    public class AuthorizedConcreteClass : EditBase<AuthorizedConcreteClass>
//    {
//        public AuthorizedConcreteClass() : base(new EditBaseServices<AuthorizedConcreteClass>(Mock.Of<IFactoryEditBase<AuthorizedConcreteClass>>()))
//        {
//        }

//        public string Name { get => Getter<string>(); set => Setter(value); }

//        [Create]
//        public void Create()
//        {
//        }

//        [Create]
//        public void CreateString(string name)
//        {
//            Name = name;
//        }

//        [Insert]
//        public void Insert()
//        {

//        }

//        [Insert]
//        public void InsertString(string name)
//        {
//        }
//    }


//    private IServiceScope clientScope;
//    private AuthorizationConcreteClass authorizationClassClient;
//    private AuthorizationConcreteClass authorizationClassServer;

//    [TestInitialize]
//    public void TestIntialize()
//    {
//        var scopes = FactoryContainers.Scopes();
//        clientScope = scopes.client;
//        authorizationClassClient = clientScope.ServiceProvider.GetRequiredService<AuthorizationConcreteClass>();
//        authorizationClassServer = scopes.server.ServiceProvider.GetRequiredService<AuthorizationConcreteClass>();
//    }

//    [TestMethod]
//    public void AuthorizationClassTests_Create()
//    {
//        var factory = clientScope.GetRequiredService<AuthorizedConcreteClassFactory>();

//        var result = factory.Create();

//        Assert.IsNotNull(result);
//        Assert.AreEqual(1, authorizationClassClient.AnyAccessCount);
//        Assert.AreEqual(1, authorizationClassClient.CanReadCount);
//        Assert.AreEqual(0, authorizationClassServer.CanReadCount);
//    }


//    [TestMethod]
//    public async Task AuthorizationClassTests_CreateAuthRemote()
//    {
//        var factory = clientScope.GetRequiredService<AuthorizedConcreteClassFactory>();

//        var result = await factory.CreateString(Guid.NewGuid().ToString());

//        Assert.IsNotNull(result);
//        Assert.AreEqual(0, authorizationClassClient.AnyAccessCount);
//        Assert.AreEqual(1, authorizationClassServer.AnyAccessCount);
//        Assert.AreEqual(0, authorizationClassClient.CanReadCount);
//        Assert.AreEqual(2, authorizationClassServer.CanReadCount);
//    }

//    [TestMethod]
//    public async Task AuthorizationClassTests_CreateAuthRemote_Fail()
//    {
//        var factory = clientScope.GetRequiredService<AuthorizedConcreteClassFactory>();

//        var result = await factory.CreateString("FAIL");

//        Assert.IsNull(result);
//    }

//    [TestMethod]
//    public async Task AuthorizationClassTests_TryCreateAuthRemote_Fail()
//    {
//        var factory = clientScope.GetRequiredService<AuthorizedConcreteClassFactory>();

//        var result = await factory.TryCreateString("FAIL");

//        Assert.IsFalse(result.HasAccess);
//        Assert.AreEqual("FAILMESSAGE", result.Message);
//    }

//    [TestMethod]
//    public void AuthorizationClassTests_Write()
//    {
//        var factory = clientScope.GetRequiredService<AuthorizedConcreteClassFactory>();

//        var result = factory.Create();
//        result = factory.Save(result);

//        Assert.IsNotNull(result);
//        Assert.AreEqual(2, authorizationClassClient.AnyAccessCount);
//        Assert.AreEqual(0, authorizationClassServer.AnyAccessCount);
//        Assert.AreEqual(1, authorizationClassClient.CanWriteCount);
//        Assert.AreEqual(0, authorizationClassServer.CanWriteCount);
//    }

//    [TestMethod]
//    public async Task AuthorizationClassTests_WriteRemote()
//    {
//        var factory = clientScope.GetRequiredService<AuthorizedConcreteClassFactory>();

//        var result = factory.Create();
//        result = await factory.Save(result, "string");

//        Assert.AreEqual(1, authorizationClassClient.AnyAccessCount);
//        Assert.AreEqual(1, authorizationClassServer.AnyAccessCount);
//        Assert.AreEqual(0, authorizationClassClient.CanWriteCount);
//        Assert.AreEqual(2, authorizationClassServer.CanWriteCount);
//    }

//    [TestMethod]
//    public async Task AuthorizationClassTests_WriteRemote_Fail()
//    {
//        var factory = clientScope.GetRequiredService<AuthorizedConcreteClassFactory>();

//        var result = factory.Create();

//        await Assert.ThrowsExceptionAsync<NotAuthorizedException>(() => factory.Save(result, "FAIL"));
//    }

//    [TestMethod]
//    public async Task AuthorizationClassTests_TryWriteRemote_Fail()
//    {
//        var factory = clientScope.GetRequiredService<AuthorizedConcreteClassFactory>();

//        var result = factory.Create();

//        var authorized = await factory.TrySave(result, "FAIL");

//        Assert.IsFalse(authorized.HasAccess);
//        Assert.IsNull(authorized.Result);
//        Assert.IsNull(authorized.Message);
//    }
//}
