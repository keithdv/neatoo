using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neatoo.Internal;
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
    public class RemoteWriteDataMapperTests
    {

        [Factory]
        public class RemoteWriteDataMapper : IEditMetaSaveProperties
        {
            public bool IsDeleted { get; set; }

            public bool IsNew { get; set; }

            public bool InsertCalled { get; set; }
            [Insert]
            [Remote]
            public void InsertVoid()
            {
                InsertCalled = true;
            }

            [Insert]
            [Remote]
            public bool InsertBool()
            {
                InsertCalled = true;
                return true;
            }

            [Insert]
            [Remote]
            public Task InsertTask()
            {
                InsertCalled = true;
                return Task.CompletedTask;
            }

            [Insert]
            [Remote]
            public Task<bool> InsertTaskBool()
            {
                InsertCalled = true;
                return Task.FromResult(true);
            }

            [Insert]
            [Remote]
            public void InsertVoid(int? param)
            {
                InsertCalled = true;
                Assert.AreEqual(1, param);
            }

            [Insert]
            [Remote]
            public bool InsertBool(int? param)
            {
                InsertCalled = true;
                Assert.AreEqual(1, param);
                return true;
            }

            [Insert]
            [Remote]
            public Task InsertTask(int? param)
            {
                InsertCalled = true;
                Assert.AreEqual(1, param);
                return Task.CompletedTask;
            }

            [Insert]
            [Remote]
            public Task<bool> InsertTaskBool(int? param)
            {
                InsertCalled = true;
                Assert.AreEqual(1, param);
                return Task.FromResult(true);
            }

            [Insert]
            [Remote]
            public Task<bool> InsertTaskBoolFalse(int? param)
            {
                InsertCalled = true;
                Assert.AreEqual(1, param);
                return Task.FromResult(false);
            }

            [Insert]
            [Remote]
            public void InsertVoidDep([Service] IDisposableDependency disposableDependency)
            {
                InsertCalled = true;
                Assert.IsNotNull(disposableDependency);
            }

            [Insert]
            [Remote]
            public bool InsertBoolTrueDep([Service] IDisposableDependency disposableDependency)
            {
                InsertCalled = true;
                Assert.IsNotNull(disposableDependency);
                return true;
            }

            [Insert]
            [Remote]
            public bool InsertBoolFalseDep([Service] IDisposableDependency disposableDependency)
            {
                InsertCalled = true;
                Assert.IsNotNull(disposableDependency);
                return false;
            }

            [Insert]
            [Remote]
            public Task InsertTaskDep([Service] IDisposableDependency disposableDependency)
            {
                InsertCalled = true;
                Assert.IsNotNull(disposableDependency);
                return Task.CompletedTask;
            }

            [Insert]
            [Remote]
            public Task<bool> InsertTaskBoolDep([Service] IDisposableDependency disposableDependency)
            {
                InsertCalled = true;
                Assert.IsNotNull(disposableDependency);
                return Task.FromResult(true);
            }

            [Insert]
            [Remote]
            public Task<bool> InsertTaskBoolFalseDep([Service] IDisposableDependency disposableDependency)
            {
                InsertCalled = true;
                Assert.IsNotNull(disposableDependency);
                return Task.FromResult(false);
            }

            [Insert]
            [Remote]
            public void InsertVoidDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                InsertCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
            }

            [Insert]
            [Remote]
            public bool InsertBoolTrueDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                InsertCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return true;
            }

            [Insert]
            [Remote]
            public bool InsertBoolFalseDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                InsertCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return false;
            }

            [Insert]
            [Remote]
            public Task InsertTaskDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                InsertCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return Task.CompletedTask;
            }

            [Insert]
            [Remote]
            public Task<bool> InsertTaskBoolDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                InsertCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return Task.FromResult(true);
            }



            public bool UpdateCalled { get; set; }
            [Update]
            [Remote]
            public void UpdateVoid()
            {
                UpdateCalled = true;
            }

            [Update]
            [Remote]
            public bool UpdateBool()
            {
                UpdateCalled = true;
                return true;
            }

            [Update]
            [Remote]
            public Task UpdateTask()
            {
                UpdateCalled = true;
                return Task.CompletedTask;
            }

            [Update]
            [Remote]
            public Task<bool> UpdateTaskBool()
            {
                UpdateCalled = true;
                return Task.FromResult(true);
            }

            [Update]
            [Remote]
            public void UpdateVoid(int? param)
            {
                UpdateCalled = true;
                Assert.AreEqual(1, param);
            }

            [Update]
            [Remote]
            public bool UpdateBool(int? param)
            {
                UpdateCalled = true;
                Assert.AreEqual(1, param);
                return true;
            }

            [Update]
            [Remote]
            public Task UpdateTask(int? param)
            {
                UpdateCalled = true;
                Assert.AreEqual(1, param);
                return Task.CompletedTask;
            }

            [Update]
            [Remote]
            public Task<bool> UpdateTaskBool(int? param)
            {
                UpdateCalled = true;
                Assert.AreEqual(1, param);
                return Task.FromResult(true);
            }

            [Update]
            [Remote]
            public Task<bool> UpdateTaskBoolFalse(int? param)
            {
                UpdateCalled = true;
                Assert.AreEqual(1, param);
                return Task.FromResult(false);
            }

            [Update]
            [Remote]
            public void UpdateVoidDep([Service] IDisposableDependency disposableDependency)
            {
                UpdateCalled = true;
                Assert.IsNotNull(disposableDependency);
            }

            [Update]
            [Remote]
            public bool UpdateBoolTrueDep([Service] IDisposableDependency disposableDependency)
            {
                UpdateCalled = true;
                Assert.IsNotNull(disposableDependency);
                return true;
            }

            [Update]
            [Remote]
            public bool UpdateBoolFalseDep([Service] IDisposableDependency disposableDependency)
            {
                UpdateCalled = true;
                Assert.IsNotNull(disposableDependency);
                return false;
            }

            [Update]
            [Remote]
            public Task UpdateTaskDep([Service] IDisposableDependency disposableDependency)
            {
                UpdateCalled = true;
                Assert.IsNotNull(disposableDependency);
                return Task.CompletedTask;
            }

            [Update]
            [Remote]
            public Task<bool> UpdateTaskBoolDep([Service] IDisposableDependency disposableDependency)
            {
                UpdateCalled = true;
                Assert.IsNotNull(disposableDependency);
                return Task.FromResult(true);
            }

            [Update]
            [Remote]
            public Task<bool> UpdateTaskBoolFalseDep([Service] IDisposableDependency disposableDependency)
            {
                UpdateCalled = true;
                Assert.IsNotNull(disposableDependency);
                return Task.FromResult(false);
            }

            [Update]
            [Remote]
            public void UpdateVoidDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                UpdateCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
            }

            [Update]
            [Remote]
            public bool UpdateBoolTrueDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                UpdateCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return true;
            }

            [Update]
            [Remote]
            public bool UpdateBoolFalseDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                UpdateCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return false;
            }

            [Update]
            [Remote]
            public Task UpdateTaskDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                UpdateCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return Task.CompletedTask;
            }

            [Update]
            [Remote]
            public Task<bool> UpdateTaskBoolDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                UpdateCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return Task.FromResult(true);
            }

            public bool DeleteCalled { get; set; }
            [Delete]
            [Remote]
            public void DeleteVoid()
            {
                DeleteCalled = true;
            }

            [Delete]
            [Remote]
            public bool DeleteBool()
            {
                DeleteCalled = true;
                return true;
            }

            [Delete]
            [Remote]
            public Task DeleteTask()
            {
                DeleteCalled = true;
                return Task.CompletedTask;
            }

            [Delete]
            [Remote]
            public Task<bool> DeleteTaskBool()
            {
                DeleteCalled = true;
                return Task.FromResult(true);
            }

            [Delete]
            [Remote]
            public void DeleteVoid(int? param)
            {
                DeleteCalled = true;
                Assert.AreEqual(1, param);
            }

            [Delete]
            [Remote]
            public bool DeleteBool(int? param)
            {
                DeleteCalled = true;
                Assert.AreEqual(1, param);
                return true;
            }

            [Delete]
            [Remote]
            public Task DeleteTask(int? param)
            {
                DeleteCalled = true;
                Assert.AreEqual(1, param);
                return Task.CompletedTask;
            }

            [Delete]
            [Remote]
            public Task<bool> DeleteTaskBool(int? param)
            {
                DeleteCalled = true;
                Assert.AreEqual(1, param);
                return Task.FromResult(true);
            }

            [Delete]
            [Remote]
            public Task<bool> DeleteTaskBoolFalse(int? param)
            {
                DeleteCalled = true;
                Assert.AreEqual(1, param);
                return Task.FromResult(false);
            }

            [Delete]
            [Remote]
            public void DeleteVoidDep([Service] IDisposableDependency disposableDependency)
            {
                DeleteCalled = true;
                Assert.IsNotNull(disposableDependency);
            }

            [Delete]
            [Remote]
            public bool DeleteBoolTrueDep([Service] IDisposableDependency disposableDependency)
            {
                DeleteCalled = true;
                Assert.IsNotNull(disposableDependency);
                return true;
            }

            [Delete]
            [Remote]
            public bool DeleteBoolFalseDep([Service] IDisposableDependency disposableDependency)
            {
                DeleteCalled = true;
                Assert.IsNotNull(disposableDependency);
                return false;
            }

            [Delete]
            [Remote]
            public Task DeleteTaskDep([Service] IDisposableDependency disposableDependency)
            {
                DeleteCalled = true;
                Assert.IsNotNull(disposableDependency);
                return Task.CompletedTask;
            }

            [Delete]
            [Remote]
            public Task<bool> DeleteTaskBoolDep([Service] IDisposableDependency disposableDependency)
            {
                DeleteCalled = true;
                Assert.IsNotNull(disposableDependency);
                return Task.FromResult(true);
            }

            [Delete]
            [Remote]
            public Task<bool> DeleteTaskBoolFalseDep([Service] IDisposableDependency disposableDependency)
            {
                DeleteCalled = true;
                Assert.IsNotNull(disposableDependency);
                return Task.FromResult(false);
            }

            [Delete]
            [Remote]
            public void DeleteVoidDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                DeleteCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
            }

            [Delete]
            [Remote]
            public bool DeleteBoolTrueDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                DeleteCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return true;
            }

            [Delete]
            [Remote]
            public bool DeleteBoolFalseDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                DeleteCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return false;
            }

            [Delete]
            [Remote]
            public Task DeleteTaskDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                DeleteCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return Task.CompletedTask;
            }

            [Delete]
            [Remote]
            public Task<bool> DeleteTaskBoolDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                DeleteCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return Task.FromResult(true);
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
        public async Task RemoteWriteDataMapperTest()
        {
            var readFactory = clientScope.ServiceProvider.GetRequiredService<IRemoteWriteDataMapperFactory>();

            var methods = readFactory.GetType().GetMethods().Where(m => m.Name.StartsWith("Save")).ToList();

            foreach (var method in methods)
            {
                object? result;
                var methodName = method.Name;

                async Task<RemoteWriteDataMapper?> doSave(RemoteWriteDataMapper writeDataMapper)
                {
                    if (method.GetParameters().Count() == 2)
                    {
                        result = method.Invoke(readFactory, [writeDataMapper, 1]);
                    }
                    else
                    {
                        result = method.Invoke(readFactory, [writeDataMapper]);
                    }

                    if (result is Task<RemoteWriteDataMapper?> taskBool)
                    {
                        if (method.Name.Contains("False"))
                        {
                            Assert.IsNull(await taskBool);
                        }
                        else
                        {
                            Assert.IsNotNull(await taskBool);
                        }
                        return taskBool.Result;
                    }
                    else
                    {
                        Assert.IsTrue(methodName.Contains("Bool"));
                        Assert.IsTrue(methodName.Contains("False"));
                        Assert.IsNull(result);
                        return null;
                    }
                }

                var writeDataMapperToSave = new RemoteWriteDataMapper();
                var writeDataMapper = await doSave(writeDataMapperToSave);
                Assert.IsTrue(writeDataMapper?.UpdateCalled ?? true);

                writeDataMapperToSave = new RemoteWriteDataMapper() { IsNew = true };
                writeDataMapper = await doSave(writeDataMapperToSave);
                Assert.IsTrue(writeDataMapper?.InsertCalled ?? true);

                writeDataMapperToSave = new RemoteWriteDataMapper() { IsDeleted = true };
                writeDataMapper = await doSave(writeDataMapperToSave);
                Assert.IsTrue(writeDataMapper?.DeleteCalled ?? true);

            }
        }
    }
}


