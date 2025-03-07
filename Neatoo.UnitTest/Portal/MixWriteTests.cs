using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.EventHandlers;
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
    public class MixedReturnTypeWriteTests
    {

        [Factory]
        public class MixedReturnTypeWriteDataMapper : IEditMetaSaveProperties
        {
            public bool IsDeleted { get; set; }

            public bool IsNew { get; set; }

            public bool InsertCalled { get; set; }
            [Insert]
            public void InsertVoid()
            {
                InsertCalled = true;
            }

            [Insert]
            public bool InsertBool()
            {
                InsertCalled = true;
                return true;
            }

            [Insert]
            public Task InsertTask()
            {
                InsertCalled = true;
                return Task.CompletedTask;
            }

            [Insert]
            public Task<bool> InsertTaskBool()
            {
                InsertCalled = true;
                return Task.FromResult(true);
            }

            [Insert]
            public void InsertVoid(int? param)
            {
                InsertCalled = true;
                Assert.AreEqual(1, param);
            }

            [Insert]
            public bool InsertBool(int? param)
            {
                InsertCalled = true;
                Assert.AreEqual(1, param);
                return true;
            }

            [Insert]
            public Task InsertTask(int? param)
            {
                InsertCalled = true;
                Assert.AreEqual(1, param);
                return Task.CompletedTask;
            }

            [Insert]
            public Task<bool> InsertTaskBool(int? param)
            {
                InsertCalled = true;
                Assert.AreEqual(1, param);
                return Task.FromResult(true);
            }

            [Insert]
            public Task<bool> InsertTaskBoolFalse(int? param)
            {
                InsertCalled = true;
                Assert.AreEqual(1, param);
                return Task.FromResult(false);
            }

            [Insert]
            public void InsertVoidDep([Service] IDisposableDependency disposableDependency)
            {
                InsertCalled = true;
                Assert.IsNotNull(disposableDependency);
            }

            [Insert]
            public bool InsertBoolTrueDep([Service] IDisposableDependency disposableDependency)
            {
                InsertCalled = true;
                Assert.IsNotNull(disposableDependency);
                return true;
            }

            [Insert]
            public bool InsertBoolFalseDep([Service] IDisposableDependency disposableDependency)
            {
                InsertCalled = true;
                Assert.IsNotNull(disposableDependency);
                return false;
            }

            [Insert]
            public Task InsertTaskDep([Service] IDisposableDependency disposableDependency)
            {
                InsertCalled = true;
                Assert.IsNotNull(disposableDependency);
                return Task.CompletedTask;
            }

            [Insert]
            public Task<bool> InsertTaskBoolDep([Service] IDisposableDependency disposableDependency)
            {
                InsertCalled = true;
                Assert.IsNotNull(disposableDependency);
                return Task.FromResult(true);
            }

            [Insert]
            public Task<bool> InsertTaskBoolFalseDep([Service] IDisposableDependency disposableDependency)
            {
                InsertCalled = true;
                Assert.IsNotNull(disposableDependency);
                return Task.FromResult(false);
            }

            [Insert]
            public void InsertVoidDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                InsertCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
            }

            [Insert]
            public bool InsertBoolTrueDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                InsertCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return true;
            }

            [Insert]
            public bool InsertBoolFalseDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                InsertCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return false;
            }

            [Insert]
            public Task InsertTaskDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                InsertCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return Task.CompletedTask;
            }

            [Insert]
            public Task<bool> InsertTaskBoolDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                InsertCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return Task.FromResult(true);
            }



            public bool UpdateCalled { get; set; }
            [Update]
            public void UpdateVoid()
            {
                UpdateCalled = true;
            }

            [Update]
            public Task<bool> UpdateBool()
            {
                UpdateCalled = true;
                return Task.FromResult(true);
            }

            [Update]
            public Task UpdateTask()
            {
                UpdateCalled = true;
                return Task.CompletedTask;
            }

            [Update]
            public Task<bool> UpdateTaskBool()
            {
                UpdateCalled = true;
                return Task.FromResult(true);
            }

            [Update]
            public void UpdateVoid(int? param)
            {
                UpdateCalled = true;
                Assert.AreEqual(1, param);
            }

            [Update]
            public void UpdateBool(int? param)
            {
                UpdateCalled = true;
                Assert.AreEqual(1, param);
            }

            [Update]
            public void UpdateTask(int? param)
            {
                UpdateCalled = true;
                Assert.AreEqual(1, param);
            }

            [Update]
            public void UpdateTaskBool(int? param)
            {
                UpdateCalled = true;
                Assert.AreEqual(1, param);
            }

            [Update]
            public void UpdateTaskBoolFalse(int? param)
            {
                UpdateCalled = true;
                Assert.AreEqual(1, param);
            }

            [Update]
            public void UpdateVoidDep([Service] IDisposableDependency disposableDependency)
            {
                UpdateCalled = true;
                Assert.IsNotNull(disposableDependency);
            }

            [Remote]
            [Update]
            public bool UpdateBoolTrueDep([Service] IDisposableDependency disposableDependency)
            {
                UpdateCalled = true;
                Assert.IsNotNull(disposableDependency);
                return true;
            }

            [Update]
            public bool UpdateBoolFalseDep([Service] IDisposableDependency disposableDependency)
            {
                UpdateCalled = true;
                Assert.IsNotNull(disposableDependency);
                return false;
            }

            [Update]
            public Task UpdateTaskDep([Service] IDisposableDependency disposableDependency)
            {
                UpdateCalled = true;
                Assert.IsNotNull(disposableDependency);
                return Task.CompletedTask;
            }

            [Update]
            public void UpdateTaskBoolDep([Service] IDisposableDependency disposableDependency)
            {
                UpdateCalled = true;
                Assert.IsNotNull(disposableDependency);
            }

            [Update]
            public Task<bool> UpdateTaskBoolFalseDep([Service] IDisposableDependency disposableDependency)
            {
                UpdateCalled = true;
                Assert.IsNotNull(disposableDependency);
                return Task.FromResult(false);
            }

            [Update]
            public void UpdateVoidDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                UpdateCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
            }

            [Remote]
            [Update]
            public bool UpdateBoolTrueDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                UpdateCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return true;
            }

            [Update]
            public Task<bool> UpdateBoolFalseDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                UpdateCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return Task.FromResult(false);
            }

            [Update]
            public Task UpdateTaskDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                UpdateCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return Task.CompletedTask;
            }

            [Update]
            public Task<bool> UpdateTaskBoolDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                UpdateCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return Task.FromResult(true);
            }

            public bool DeleteCalled { get; set; }
            [Delete]
            public void DeleteVoid()
            {
                DeleteCalled = true;
            }

            [Delete]
            public bool DeleteBool()
            {
                DeleteCalled = true;
                return true;
            }

            [Delete]
            public Task DeleteTask()
            {
                DeleteCalled = true;
                return Task.CompletedTask;
            }

            [Delete]
            public Task<bool> DeleteTaskBool()
            {
                DeleteCalled = true;
                return Task.FromResult(true);
            }

            [Delete]
            public void DeleteVoid(int? param)
            {
                DeleteCalled = true;
                Assert.AreEqual(1, param);
            }

            [Remote]
            [Delete]
            public bool DeleteBool(int? param)
            {
                DeleteCalled = true;
                Assert.AreEqual(1, param);
                return true;
            }

            [Delete]
            public void DeleteTask(int? param)
            {
                DeleteCalled = true;
                Assert.AreEqual(1, param);
            }

            [Delete]
            public Task DeleteTaskBool(int? param)
            {
                DeleteCalled = true;
                Assert.AreEqual(1, param);
                return Task.FromResult(true);
            }

            [Delete]
            public Task<bool> DeleteTaskBoolFalse(int? param)
            {
                DeleteCalled = true;
                Assert.AreEqual(1, param);
                return Task.FromResult(false);
            }

            [Remote]
            [Delete]
            public void DeleteVoidDep([Service] IDisposableDependency disposableDependency)
            {
                DeleteCalled = true;
                Assert.IsNotNull(disposableDependency);
            }

            [Delete]
            public bool DeleteBoolTrueDep([Service] IDisposableDependency disposableDependency)
            {
                DeleteCalled = true;
                Assert.IsNotNull(disposableDependency);
                return true;
            }

            [Delete]
            public bool DeleteBoolFalseDep([Service] IDisposableDependency disposableDependency)
            {
                DeleteCalled = true;
                Assert.IsNotNull(disposableDependency);
                return false;
            }

            [Delete]
            public Task DeleteTaskDep([Service] IDisposableDependency disposableDependency)
            {
                DeleteCalled = true;
                Assert.IsNotNull(disposableDependency);
                return Task.CompletedTask;
            }

            [Delete]
            public bool DeleteTaskBoolDep([Service] IDisposableDependency disposableDependency)
            {
                DeleteCalled = true;
                Assert.IsNotNull(disposableDependency);
                return true;
            }

            [Delete]
            public void DeleteTaskBoolFalseDep([Service] IDisposableDependency disposableDependency)
            {
                DeleteCalled = true;
                Assert.IsNotNull(disposableDependency);
            }

            [Delete]
            public Task<bool> DeleteVoidDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                DeleteCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return Task.FromResult(true);
            }

            [Delete]
            public void DeleteBoolTrueDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                DeleteCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
            }

            [Remote]
            [Delete]
            public bool DeleteBoolFalseDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                DeleteCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return false;
            }

            [Remote]
            [Delete]
            public Task DeleteTaskDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                DeleteCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
                return Task.CompletedTask;
            }

            [Remote]
            [Delete]
            public void DeleteTaskBoolDep(int? param, [Service] IDisposableDependency disposableDependency)
            {
                DeleteCalled = true;
                Assert.AreEqual(1, param);
                Assert.IsNotNull(disposableDependency);
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
        public async Task MixedReturnTypeWriteDataMapperTest()
        {
            var readFactory = clientScope.ServiceProvider.GetRequiredService<IMixedReturnTypeWriteDataMapperFactory>();

            var methods = readFactory.GetType().GetMethods().Where(m => m.Name.StartsWith("Save")).ToList();

            foreach (var method in methods)
            {
                object? result;
                var methodName = method.Name;

                async Task<MixedReturnTypeWriteDataMapper?> doSave(MixedReturnTypeWriteDataMapper writeDataMapper)
                {
                    if (method.GetParameters().Count() == 2)
                    {
                        result = method.Invoke(readFactory, [writeDataMapper, 1]);
                    }
                    else
                    {
                        result = method.Invoke(readFactory, [writeDataMapper]);
                    }

                    if (result is Task<MixedReturnTypeWriteDataMapper?> taskBool)
                    {
                        return await taskBool;
                    }
                    else if (result is MixedReturnTypeWriteDataMapper r)
                    {
                        Assert.IsNotNull(r);
                        return r;
                    }
                    else
                    {
                        Assert.IsNull(result);
                        return null;
                    }
                }

                var writeDataMapperToSave = new MixedReturnTypeWriteDataMapper();
                var writeDataMapper = await doSave(writeDataMapperToSave);
                Assert.IsTrue(writeDataMapper?.UpdateCalled ?? true);

                writeDataMapperToSave = new MixedReturnTypeWriteDataMapper() { IsNew = true };
                writeDataMapper = await doSave(writeDataMapperToSave);
                Assert.IsTrue(writeDataMapper?.InsertCalled ?? true);

                writeDataMapperToSave = new MixedReturnTypeWriteDataMapper() { IsDeleted = true };
                writeDataMapper = await doSave(writeDataMapperToSave);
                Assert.IsTrue(writeDataMapper?.DeleteCalled ?? true);

            }
        }
    }
}


