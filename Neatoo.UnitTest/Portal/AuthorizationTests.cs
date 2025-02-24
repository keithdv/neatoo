using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Neatoo.AuthorizationRules;
using Neatoo.Portal;
using Neatoo.Portal.Internal;
using Neatoo.UnitTest.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.Portal;


public interface IAuthorizationClass
{
    int CanReadCount { get; }
    int CanWriteCount { get; }
    int AnyAccessCount { get; set; }

    [Authorize(DataMapperMethodType.Read | DataMapperMethodType.Write)]
    IAuthorizationRuleResult AnyAccess();

    [Authorize(DataMapperMethodType.Read)]
    IAuthorizationRuleResult CanRead();

    [Authorize(DataMapperMethodType.Read)]
    IAuthorizationRuleResult CanReadInt(int param);

    [Remote]
    [Authorize(DataMapperMethodType.Read)]
    IAuthorizationRuleResult CanReadStringRemote(string param);

    [Authorize(DataMapperMethodType.Write)]
    IAuthorizationRuleResult CanWrite();

    [Authorize(DataMapperMethodType.Write)]
    IAuthorizationRuleResult CanWriteTarget(IAuthorizedClass authorizedClass);


    [Authorize(DataMapperMethodType.Write)]
    IAuthorizationRuleResult CanWriteInt(int param);

    [Remote]
    [Authorize(DataMapperMethodType.Write)]
    IAuthorizationRuleResult CanWriteStringRemote(string param);
}

public class AuthorizationClass : AuthorizedEditBase, IAuthorizationClass
{
    private readonly IDisposableDependency disposableDependency;

    public int AnyAccessCount { get; set; } = 0;
    public int CanReadCount { get; set; } = 0;
    public int CanWriteCount { get; set; } = 0;

    public AuthorizationClass(IDisposableDependency disposableDependency)
    {
        this.disposableDependency = disposableDependency;
    }

    public IAuthorizationRuleResult AnyAccess()
    {
        Assert.IsNotNull(disposableDependency);
        AnyAccessCount++;
        return AuthorizationRuleResult.AccessGranted();
    }

    public IAuthorizationRuleResult CanRead()
    {
        Assert.IsNotNull(disposableDependency);
        CanReadCount++;
        return AuthorizationRuleResult.AccessGranted();
    }

    public IAuthorizationRuleResult CanReadInt(int param)
    {
        CanReadCount++;
        if (param == 10)
        {
            return AuthorizationRuleResult.AccessDenied("Read is not allowed");
        }
        return AuthorizationRuleResult.AccessGranted();
    }

    public IAuthorizationRuleResult CanReadStringRemote(string param)
    {
        CanReadCount++;
        if (param == "FAIL")
        {
            return AuthorizationRuleResult.AccessDenied("Read is not allowed");
        }
        return AuthorizationRuleResult.AccessGranted();
    }


    public IAuthorizationRuleResult CanWrite()
    {
        CanWriteCount++;
        return AuthorizationRuleResult.AccessGranted();
    }

    public IAuthorizationRuleResult CanWriteTarget(IAuthorizedClass authorizedClass)
    {
        CanWriteCount++;
        return AuthorizationRuleResult.AccessGranted();
    }

    public IAuthorizationRuleResult CanWriteInt(int param)
    {
        CanWriteCount++;
        if (param == 10)
        {
            return AuthorizationRuleResult.AccessDenied("Write is not allowed");
        }
        return AuthorizationRuleResult.AccessGranted();
    }

    public IAuthorizationRuleResult CanWriteStringRemote(string param)
    {
        CanWriteCount++;
        if (param == "FAIL")
        {
            return AuthorizationRuleResult.AccessDenied("Write is not allowed");
        }
        return AuthorizationRuleResult.AccessGranted();
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
    string Name { get; set; }
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
    public void CreateInt(int param)
    {
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

}

[TestClass]
public class AuthorizationClassTests
{
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
        Assert.AreEqual(authorizationClassClient.AnyAccessCount, 1);
        Assert.AreEqual(authorizationClassClient.CanReadCount, 1);
        Assert.AreEqual(authorizationClassServer.CanReadCount, 0);
    }

    [TestMethod]
    public void AuthorizationClassTests_CreateInt()
    {
        var factory = clientScope.GetRequiredService<IAuthorizedClassFactory>();

        var result = factory.CreateInt(1);

        Assert.AreEqual(1, authorizationClassClient.AnyAccessCount);
        Assert.AreEqual(2, authorizationClassClient.CanReadCount);
        Assert.AreEqual(0, authorizationClassServer.CanReadCount);
    }

    [TestMethod]
    public void AuthorizationClassTests_CreateInt_Fail()
    {
        var factory = clientScope.GetRequiredService<IAuthorizedClassFactory>();

        Assert.ThrowsException<NotAuthorizedException>(() => factory.CreateInt(10));

        Assert.AreEqual(1, authorizationClassClient.AnyAccessCount);
        Assert.AreEqual(2, authorizationClassClient.CanReadCount);
        Assert.AreEqual(0, authorizationClassServer.CanReadCount);
    }

    [TestMethod]
    public async Task AuthorizationClassTests_CreateAuthRemote()
    {
        var factory = clientScope.GetRequiredService<IAuthorizedClassFactory>();

        var result = await factory.CreateString(Guid.NewGuid().ToString());

        Assert.AreEqual(0, authorizationClassClient.AnyAccessCount);
        Assert.AreEqual(1, authorizationClassServer.AnyAccessCount);
        Assert.AreEqual(0, authorizationClassClient.CanReadCount);
        Assert.AreEqual(2, authorizationClassServer.CanReadCount);
    }

    [TestMethod]
    public async Task AuthorizationClassTests_CreateAuthRemote_Fail()
    {
        var factory = clientScope.GetRequiredService<IAuthorizedClassFactory>();

        await Assert.ThrowsExceptionAsync<NotAuthorizedException>(() => factory.CreateString("FAIL"));
    }

    [TestMethod]
    public void AuthorizationClassTests_Write()
    {
        var factory = clientScope.GetRequiredService<IAuthorizedClassFactory>();

        var result = factory.Create();
        result = factory.Save(result);

        Assert.AreEqual(2, authorizationClassClient.AnyAccessCount);
        Assert.AreEqual(0, authorizationClassServer.AnyAccessCount);
        Assert.AreEqual(2, authorizationClassClient.CanWriteCount);
        Assert.AreEqual(0, authorizationClassServer.CanWriteCount);
    }

    [TestMethod]
    public async Task AuthorizationClassTests_WriteRemote()
    {
        var factory = clientScope.GetRequiredService<IAuthorizedClassFactory>();

        var result = factory.Create();
        result = await factory.Save(result, "string");

        Assert.AreEqual(1, authorizationClassClient.AnyAccessCount);
        Assert.AreEqual(1, authorizationClassServer.AnyAccessCount);
        Assert.AreEqual(0, authorizationClassClient.CanWriteCount);
        Assert.AreEqual(3, authorizationClassServer.CanWriteCount);
    }

    [TestMethod]
    public async Task AuthorizationClassTests_WriteRemote_Fail()
    {
        var factory = clientScope.GetRequiredService<IAuthorizedClassFactory>();

        var result = factory.Create();

        await Assert.ThrowsExceptionAsync<NotAuthorizedException>(() => factory.Save(result, "FAIL"));
    }
}
