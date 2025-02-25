﻿using Microsoft.CodeAnalysis.CSharp.Syntax;
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
using System.Text;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.Portal;

[TestClass]
public class AuthorizationClassTests
{
    public interface IAuthorizationClass
    {
        int CanReadCount { get; }
        int CanWriteCount { get; }
        int AnyAccessCount { get; set; }

        [Authorize(DataMapperMethodType.Read | DataMapperMethodType.Write)]
        string? AnyAccess();

        [Authorize(DataMapperMethodType.Read)]
        bool CanRead();

        [Authorize(DataMapperMethodType.Read)]
        string? CanReadInt(int param);

        [Authorize(DataMapperMethodType.Read)]
        string? CanReadInt2();

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        string? CanReadStringRemote(string param);

        [Authorize(DataMapperMethodType.Write)]
        bool CanWrite();

        [Authorize(DataMapperMethodType.Write)]
        bool CanWriteTarget(AuthorizedClass authorizedClass);


        [Authorize(DataMapperMethodType.Write)]
        string? CanWriteInt(int param);

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        bool CanWriteStringRemote(string param);
    }

    public class AuthorizationClass : IAuthorizationClass
    {
        private readonly IDisposableDependency disposableDependency;

        public int AnyAccessCount { get; set; } = 0;
        public int CanReadCount { get; set; } = 0;
        public int CanWriteCount { get; set; } = 0;

        public AuthorizationClass(IDisposableDependency disposableDependency)
        {
            this.disposableDependency = disposableDependency;
        }

        public string? AnyAccess()
        {
            Assert.IsNotNull(disposableDependency);
            AnyAccessCount++;
            return string.Empty;
        }

        public bool CanRead()
        {
            Assert.IsNotNull(disposableDependency);
            CanReadCount++;
            return true;
        }

        public string? CanReadInt(int param)
        {
            CanReadCount++;
            return string.Empty;
        }

        public string? CanReadInt2()
        {
            return string.Empty;
        }

        public string? CanReadStringRemote(string param)
        {
            CanReadCount++;
            if (param == "FAIL")
            {
                return "FAILMESSAGE";
            }
            return string.Empty;
        }

        public bool CanWrite()
        {
            CanWriteCount++;
            return true;
        }

        public bool CanWriteTarget(AuthorizedClass authorizedClass)
        {
            CanWriteCount++;
            return true;
        }

        public string? CanWriteInt(int param)
        {
            CanWriteCount++;
            return string.Empty;
        }

        public bool CanWriteStringRemote(string param)
        {
            CanWriteCount++;
            if (param == "FAIL")
            {
                return false;
            }
            return true;
        }
    }

    public class AuthorizedEditBase
    {
        public virtual Task<IEditBase> Save()
        {
            throw new NotImplementedException();
        }
    }

    public interface IAuthorizedClass : IEditBase
    {
        public string Name { get; }
    }

    [Factory]
    [Authorize<IAuthorizationClass>]
    public class AuthorizedClass : EditBase<AuthorizedClass>, IAuthorizedClass
    {
        public AuthorizedClass() : base(new EditBaseServices<AuthorizedClass>(Mock.Of<IFactoryEditBase<AuthorizedClass>>()))
        {
        }

        public string Name { get => Getter<string>(); set => Setter(value); }

        [Create]
        public void Create()
        {
        }

        [Create]
        public async Task CreateInt(int param)
        {
            await Task.CompletedTask;
        }

        [Create]
        public void CreateString(string name)
        {
            Name = name;
        }

        [Insert]
        public void Insert()
        {

        }

        [Insert]
        public void InsertInt(int name)
        {
        }

        [Insert]
        public void InsertString(string name)
        {
        }

        [Update]
        public void UpdateString(string name)
        {
        }
    }


    private IServiceScope clientScope;
    private IAuthorizationClass authorizationClassClient;
    private IAuthorizationClass authorizationClassServer;

    [TestInitialize]
    public void TestIntialize()
    {
        var scopes = FactoryContainers.Scopes();
        clientScope = scopes.client;
        authorizationClassClient = clientScope.ServiceProvider.GetRequiredService<IAuthorizationClass>();
        authorizationClassServer = scopes.server.ServiceProvider.GetRequiredService<IAuthorizationClass>();
    }

