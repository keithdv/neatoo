using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neatoo.AuthorizationRules;
using Neatoo.Portal;
using Neatoo.UnitTest.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.Portal
{

    [TestClass]
    public class ReadRemoteAuthTests
    {

        public class ReadRemoteAuthTask : ReadRemoteAuth
        {
            [Remote]
            [Authorize(DataMapperMethodType.Read)]
            public Task<bool> CanReadRemoteBoolTask()
            {
                CanReadRemoteCalled++;
                return Task.FromResult(true);
            }

            [Remote]
            [Authorize(DataMapperMethodType.Read)]
            public Task<bool> CanReadRemoteBoolFalseTask(int? p)
            {
                CanReadRemoteCalled++;
                if (p == 10)
                {
                    return Task.FromResult(false);
                }
                return Task.FromResult(true);
            }

            [Remote]
            [Authorize(DataMapperMethodType.Read)]
            public Task<string> CanReadRemoteStringTask()
            {
                CanReadRemoteCalled++;
                return Task.FromResult(string.Empty);
            }

            [Remote]
            [Authorize(DataMapperMethodType.Read)]
            public Task<string> CanReadRemoteStringFalseTask(int? p)
            {
                CanReadRemoteCalled++;
                if (p == 20)
                {
                    return Task.FromResult("Fail");
                }
                return Task.FromResult(string.Empty);
            }

            [Remote]
            [Authorize(DataMapperMethodType.Create)]
            public Task<bool> CanCreateBoolTask()
            {
                CanCreateCalled++;
                return Task.FromResult(true);
            }

            [Remote]
            [Authorize(DataMapperMethodType.Create)]
            public Task<bool> CanCreateBoolFalseTask(int? p)
            {
                CanCreateCalled++;
                if (p == 10)
                {
                    return Task.FromResult(false);
                }
                return Task.FromResult(true);
            }

            [Remote]
            [Authorize(DataMapperMethodType.Create)]
            public Task<string> CanCreateStringTask()
            {
                CanCreateCalled++;
                return Task.FromResult(string.Empty);
            }

            [Remote]
            [Authorize(DataMapperMethodType.Create)]
            public Task<string> CanCreateStringFalseTask(int? p)
            {
                CanCreateCalled++;
                if (p == 20)
                {
                    return Task.FromResult("Fail");
                }
                return Task.FromResult(string.Empty);
            }

            [Remote]
            [Authorize(DataMapperMethodType.Fetch)]
            public Task<bool> CanFetchBoolTask()
            {
                CanFetchCalled++;
                return Task.FromResult(true);
            }

            [Remote]
            [Authorize(DataMapperMethodType.Fetch)]
            public Task<bool> CanFetchBoolFalseTask(int? p)
            {
                CanFetchCalled++;
                if (p == 10)
                {
                    return Task.FromResult(false);
                }
                return Task.FromResult(true);
            }

            [Remote]
            [Authorize(DataMapperMethodType.Fetch)]
            public Task<string> CanFetchStringTask()
            {
                CanFetchCalled++;
                return Task.FromResult(string.Empty);
            }

            [Remote]
            [Authorize(DataMapperMethodType.Fetch)]
            public Task<string> CanFetchStringFalseTask(int? p)
            {
                CanFetchCalled++;
                if (p == 20)
                {
                    return Task.FromResult("Fail");
                }
                return Task.FromResult(string.Empty);
            }
        }

        public class ReadRemoteAuth
        {
            public int CanReadRemoteCalled { get; set; }

            [Remote]
            [Authorize(DataMapperMethodType.Read)]
            public bool CanReadRemoteBool()
            {
                CanReadRemoteCalled++;
                return true;
            }

            [Remote]
            [Authorize(DataMapperMethodType.Read)]
            public bool CanReadRemoteBoolFalse(int? p)
            {
                CanReadRemoteCalled++;
                if (p == 10)
                {
                    return false;
                }
                return true;
            }

            [Remote]
            [Authorize(DataMapperMethodType.Read)]
            public string? CanReadRemoteString()
            {
                CanReadRemoteCalled++;
                return string.Empty;
            }

            [Remote]
            [Authorize(DataMapperMethodType.Read)]
            public string? CanReadRemoteStringFalse(int? p)
            {
                CanReadRemoteCalled++;
                if (p == 20)
                {
                    return "Fail";
                }
                return string.Empty;
            }

            public int CanCreateCalled { get; set; }

            [Remote]
            [Authorize(DataMapperMethodType.Create)]
            public bool CanCreateBool()
            {
                CanCreateCalled++;
                return true;
            }

            [Remote]
            [Authorize(DataMapperMethodType.Create)]
            public bool CanCreateBoolFalse(int? p)
            {
                CanCreateCalled++;
                if (p == 10)
                {
                    return false;
                }
                return true;
            }

            [Remote]
            [Authorize(DataMapperMethodType.Create)]
            public string? CanCreateString()
            {
                CanCreateCalled++;
                return string.Empty;
            }

            [Remote]
            [Authorize(DataMapperMethodType.Create)]
            public string? CanCreateStringFalse(int? p)
            {
                CanCreateCalled++;
                if (p == 20)
                {
                    return "Fail";
                }
                return string.Empty;
            }

            public int CanFetchCalled { get; set; }

            [Remote]
            [Authorize(DataMapperMethodType.Fetch)]
            public bool CanFetchBool()
            {
                CanFetchCalled++;
                return true;
            }

            [Remote]
            [Authorize(DataMapperMethodType.Fetch)]
            public bool CanFetchBoolFalse(int? p)
            {
                CanFetchCalled++;
                if (p == 10)
                {
                    return false;
                }
                return true;
            }

            [Remote]
            [Authorize(DataMapperMethodType.Fetch)]
            public string? CanFetchString()
            {
                CanFetchCalled++;
                return string.Empty;
            }

            [Remote]
            [Authorize(DataMapperMethodType.Fetch)]
            public string? CanFetchStringFalse(int? p)
            {
                CanFetchCalled++;
                if (p == 20)
                {
                    return "Fail";
                }
                return string.Empty;
            }
        }

        [Factory]
        [Authorize<ReadRemoteAuth>]
        public class ReadRemoteAuthDataMapper : ReadTests.ReadDataMapper
        {

        }

        [Factory]
        [Authorize<ReadRemoteAuthTask>]
        public class ReadRemoteAuthTaskDataMapper : ReadTests.ReadDataMapper
        {

        }

        private IServiceScope clientScope;
        private IServiceScope serverScope;

        [TestInitialize]
        public void TestIntialize()
        {
            var scopes = FactoryContainers.Scopes();
            clientScope = scopes.client;
            serverScope = scopes.server;
        }

        [TestMethod]
        public async Task ReadRemoteAuthTest()
        {
            var readFactory = clientScope.ServiceProvider.GetRequiredService<IReadRemoteAuthDataMapperFactory>();
            var authorized = serverScope.ServiceProvider.GetRequiredService<ReadRemoteAuth>();

            var methods = readFactory.GetType().GetMethods().Where(m => m.Name.StartsWith("Create") || m.Name.StartsWith("Fetch") || m.Name.StartsWith("Can") || m.Name.StartsWith("Try")).ToList();

            foreach (var method in methods)
            {
                object? result;
                var methodName = method.Name;

                int expect = 2;
                if (method.GetParameters().FirstOrDefault()?.ParameterType == typeof(int?))
                {
                    result = method.Invoke(readFactory, new object[] { 1 });
                    expect = 4;
                }
                else
                {
                    result = method.Invoke(readFactory, null);
                }

                if (result is Task<ReadRemoteAuthDataMapper?> task)
                {
                    if (!methodName.Contains("False"))
                    {
                        Assert.IsNotNull(await task);
                    }
                    else
                    {
                        Assert.IsNull(await task);
                    }
                }
                else if (result is Task<Authorized<ReadRemoteAuthDataMapper>> authTask)
                {
                    if (!methodName.Contains("False"))
                    {
                        Assert.IsNotNull((await authTask).Result);
                    }
                    else
                    {
                        Assert.IsNull((await authTask).Result);
                    }
                }
                else if (result is Authorized<ReadRemoteAuthDataMapper> auth)
                {
                    Assert.IsTrue(auth.HasAccess);

                    if (auth.Result == null)
                    {
                        Assert.IsTrue(methodName.StartsWith("Can") || methodName.Contains("False"));
                    }
                    else
                    {
                        Assert.IsTrue(methodName.StartsWith("Try"));
                    }
                }
                else if (result is Task<Authorized> canTask)
                {
                    Assert.IsTrue(methodName.StartsWith("Can"));
                    Assert.IsTrue((await canTask).HasAccess);
                }
                else
                {
                    Assert.Fail();
                }


                if (methodName.Contains("Create"))
                {
                    Assert.AreEqual(expect, authorized.CanCreateCalled);
                }
                else
                {
                    Assert.AreEqual(expect, authorized.CanFetchCalled);
                }

                Assert.AreEqual(expect, authorized.CanReadRemoteCalled);

                authorized.CanCreateCalled = 0;
                authorized.CanFetchCalled = 0;
                authorized.CanReadRemoteCalled = 0;
            }
        }


        [TestMethod]
        public async Task ReadRemoteAuthTaskTest()
        {
            var readFactory = clientScope.ServiceProvider.GetRequiredService<IReadRemoteAuthTaskDataMapperFactory>();
            var authorized = serverScope.ServiceProvider.GetRequiredService<ReadRemoteAuthTask>();

            var methods = readFactory.GetType().GetMethods().Where(m => m.Name.StartsWith("Create") || m.Name.StartsWith("Fetch") || m.Name.StartsWith("Can") || m.Name.StartsWith("Try")).ToList();

            foreach (var method in methods)
            {
                object? result;
                var methodName = method.Name;

                int expect = 2;
                if (method.GetParameters().FirstOrDefault()?.ParameterType == typeof(int?))
                {
                    result = method.Invoke(readFactory, new object[] { 1 });
                    expect = 4;
                }
                else
                {
                    result = method.Invoke(readFactory, null);
                }

                if (result is Task<ReadRemoteAuthTaskDataMapper?> task)
                {
                    if (!methodName.Contains("False"))
                    {
                        Assert.IsNotNull(await task);
                    }
                    else
                    {
                        Assert.IsNull(await task);
                    }
                }
                else if (result is Task<Authorized<ReadRemoteAuthTaskDataMapper>> authTask)
                {
                    if (!methodName.Contains("False"))
                    {
                        Assert.IsNotNull((await authTask).Result);
                    }
                    else
                    {
                        Assert.IsNull((await authTask).Result);
                    }
                }
                else if (result is Authorized<ReadRemoteAuthTaskDataMapper> auth)
                {
                    Assert.IsTrue(auth.HasAccess);

                    if (auth.Result == null)
                    {
                        Assert.IsTrue(methodName.StartsWith("Can") || methodName.Contains("False"));
                    }
                    else
                    {
                        Assert.IsTrue(methodName.StartsWith("Try"));
                    }
                }
                else if (result is Task<Authorized> canTask)
                {
                    Assert.IsTrue(methodName.StartsWith("Can"));
                    Assert.IsTrue((await canTask).HasAccess);
                }
                else if (result is Authorized can)
                {
                    Assert.IsTrue(methodName.StartsWith("Can"));
                    Assert.IsTrue(can.HasAccess);
                }
                else if (result is Task voidTask)
                {
                    await voidTask;
                }
                else
                {
                    Assert.IsTrue(methodName.Contains("False"));
                }


                if (methodName.Contains("Create"))
                {
                    Assert.AreEqual(expect, authorized.CanCreateCalled);
                }
                else
                {
                    Assert.AreEqual(expect, authorized.CanFetchCalled);
                }

                Assert.AreEqual(expect, authorized.CanReadRemoteCalled);

                authorized.CanCreateCalled = 0;
                authorized.CanFetchCalled = 0;
                authorized.CanReadRemoteCalled = 0;
            }
        }



        [TestMethod]
        public async Task ReadRemoteAuthBoolFailTest()
        {
            var readFactory = clientScope.ServiceProvider.GetRequiredService<IReadRemoteAuthDataMapperFactory>();
            var authorized = serverScope.ServiceProvider.GetRequiredService<ReadRemoteAuth>();

            var methods = readFactory.GetType().GetMethods().Where(m => m.Name.StartsWith("Create") || m.Name.StartsWith("Fetch") || m.Name.StartsWith("Can") || m.Name.StartsWith("Try")).ToList();

            foreach (var method in methods)
            {
                object? result;
                var methodName = method.Name;

                if (method.GetParameters().FirstOrDefault()?.ParameterType == typeof(int?))
                {
                    result = method.Invoke(readFactory, new object[] { 10 }); // Fail
                }
                else
                {
                    continue;
                }

                if (result is Task<ReadRemoteAuthDataMapper?> task)
                {
                    Assert.IsNull(await task);
                }
                else if (result is Task<Authorized<ReadRemoteAuthDataMapper>> authTask)
                {
                    var auth = await authTask;
                    Assert.IsFalse(auth.HasAccess);
                    Assert.IsNull(auth.Result);
                    Assert.IsNull(auth.Message);
                }
                else if (result is Authorized<ReadRemoteAuthDataMapper> authDataMapper)
                {
                    Assert.IsFalse(authDataMapper.HasAccess);
                    Assert.IsNull(authDataMapper.Result);
                    Assert.IsNull(authDataMapper.Message);
                }
                else if (result is Task<Authorized> authorizedTask)
                {
                    var a = await authorizedTask;
                    Assert.IsFalse(a.HasAccess);
                    Assert.IsNull(a.Message);
                }
                else
                {
                    Assert.Fail();
                }

            }
        }

        [TestMethod]
        public async Task ReadRemoteAuthStringFailTest()
        {
            var readFactory = clientScope.ServiceProvider.GetRequiredService<IReadRemoteAuthDataMapperFactory>();
            var authorized = serverScope.ServiceProvider.GetRequiredService<ReadRemoteAuth>();

            var methods = readFactory.GetType().GetMethods().Where(m => m.Name.StartsWith("Create") || m.Name.StartsWith("Fetch") || m.Name.StartsWith("Can") || m.Name.StartsWith("Try")).ToList();

            foreach (var method in methods)
            {
                object? result;
                var methodName = method.Name;

                if (method.GetParameters().FirstOrDefault()?.ParameterType == typeof(int?))
                {
                    result = method.Invoke(readFactory, new object[] { 20 }); // Fail
                }
                else
                {
                    continue;
                }

                if (result is Task<ReadRemoteAuthDataMapper?> task)
                {
                    Assert.IsNull(await task);
                }
                else if (result is Task<Authorized<ReadRemoteAuthDataMapper>> authTask)
                {
                    var auth = await authTask;
                    Assert.IsFalse(auth.HasAccess);
                    Assert.IsNull(auth.Result);
                    Assert.AreEqual("Fail", auth.Message);
                }
                else if (result is Authorized<ReadRemoteAuthDataMapper> authDataMapper)
                {
                    Assert.IsFalse(authDataMapper.HasAccess);
                    Assert.IsNull(authDataMapper.Result);
                    Assert.AreEqual("Fail", authDataMapper.Message);
                }
                else if (result is Task<Authorized> authorizedTask)
                {
                    var a = await authorizedTask;
                    Assert.IsFalse(a.HasAccess);
                    Assert.AreEqual("Fail", a.Message);
                }
                else
                {
                    Assert.Fail();
                }

            }
        }
    }
}

