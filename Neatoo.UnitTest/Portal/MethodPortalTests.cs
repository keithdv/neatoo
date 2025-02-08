using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Neatoo.UnitTest.Portal
{

    public class IMethodPortal<S> where S : Delegate
    {


    }

    public class MethodPortal<S> where S : Delegate
    {

        public MethodPortal(S method)
        {
            Method = method;
        }

        public S Method { get; }

        public T Execute<P, T>(P param)
        {
            return (T)Method.Method.Invoke(Method.Target, new object[1] { param });
        }
    }

    [TestClass]
    public class MethodPortalTests
    {
        private IServiceProvider container;

        public delegate bool RemoteMethod(int number);

        public static bool RemoteMethod_(int number)
        {
            return number == 10;
        }


        [TestInitialize]
        public void TestInitialize()
        {
            var containerBuilder = new ServiceCollection();
            containerBuilder.AddTransient<RemoteMethod>(cc =>
            {
                return i => RemoteMethod_(i);
            });
            containerBuilder.AddTransient(typeof(MethodPortal<>));
            container = containerBuilder.BuildServiceProvider();
        }

        [TestMethod]
        public void MethodPortalTest()
        {
            var mp = container.GetRequiredService<MethodPortal<RemoteMethod>>();
            var result = mp.Execute<int, bool>(10);
        }
    }
}

