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
using static Neatoo.UnitTest.Portal.ReadRemoteAuthTests;

namespace Neatoo.UnitTest.Portal
{

    [TestClass]
    public class ReadAuthTests
    {

        public class ReadAuthTask : ReadAuth
        {
            [Authorize(DataMapperMethodType.Read)]
            public Task<bool> CanReadBoolTask()
            {
                CanReadCalled++;
                return Task.FromResult(true);
            }

            [Authorize(DataMapperMethodType.Read)]
            public Task<bool> CanReadBoolFalseTask(int? p)
            {
                CanReadCalled++;
                if (p == 10)
                {
                    return Task.FromResult(false);
                }
                return Task.FromResult(true);
            }

            [Authorize(DataMapperMethodType.Read)]
            public Task<string> CanReadStringTask()
            {
                CanReadCalled++;
                return Task.FromResult(string.Empty);
            }

            [Authorize(DataMapperMethodType.Read)]
            public Task<string> CanReadStringFalseTask(int? p)
            {
                CanReadCalled++;
                if (p == 20)
                {
                    return Task.FromResult("Fail");
                }
                return Task.FromResult(string.Empty);
            }

            [Authorize(DataMapperMethodType.Create)]
            public Task<bool> CanCreateBoolTask()
            {
                CanCreateCalled++;
                return Task.FromResult(true);
            }

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

            [Authorize(DataMapperMethodType.Create)]
            public Task<string> CanCreateStringTask()
            {
                CanCreateCalled++;
                return Task.FromResult(string.Empty);
            }

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

            [Authorize(DataMapperMethodType.Fetch)]
            public Task<bool> CanFetchBoolTask()
            {
                CanFetchCalled++;
                return Task.FromResult(true);
            }

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

            [Authorize(DataMapperMethodType.Fetch)]
            public Task<string> CanFetchStringTask()
            {
                CanFetchCalled++;
                return Task.FromResult(string.Empty);
            }

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

        public class ReadAuth
        {
            public int CanReadCalled { get; set; }

            [Authorize(DataMapperMethodType.Read)]
            public bool CanReadBool()
            {
                CanReadCalled++;
                return true;
            }

            [Authorize(DataMapperMethodType.Read)]
            public bool CanReadBoolFalse(int? p)
            {
                CanReadCalled++;
                if (p == 10)
                {
                    return false;
                }
                return true;
            }

            [Authorize(DataMapperMethodType.Read)]
            public string? CanReadString()
            {
                CanReadCalled++;
                return string.Empty;
            }

            [Authorize(DataMapperMethodType.Read)]
            public string? CanReadStringFalse(int? p)
            {
                CanReadCalled++;
                if (p == 20)
                {
                    return "Fail";
                }
                return string.Empty;
            }

            public int CanCreateCalled { get; set; }

            [Authorize(DataMapperMethodType.Create)]
            public bool CanCreateBool()
            {
                CanCreateCalled++;
                return true;
            }

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

            [Authorize(DataMapperMethodType.Create)]
            public string? CanCreateString()
            {
                CanCreateCalled++;
                return string.Empty;
            }

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

            [Authorize(DataMapperMethodType.Fetch)]
            public bool CanFetchBool()
            {
                CanFetchCalled++;
                return true;
            }

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

            [Authorize(DataMapperMethodType.Fetch)]
            public string? CanFetchString()
            {
                CanFetchCalled++;
                return string.Empty;
            }

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
        [Authorize<ReadAuth>]
        public class ReadAuthDataMapper : ReadTests.ReadDataMapper
        {

        }

        [Factory]
        [Authorize<ReadAuthTask>]
        public class ReadAuthTaskDataMapper : ReadTests.ReadDataMapper
        {

        }

        private IServiceScope clientScope;

        [TestInitialize]
        public void TestIntialize()
        {
            var scopes = FactoryContainers.Scopes();
            clientScope = scopes.client;
        }

        [TestMethod]
        public async Task ReadAuthTest()
        {
            var readFactory = clientScope.ServiceProvider.GetRequiredService<IReadAuthDataMapperFactory>();
            var authorized = clientScope.ServiceProvider.GetRequiredService<ReadAuth>();

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

                if (result is Task<ReadAuthDataMapper?> task)
                {
                    Assert.IsTrue(methodName.Contains("Task"));
                    if (!methodName.Contains("False"))
                    {
                        Assert.IsNotNull(await task);
                    }
                    else
                    {
                        Assert.IsNull(await task);
                    }
                }
                else if (result is Task<Authorized<ReadAuthDataMapper>> authTask)
                {
                    Assert.IsTrue(methodName.Contains("Task"));

                    if (!methodName.Contains("False"))
                    {
                        Assert.IsNotNull((await authTask).Result);
                    }
                    else
                    {
                        Assert.IsNull((await authTask).Result);
                    }
                }
                else if (result is Authorized<ReadAuthDataMapper> auth)
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
                else if (result is ReadAuthDataMapper success)
                {
                    Assert.IsFalse(methodName.Contains("False"));
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

                Assert.AreEqual(expect, authorized.CanReadCalled);

                authorized.CanCreateCalled = 0;
                authorized.CanFetchCalled = 0;
                authorized.CanReadCalled = 0;
            }
        }


