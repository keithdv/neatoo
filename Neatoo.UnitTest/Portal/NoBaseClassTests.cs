using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neatoo.AuthorizationRules;
using Neatoo.Portal;
using Neatoo.Portal.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.Portal
{

    public interface INoBaseClass
    {
        string Name { get; set; }
    }

    public interface INoBaseClassA : INoBaseClass
    {
        string Name { get; set; }
    }

    [Factory]
    public class NoBaseClassA : INoBaseClassA
    {
        public string Name { get; set; }

        [Create]
        public void Create(string name)
        {
            Name = name;
        }

        [Remote]
        [Create]
        public void CreateRemote(string name)
        {
            Name = name;
        }
    }

    public interface INoBaseClassB : INoBaseClass
    {
        string Name { get; set; }
    }

    [Factory]
    public class NoBaseClassB : INoBaseClassB
    {
        public string Name { get; set; }
        public string Value { get; set; }

        [Create]
        public void Create(string name)
        {
            Name = name;
        }


        [Remote]
        [Create]
        public void CreateRemote(string name)
        {
            Name = name;
        }
    }

    public interface INoBaseClassAList : IList<INoBaseClassA>
    {
    }

    [Factory]
    public class NoBaseClassAList : List<INoBaseClassA>, INoBaseClassAList
    {
        [Create]
        public void Create([Service] INoBaseClassAFactory factoryA)
        {
            Add(factoryA.Create(Guid.NewGuid().ToString()));
            Add(factoryA.Create(Guid.NewGuid().ToString()));
            Add(this[0]);
        }


        [Remote]
        [Create]
        public void CreateRemote([Service] INoBaseClassAFactory factoryA)
        {
            Add(factoryA.Create(Guid.NewGuid().ToString()));
            Add(factoryA.Create(Guid.NewGuid().ToString()));
            Add(this[0]);
        }
    }


    public interface INoBaseClassList : IList<INoBaseClass>
    {
    }

    [Factory]
    public class NoBaseClassList : List<INoBaseClass>, INoBaseClassList
    {
        [Create]
        public void Create([Service] INoBaseClassAFactory factoryA,
                            [Service] INoBaseClassBFactory factoryB)
        {
            Add(factoryA.Create(Guid.NewGuid().ToString()));
            Add(factoryB.Create(Guid.NewGuid().ToString()));
            Add(this[0]);
        }


        [Remote]
        [Create]
        public void CreateRemote([Service] INoBaseClassAFactory factoryA,
                            [Service] INoBaseClassBFactory factoryB)
        {
            Add(factoryA.Create(Guid.NewGuid().ToString()));
            Add(factoryB.Create(Guid.NewGuid().ToString()));
            Add(this[0]);
        }
    }



    [TestClass]
    public class NoBaseClassTests
    {
        private IServiceScope serverScope;
        private IServiceScope clientScope;

        [TestInitialize]
        public void TestIntialize()
        {
            var scopes = FactoryContainers.Scopes();
            serverScope = scopes.server;
            clientScope = scopes.client;
        }

        [TestMethod]
        public void NoBaseClassTests_SerializeInterface_INoBaseClass()
        {
            var guidStr = Guid.NewGuid().ToString();
            var obj = new NoBaseClassA() { Name = guidStr };
            var neatooJsonSerializer = serverScope.ServiceProvider.GetRequiredService<INeatooJsonSerializer>();

            var json = neatooJsonSerializer.Serialize(obj, typeof(INoBaseClass));
        }

        [TestMethod]
        public void NoBaseClassTests_DeSerialize_INoBaseClassA()
        {
            var guidStr = Guid.NewGuid().ToString();
            var obj = new NoBaseClassA() { Name = guidStr };
            var neatooJsonSerializer = serverScope.ServiceProvider.GetRequiredService<INeatooJsonSerializer>();

            var json = neatooJsonSerializer.Serialize(obj, typeof(INoBaseClassA));

            var result = neatooJsonSerializer.Deserialize<INoBaseClassA>(json);

            Assert.AreNotSame(obj, result);
            Assert.AreEqual(obj.Name, result.Name);
        }

        [TestMethod]
        public void NoBaseClassTests_Create()
        {
            var factory = clientScope.GetRequiredService<INoBaseClassAFactory>();
            var guid = Guid.NewGuid().ToString();
            var result = factory.Create(guid);

            Assert.AreEqual(guid, result.Name);
        }

        [TestMethod]
        public async Task NoBaseClassTests_CreateRemote()
        {
            var factory = clientScope.GetRequiredService<INoBaseClassBFactory>();

            var guid = Guid.NewGuid().ToString();
            var result = await factory.CreateRemote(guid);

            Assert.AreEqual(guid, result.Name);
        }


        [TestMethod]
        public void NoBaseClassAListTests_Create()
        {
            var factory = clientScope.GetRequiredService<INoBaseClassAListFactory>();
            var result = factory.Create();
            Assert.AreEqual(3, result.Count);
            Assert.AreSame(result[0], result[2]);
        }

        [TestMethod]
        public async Task NoBaseClassAListTests_CreateRemote()
        {
            var factory = clientScope.GetRequiredService<INoBaseClassAListFactory>();
            var result = await factory.CreateRemote();
            Assert.AreEqual(3, result.Count);
            Assert.IsInstanceOfType<NoBaseClassA>(result[0]);
            Assert.IsInstanceOfType<NoBaseClassA>(result[1]);
            Assert.AreSame(result[0], result[2]);
        }

        [TestMethod]
        public void NoBaseClassListTests_Create()
        {
            var factory = clientScope.GetRequiredService<INoBaseClassListFactory>();
            var result = factory.Create();
            Assert.AreEqual(3, result.Count);
            Assert.AreSame(result[0], result[2]);
        }

        [TestMethod]
        public async Task NoBaseClassListTests_CreateRemote()
        {
            var factory = clientScope.GetRequiredService<INoBaseClassListFactory>();
            var result = await factory.CreateRemote();
            Assert.AreEqual(3, result.Count);
            Assert.IsInstanceOfType<NoBaseClassA>(result[0]);
            Assert.IsInstanceOfType<NoBaseClassB>(result[1]);
            Assert.AreSame(result[0], result[2]);
        }
    }
}
