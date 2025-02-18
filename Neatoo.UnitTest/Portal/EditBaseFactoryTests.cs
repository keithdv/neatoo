using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neatoo.Portal;
using Neatoo.Portal.Internal;
using Neatoo.UnitTest.ObjectPortal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.Portal
{

[TestClass]
    public class EditBaseFactoryTests
    {
        public class ServerServiceProvider
        {
            public IServiceProvider serverProvider { get; set; }
        }

        private object lockContainer = new object();
        IServiceProvider serverContainer = null!;
        IServiceProvider clientContainer = null!;

        IServiceScope serverScope = null!;
        IServiceScope clientScope = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            lock (lockContainer)
            {
                if (serverContainer == null)
                {
                    var serverCollection = new ServiceCollection();
                    var clientCollection = new ServiceCollection();

                    serverCollection.AddNeatooServices(NeatooHost.Local, Assembly.GetExecutingAssembly());
                    serverCollection.AddTransient<Objects.IDisposableDependency, Objects.DisposableDependency>();
                    serverCollection.AddScoped<Objects.DisposableDependencyList>();

                    clientCollection.AddNeatooServices(NeatooHost.Remote, Assembly.GetExecutingAssembly());
                    clientCollection.AddScoped<ServerServiceProvider>();
                    clientCollection.AddScoped<ServerServiceProvider>();

                    clientCollection.AddScoped<RequestFromServerDelegate>(cc =>
                    {
                        var serverServiceProvider = cc.GetRequiredService<ServerServiceProvider>().serverProvider;
                        return (RemoteRequest remoteRequest) =>
                        {
                            return serverServiceProvider.GetRequiredService<ServerHandlePortalRequest>()(remoteRequest);
                        };
                    });

                    serverContainer = serverCollection.BuildServiceProvider();
                    clientContainer = clientCollection.BuildServiceProvider();
                }
            }

            serverScope = serverContainer.CreateScope();
            clientScope = clientContainer.CreateScope();

            clientScope.GetRequiredService<ServerServiceProvider>().serverProvider = serverScope.ServiceProvider;
        }

        [TestMethod]
        public async Task EditBaseFactoryTests_IEditObjectCreateEditBaseObject()
        {
            var factory = clientScope.GetRequiredService<EditObjectFactory>();

            var result = await factory.Create();

            Assert.IsTrue(result.CreateCalled);
            Assert.IsTrue(result.IsNew);
            Assert.IsTrue(result.IsModified);
        }

        [TestMethod]
        public async Task EditBaseFactoryTests_IEditObjectCreateEditBaseObjectInt()
        {
            var factory = clientScope.GetRequiredService<EditObjectFactory>();

            var criteria = 10;

            var result = await factory.CreateInt(criteria);

            Assert.AreEqual(criteria, result.IntCriteria);
            Assert.IsTrue(result.IsNew);
            Assert.IsTrue(result.IsModified);
        }

        [TestMethod]
        public async Task EditBaseFactoryTests_EditBaseObjectCreateDependency()
        {
            var factory = clientScope.GetRequiredService<EditObjectFactory>();
            var guidCriteria = Guid.NewGuid();
            var result = await factory.CreateDependency(guidCriteria);

            Assert.IsNotNull(result.GuidCriteria);
            Assert.IsTrue(result.IsNew);
            Assert.IsTrue(result.IsModified);
        }

        [TestMethod]
        public async Task EditBaseFactoryTests_IEditObjectFetchEditBaseObject()
        {
            var factory = clientScope.GetRequiredService<EditObjectFactory>();

            var result = await factory.Fetch();

            Assert.IsNotNull(result.FetchCalled);
            Assert.IsFalse(result.IsNew);
            Assert.IsFalse(result.IsModified);
        }

        [TestMethod]
        public async Task EditBaseFactoryTests_IEditObjectFetchEditBaseObjectGuid()
        {
            var factory = clientScope.GetRequiredService<EditObjectFactory>();

            var guidCriteria = Guid.NewGuid();

            var result = await factory.FetchGuidDependency(guidCriteria);

            Assert.AreEqual(guidCriteria, result.GuidCriteria);
            Assert.IsFalse(result.IsNew);
            Assert.IsFalse(result.IsModified);
        }

        [TestMethod]
        public async Task EditBaseFactoryTests_Save()
        {
            var factory = clientScope.GetRequiredService<EditObjectFactory>();

            var result = await factory.Create();

            result = await factory.Save(result);

            Assert.IsTrue(result.InsertCalled);
            Assert.IsFalse(result.IsNew);
            Assert.IsFalse(result.IsModified);
        }
    }
}