    [TestMethod]
    public void AuthorizationClassTests_Create()
    {
        var factory = clientScope.GetRequiredService<IAuthorizedClassFactory>();

        var result = factory.Create();

        Assert.IsNotNull(result);
        Assert.AreEqual(1, authorizationClassClient.AnyAccessCount);
        Assert.AreEqual(1, authorizationClassClient.CanReadCount);
        Assert.AreEqual(0, authorizationClassServer.CanReadCount);
    }

    [TestMethod]
    public void AuthorizationClassTests_CreateInt()
    {
        var factory = clientScope.GetRequiredService<IAuthorizedClassFactory>();

        var result = factory.CreateInt(1);

        Assert.IsNotNull(result);
        Assert.AreEqual(1, authorizationClassClient.AnyAccessCount);
        Assert.AreEqual(2, authorizationClassClient.CanReadCount);
        Assert.AreEqual(0, authorizationClassServer.CanReadCount);
    }

    [TestMethod]
    public async Task AuthorizationClassTests_CreateAuthRemote()
    {
        var factory = clientScope.GetRequiredService<IAuthorizedClassFactory>();

        var result = await factory.CreateString(Guid.NewGuid().ToString());

        Assert.IsNotNull(result);
        Assert.AreEqual(0, authorizationClassClient.AnyAccessCount);
        Assert.AreEqual(1, authorizationClassServer.AnyAccessCount);
        Assert.AreEqual(0, authorizationClassClient.CanReadCount);
        Assert.AreEqual(2, authorizationClassServer.CanReadCount);
    }

    [TestMethod]
    public async Task AuthorizationClassTests_CreateAuthRemote_Fail()
    {
        var factory = clientScope.GetRequiredService<IAuthorizedClassFactory>();

        var result = await factory.CreateString("FAIL");

        Assert.IsNull(result);
    }

    [TestMethod]
    public async Task AuthorizationClassTests_TryCreateAuthRemote_Fail()
    {
        var factory = clientScope.GetRequiredService<IAuthorizedClassFactory>();

        var result = await factory.TryCreateString("FAIL");

        Assert.IsFalse(result.HasAccess);
        Assert.AreEqual("FAILMESSAGE", result.Message);
    }

    [TestMethod]
    public void AuthorizationClassTests_Write()
    {
        var factory = clientScope.GetRequiredService<IAuthorizedClassFactory>();

        var result = factory.Create();
        result = factory.Save(result);

        Assert.IsNotNull(result);
        Assert.AreEqual(2, authorizationClassClient.AnyAccessCount);
        Assert.AreEqual(0, authorizationClassServer.AnyAccessCount);
        Assert.AreEqual(1, authorizationClassClient.CanWriteCount);
        Assert.AreEqual(0, authorizationClassServer.CanWriteCount);
    }

    [TestMethod]
    public async Task AuthorizationClassTests_WriteRemote()
    {
        var factory = clientScope.GetRequiredService<IAuthorizedClassFactory>();

        IAuthorizedClass? result = factory.Create();
        result = await factory.Save(result, "string");

        Assert.AreEqual(1, authorizationClassClient.AnyAccessCount);
        Assert.AreEqual(1, authorizationClassServer.AnyAccessCount);
        Assert.AreEqual(0, authorizationClassClient.CanWriteCount);
        Assert.AreEqual(2, authorizationClassServer.CanWriteCount);
    }

    [TestMethod]
    public async Task AuthorizationClassTests_WriteRemote_Fail()
    {
        var factory = clientScope.GetRequiredService<IAuthorizedClassFactory>();

        var result = factory.Create();

        await Assert.ThrowsExceptionAsync<NotAuthorizedException>(() => factory.Save(result, "FAIL"));
    }

    [TestMethod]
    public async Task AuthorizationClassTests_TryWriteRemote_Fail()
    {
        var factory = clientScope.GetRequiredService<IAuthorizedClassFactory>();

        var result = factory.Create();

        var authorized = await factory.TrySave(result, "FAIL");

        Assert.IsFalse(authorized.HasAccess);
        Assert.IsNull(authorized.Result);
        Assert.IsNull(authorized.Message);
    }
}
