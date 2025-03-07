using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public class RemoteReadTests
    {

        [Factory]
        public class RemoteReadDataMapper
        {
            public bool CreateCalled { get; set; }

            [Create]
            [Remote]
            public void CreateVoid()
            {
                CreateCalled = true;
            }

            [Create]
            [Remote]
            public bool CreateBool()
            {
                CreateCalled = true;
                return true;
            }

            [Create]
            [Remote]
            public Task CreateTask()
            {
                CreateCalled = true;
                return Task.CompletedTask;
            }

            [Create]
            [Remote]
            public Task<bool> CreateTaskBool()
            {
                CreateCalled = true;
                return Task.FromResult(true);
            }

            [Create]
            [Remote]
            public void CreateVoid(int? param)
            {
                CreateCalled = true;
                Assert.AreEqual(1, param);
            }

            [Create]
            [Remote]
            public bool CreateBool(int? param)
            {
                CreateCalled = true;
                Assert.AreEqual(1, param);
                return true;
            }

            [Create]
            [Remote]
            public Task CreateTask(int? param)
            {
                CreateCalled = true;
                Assert.AreEqual(1, param);
                return Task.CompletedTask;
            }

            [Create]
            [Remote]
            public Task<bool> CreateTaskBool(int? param)
            {
                CreateCalled = true;
                Assert.AreEqual(1, param);
                return Task.FromResult(true);
            }

            [Create]
            [Remote]
            public Task<bool> CreateTaskBoolFalse(int? param)
            {
                CreateCalled = true;
                Assert.AreEqual(1, param);
                return Task.FromResult(false);
            }

            [Create]
            [Remote]
            public void CreateVoidDep([Service] IDisposableDependency disposableDependency)
            {
                CreateCalled = true;
                Assert.IsNotNull(disposableDependency);
            }

            [Create]
            [Remote]
            public bool CreateBoolTrueDep([Service] IDisposableDependency disposableDependency)
            {
                CreateCalled = true;
                Assert.IsNotNull(disposableDependency);
                return true;
            }

            [Create]
            [Remote]
            public bool CreateBoolFalseDep([Service] IDisposableDependency disposableDependency)
            {
                CreateCalled = true;
                Assert.IsNotNull(disposableDependency);
                return false;
            }

            [Create]
            [Remote]
            public Task CreateTaskDep([Service] IDisposableDependency disposableDependency)
            {
                CreateCalled = true;
                Assert.IsNotNull(disposableDependency);
                return Task.CompletedTask;
            }

            [Create]
            [Remote]
            public Task<bool> CreateTaskBoolDep([Service] IDisposableDependency disposableDependency)
            {
                CreateCalled = true;
                Assert.IsNotNull(disposableDependency);
                return Task.FromResult(true);
            }

            [Create]
            [Remote]
            public Task<bool> CreateTaskBoolFalseDep([Service] IDisposableDependency disposableDependency)
            {
                CreateCalled = true;
                Assert.IsNotNull(disposableDependency);
                return Task.FromResult(false);
            }

            [Create]
            [Remote]
            public void CreateVoidDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                CreateCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
            }

            [Create]
            [Remote]
            public bool CreateBoolTrueDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                CreateCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return true;
            }

            [Create]
            [Remote]
            public bool CreateBoolFalseDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                CreateCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return false;
            }

            [Create]
            [Remote]
            public Task CreateTaskDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                CreateCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return Task.CompletedTask;
            }

            [Create]
            [Remote]
            public Task<bool> CreateTaskBoolDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                CreateCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return Task.FromResult(true);
            }

            public bool FetchCalled { get; set; }

            [Fetch]
            [Remote]
            public void FetchVoid()
            {
                FetchCalled = true;
            }

            [Fetch]
            [Remote]
            public bool FetchBool()
            {
                FetchCalled = true;
                return true;
            }

            [Fetch]
            [Remote]
            public Task FetchTask()
            {
                FetchCalled = true;
                return Task.CompletedTask;
            }

            [Fetch]
            [Remote]
            public Task<bool> FetchTaskBool()
            {
                FetchCalled = true;
                return Task.FromResult(true);
            }

            [Fetch]
            [Remote]
            public void FetchVoid(int? param)
            {
                FetchCalled = true;
                Assert.AreEqual(1, param);
            }

            [Fetch]
            [Remote]
            public bool FetchBool(int? param)
            {
                FetchCalled = true;
                Assert.AreEqual(1, param);
                return true;
            }

            [Fetch]
            [Remote]
            public Task FetchTask(int? param)
            {
                FetchCalled = true;
                Assert.AreEqual(1, param);
                return Task.CompletedTask;
            }

            [Fetch]
            [Remote]
            public Task<bool> FetchTaskBool(int? param)
            {
                FetchCalled = true;
                Assert.AreEqual(1, param);
                return Task.FromResult(true);
            }

            [Fetch]
            [Remote]
            public void FetchVoidDep([Service] IDisposableDependency disposableDependency)
            {
                FetchCalled = true;
                Assert.IsNotNull(disposableDependency);
            }

            [Fetch]
            [Remote]
            public bool FetchBoolTrueDep([Service] IDisposableDependency disposableDependency)
            {
                FetchCalled = true;
                Assert.IsNotNull(disposableDependency);
                return true;
            }

            [Fetch]
            [Remote]
            public bool FetchBoolFalseDep([Service] IDisposableDependency disposableDependency)
            {
                FetchCalled = true;
                Assert.IsNotNull(disposableDependency);
                return false;
            }

            [Fetch]
            [Remote]
            public Task FetchTaskDep([Service] IDisposableDependency disposableDependency)
            {
                FetchCalled = true;
                Assert.IsNotNull(disposableDependency);
                return Task.CompletedTask;
            }

            [Fetch]
            [Remote]
            public Task<bool> FetchTaskBoolDep([Service] IDisposableDependency disposableDependency)
            {
                FetchCalled = true;
                Assert.IsNotNull(disposableDependency);
                return Task.FromResult(true);
            }

            [Fetch]
            [Remote]
            public void FetchVoidDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                FetchCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
            }

            [Fetch]
            [Remote]
            public bool FetchBoolTrueDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                FetchCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return true;
            }

            [Fetch]
            [Remote]
            public bool FetchBoolFalseDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                FetchCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return false;
            }

            [Fetch]
            [Remote]
            public Task FetchTaskDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                FetchCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return Task.CompletedTask;
            }

            [Fetch]
            [Remote]
            public Task<bool> FetchTaskBoolDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                FetchCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return Task.FromResult(true);
            }

            [Fetch]
            [Remote]
            public Task<bool> FetchTaskBoolFalseDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                FetchCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return Task.FromResult(false);
            }
        }


        private IServiceScope clientScope;

        [TestInitialize]
        public void TestIntialize()
        {
            var scopes = FactoryContainers.Scopes();
            clientScope = scopes.client;
        }

        [TestMethod]
        public async Task RemoteReadTest()
        {
            var readFactory = clientScope.ServiceProvider.GetRequiredService<IRemoteReadDataMapperFactory>();

            var methods = readFactory.GetType().GetMethods().Where(m => m.Name.StartsWith("Create") || m.Name.StartsWith("Fetch")).ToList();

            foreach (var method in methods)
            {
                object? result;
                var methodName = method.Name;

                if (method.GetParameters().Any())
                {
                    result = method.Invoke(readFactory, new object[] { 1 });
                }
                else
                {
                    result = method.Invoke(readFactory, null);
                }

                if (result is Task<RemoteReadDataMapper?> taskBool)
                {
                    if (method.Name.Contains("False"))
                    {
                        Assert.IsNull(await taskBool);
                    }
                    else
                    {
                        Assert.IsNotNull(await taskBool);
                        Assert.IsTrue(taskBool.Result!.CreateCalled || taskBool.Result!.FetchCalled);
                    }
                }
                else
                {
                    Assert.IsFalse(methodName.Contains("Task"));
                    Assert.IsTrue(methodName.Contains("Bool"));
                    Assert.IsTrue(methodName.Contains("False"));
                    Assert.IsNull(result);
                }

            }
        }
    }
}