        [TestMethod]
        public async Task ReadAuthTaskTest()
        {
            var readFactory = clientScope.ServiceProvider.GetRequiredService<IReadAuthTaskDataMapperFactory>();
            var authorized = clientScope.ServiceProvider.GetRequiredService<ReadAuthTask>();

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

                if (result is Task<ReadAuthTaskDataMapper?> task)
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
                else if (result is Task<Authorized<ReadAuthTaskDataMapper>> authTask)
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
                else if (result is Authorized<ReadAuthTaskDataMapper> auth)
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

                Assert.AreEqual(expect, authorized.CanReadCalled);

                authorized.CanCreateCalled = 0;
                authorized.CanFetchCalled = 0;
                authorized.CanReadCalled = 0;
            }
        }


        [TestMethod]
        public async Task ReadAuthBoolFailTest()
        {
            var readFactory = clientScope.ServiceProvider.GetRequiredService<IReadAuthDataMapperFactory>();
            var authorized = clientScope.ServiceProvider.GetRequiredService<ReadAuth>();

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

                if (result is Task<ReadAuthDataMapper?> task)
                {
                    Assert.IsNull(await task);
                    Assert.IsTrue(methodName.StartsWith("Create") || methodName.Contains("Fetch"));
                }
                else if (result is Task<Authorized<ReadAuthDataMapper>> authTask)
                {
                    var auth = await authTask;
                    Assert.IsFalse(auth.HasAccess);
                    Assert.IsNull(auth.Result);
                    Assert.IsNull(auth.Message);
                    Assert.IsTrue(methodName.StartsWith("Try"));
                }
                else if (result is Authorized<ReadAuthDataMapper> authDataMapper)
                {
                    Assert.IsFalse(authDataMapper.HasAccess);
                    Assert.IsNull(authDataMapper.Result);
                    Assert.IsNull(authDataMapper.Message);
                    Assert.IsTrue(methodName.StartsWith("Can") || methodName.StartsWith("Try"));
                }
                else if (result is Authorized auth_)
                {
                    Assert.IsFalse(auth_.HasAccess);
                    Assert.IsTrue(methodName.StartsWith("Can"));
                }
                else if (result == null)
                {
                    Assert.IsTrue(methodName.StartsWith("Create") || methodName.Contains("Fetch"));
                }

            }
        }

        [TestMethod]
        public async Task ReadAuthStringFailTest()
        {
            var readFactory = clientScope.ServiceProvider.GetRequiredService<IReadAuthDataMapperFactory>();
            var authorized = clientScope.ServiceProvider.GetRequiredService<ReadAuth>();

            var methods = readFactory.GetType().GetMethods().Where(m => m.Name == "CanCreateVoid").ToList(); // m.Name.StartsWith("Create") || m.Name.StartsWith("Fetch") || m.Name.StartsWith("Can") || m.Name.StartsWith("Try")).ToList();

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

                if (result is Task<ReadAuthDataMapper?> task)
                {
                    Assert.IsNull(await task);
                    Assert.IsTrue(methodName.StartsWith("Create") || methodName.Contains("Fetch"));
                }
                else if (result is Task<Authorized<ReadAuthDataMapper>> authTask)
                {
                    var auth = await authTask;
                    Assert.IsFalse(auth.HasAccess);
                    Assert.IsNull(auth.Result);
                    Assert.AreEqual("Fail", auth.Message);
                    Assert.IsTrue(methodName.StartsWith("Try"));
                }
                else if (result is Authorized<ReadAuthDataMapper> authDataMapper)
                {
                    Assert.IsFalse(authDataMapper.HasAccess);
                    Assert.IsNull(authDataMapper.Result);
                    Assert.AreEqual("Fail", authDataMapper.Message);
                    Assert.IsTrue(methodName.StartsWith("Can") || methodName.StartsWith("Try"));
                }
                else if (result is Authorized auth_)
                {
                    Assert.IsFalse(auth_.HasAccess);
                    Assert.AreEqual("Fail", auth_.Message);
                    Assert.IsTrue(methodName.StartsWith("Can"));
                }
                else if (result == null)
                {
                    Assert.IsTrue(methodName.StartsWith("Create") || methodName.Contains("Fetch"));
                }

            }
        }
    }
}

