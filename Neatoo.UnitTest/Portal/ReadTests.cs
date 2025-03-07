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
    public class ReadTests
    {

        [Factory]
        public class ReadDataMapper
        {
            public bool CreateCalled { get; set; }

            [Create]

            public void CreateVoid()
            {
                CreateCalled = true;
            }

            [Create]

            public bool CreateBool()
            {
                CreateCalled = true;
                return true;
            }

            [Create]

            public Task CreateTask()
            {
                CreateCalled = true;
                return Task.CompletedTask;
            }

            [Create]

            public Task<bool> CreateTaskBool()
            {
                CreateCalled = true;
                return Task.FromResult(true);
            }

            [Create]

            public void CreateVoid(int? param)
            {
                CreateCalled = true;
                Assert.AreEqual(1, param);
            }

            [Create]

            public bool CreateBool(int? param)
            {
                CreateCalled = true;
                Assert.AreEqual(1, param);
                return true;
            }

            [Create]

            public Task CreateTask(int? param)
            {
                CreateCalled = true;
                Assert.AreEqual(1, param);
                return Task.CompletedTask;
            }

            [Create]

            public Task<bool> CreateTaskBool(int? param)
            {
                CreateCalled = true;
                Assert.AreEqual(1, param);
                return Task.FromResult(true);
            }

            [Create]

            public Task<bool> CreateTaskBoolFalse(int? param)
            {
                CreateCalled = true;
                Assert.AreEqual(1, param);
                return Task.FromResult(false);
            }

            [Create]

            public void CreateVoidDep([Service] IDisposableDependency disposableDependency)
            {
                CreateCalled = true;
                Assert.IsNotNull(disposableDependency);
            }

            [Create]

            public bool CreateBoolTrueDep([Service] IDisposableDependency disposableDependency)
            {
                CreateCalled = true;
                Assert.IsNotNull(disposableDependency);
                return true;
            }

            [Create]

            public bool CreateBoolFalseDep([Service] IDisposableDependency disposableDependency)
            {
                CreateCalled = true;
                Assert.IsNotNull(disposableDependency);
                return false;
            }

            [Create]

            public Task CreateTaskDep([Service] IDisposableDependency disposableDependency)
            {
                CreateCalled = true;
                Assert.IsNotNull(disposableDependency);
                return Task.CompletedTask;
            }

            [Create]

            public Task<bool> CreateTaskBoolDep([Service] IDisposableDependency disposableDependency)
            {
                CreateCalled = true;
                Assert.IsNotNull(disposableDependency);
                return Task.FromResult(true);
            }

            [Create]

            public Task<bool> CreateTaskBoolFalseDep([Service] IDisposableDependency disposableDependency)
            {
                CreateCalled = true;
                Assert.IsNotNull(disposableDependency);
                return Task.FromResult(false);
            }

            [Create]

            public void CreateVoidDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                CreateCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
            }

            [Create]

            public bool CreateBoolTrueDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                CreateCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return true;
            }

            [Create]

            public bool CreateBoolFalseDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                CreateCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return false;
            }

            [Create]

            public Task CreateTaskDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                CreateCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return Task.CompletedTask;
            }

            [Create]

            public Task<bool> CreateTaskBoolDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                CreateCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return Task.FromResult(true);
            }

            public bool FetchCalled { get; set; }

            [Fetch]

            public void FetchVoid()
            {
                FetchCalled = true;
            }

            [Fetch]

            public bool FetchBool()
            {
                FetchCalled = true;
                return true;
            }

            [Fetch]

            public Task FetchTask()
            {
                FetchCalled = true;
                return Task.CompletedTask;
            }

            [Fetch]

            public Task<bool> FetchTaskBool()
            {
                FetchCalled = true;
                return Task.FromResult(true);
            }

            [Fetch]

            public void FetchVoid(int? param)
            {
                FetchCalled = true;
                Assert.AreEqual(1, param);
            }

            [Fetch]

            public bool FetchBool(int? param)
            {
                FetchCalled = true;
                Assert.AreEqual(1, param);
                return true;
            }

            [Fetch]

            public Task FetchTask(int? param)
            {
                FetchCalled = true;
                Assert.AreEqual(1, param);
                return Task.CompletedTask;
            }

            [Fetch]

            public Task<bool> FetchTaskBool(int? param)
            {
                FetchCalled = true;
                Assert.AreEqual(1, param);
                return Task.FromResult(true);
            }

            [Fetch]

            public void FetchVoidDep([Service] IDisposableDependency disposableDependency)
            {
                FetchCalled = true;
                Assert.IsNotNull(disposableDependency);
            }

            [Fetch]

            public bool FetchBoolTrueDep([Service] IDisposableDependency disposableDependency)
            {
                FetchCalled = true;
                Assert.IsNotNull(disposableDependency);
                return true;
            }

            [Fetch]

            public bool FetchBoolFalseDep([Service] IDisposableDependency disposableDependency)
            {
                FetchCalled = true;
                Assert.IsNotNull(disposableDependency);
                return false;
            }

            [Fetch]

            public Task FetchTaskDep([Service] IDisposableDependency disposableDependency)
            {
                FetchCalled = true;
                Assert.IsNotNull(disposableDependency);
                return Task.CompletedTask;
            }

            [Fetch]

            public Task<bool> FetchTaskBoolDep([Service] IDisposableDependency disposableDependency)
            {
                FetchCalled = true;
                Assert.IsNotNull(disposableDependency);
                return Task.FromResult(true);
            }

            [Fetch]

            public void FetchVoidDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                FetchCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
            }

            [Fetch]

            public bool FetchBoolTrueDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                FetchCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return true;
            }

            [Fetch]

            public bool FetchBoolFalseDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                FetchCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return false;
            }

            [Fetch]

            public Task FetchTaskDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                FetchCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return Task.CompletedTask;
            }

            [Fetch]

            public Task<bool> FetchTaskBoolDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                FetchCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return Task.FromResult(true);
            }

            [Fetch]

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
        public async Task ReadFactoryTest()
        {
            var readFactory = clientScope.ServiceProvider.GetRequiredService<IReadDataMapperFactory>();

            var methods = readFactory.GetType().GetMethods().Where(m => m.Name.Contains("Create") || m.Name.Contains("Fetch")).ToList();

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

                if (result is Task<ReadDataMapper?> task)
                {
                    Assert.IsTrue(methodName.Contains("Task"));
                    if (method.Name.Contains("False"))
                    {
                        Assert.IsNull(await task);
                    }
                    else
                    {
                        Assert.IsNotNull(await task);
                        Assert.IsTrue(task.Result!.CreateCalled || task.Result!.FetchCalled);
                    }
                }
                else if (result is ReadDataMapper r)
                {
                    Assert.IsFalse(methodName.Contains("Task"));
                    Assert.IsFalse(methodName.Contains("False"));
                    Assert.IsNotNull(r);
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

