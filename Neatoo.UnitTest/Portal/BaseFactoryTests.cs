using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neatoo.Portal;
using Neatoo.Portal.Internal;
using Neatoo.UnitTest.ObjectPortal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.Portal
{
    [TestClass]
    public class BaseFactoryTests
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
        public async Task BaseFactoryTests_IBaseObjectCreateBaseObject()
        {
            var factory = clientScope.GetRequiredService<BaseObjectFactory>();

            var result = await factory.Create();

            Assert.IsTrue(result.CreateCalled);
        }

        [TestMethod]
        public async Task BaseFactoryTests_IBaseObjectCreateBaseObjectInt()
        {
            var factory = clientScope.GetRequiredService<BaseObjectFactory>();

            var criteria = 10;

            var result = await factory.CreateInt(criteria);

            Assert.AreEqual(criteria, result.IntCriteria);
        }

        [TestMethod]
        public async Task BaseFactoryTests_BaseObjectCreateDependency()
        {
            var factory = clientScope.GetRequiredService<BaseObjectFactory>();

            var result = await factory.CreateDependency(2, 10d);

            Assert.IsNotNull(result.MultipleCriteria);
        }

        [TestMethod]
        public async Task BaseFactoryTests_IBaseObjectFetchBaseObject()
        {
            var factory = clientScope.GetRequiredService<BaseObjectFactory>();

            var result = await factory.Fetch();

            Assert.IsNotNull(result.FetchCalled);
        }

        [TestMethod]
        public async Task BaseFactoryTests_IBaseObjectFetchBaseObjectGuid()
        {
            var factory = clientScope.GetRequiredService<BaseObjectFactory>();

            var guidCriteria = Guid.NewGuid();

            var result = await factory.FetchGuidDependency(guidCriteria);

            Assert.AreEqual(guidCriteria, result.GuidCriteria);
        }
    }
}
