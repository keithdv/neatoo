using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neatoo.Portal;
using Neatoo.UnitTest.Objects;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.Portal
{
    public interface IMethodObject
    {
        Task<int> DoRemoteWork(int number);
    }

    public class MethodObject : IMethodObject
    {
        private IRemoteMethodPortal<Execute> Method { get; }

        public MethodObject(IRemoteMethodPortal<Execute> method)
        {
            Method = method;
        }

        // This entire approach is driven off of the fact that while the Delegate is not serializable
        // The Type definition of the Delegate is and we can resolve that Type from the container
        public delegate Task<int> Execute(int number);

        /// <summary>
        /// This will be called on the server (when not a unit test)
        /// </summary>
        /// <param name="number"></param>
        /// <param name="dependency"></param>
        /// <returns></returns>
        internal static Task<int> ExecuteServer(int number, IDisposableDependency dependency)
        {
            Assert.IsNotNull(dependency);
            return Task.FromResult(number * 10);
        }

        public Task<int> DoRemoteWork(int number)
        {
            return Method.Execute<int>(number);
        }
    }

    [TestClass]
    public class LocalMethodPortalTests
    {
        private IServiceScope scope;

        [TestInitialize]
        public void TestInitialize()
        {
            scope = UnitTestServices.GetLifetimeScope(true);
        }

        [TestMethod]
        public async Task LocalMethodPortal_ExecuteServer()
        {
            // Hide the fact that there is a remote call from the client
            var methodObject = scope.GetRequiredService<IMethodObject>();

            var result = await methodObject.DoRemoteWork(20);

            Assert.AreEqual(200, result);
        }

        [TestMethod]
        public async Task LocalMethodPortal_Execute()
        {
            var portal = scope.GetRequiredService<IRemoteMethodPortal<MethodObject.Execute>>();

            var result = await portal.Execute<int>(10);

            Assert.AreEqual(100, result);
        }



    }
}
