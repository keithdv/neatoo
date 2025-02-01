using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Neatoo.Portal;

namespace Neatoo.UnitTest.SystemTextJson.BaseTests
{

    [TestClass]
    public class FatClientBaseTests
    {
        IServiceScope scope;
        IBaseObject target;
        Guid Id = Guid.NewGuid();
        string Name = Guid.NewGuid().ToString();
        NeatooJsonSerializer resolver;

        [TestInitialize]
        public void TestInitailize()
        {
            scope = AutofacContainer.GetLifetimeScope().Resolve<IServiceScope>();
            target = scope.Resolve<IBaseObject>();
            target.ID = Id;
            target.Name = Name;
            resolver = scope.Resolve<NeatooJsonSerializer>();
        }

        private string Serialize(object target)
        {
            return resolver.Serialize(target);
        }

        private IBaseObject Deserialize(string json)
        {
            return resolver.Deserialize<IBaseObject>(json);
        }

        [TestMethod]
        public void FatClientBaseTests_Serialize()
        {

            var result = Serialize(target);

            Assert.IsTrue(result.Contains(Id.ToString()));
            Assert.IsTrue(result.Contains(Name));
        }

        [TestMethod]
        public void FatClientBaseTests_Deserialize()
        {

            var json = Serialize(target);

            var newTarget = Deserialize(json);

            Assert.AreEqual(target.ID, newTarget.ID);
            Assert.AreEqual(target.Name, newTarget.Name);
        }

        [TestMethod]
        public void FatClientBaseTests_Deserialize_Child()
        {

            var child = target.Child = scope.Resolve<IBaseObject>();

            child.ID = Guid.NewGuid();
            child.Name = Guid.NewGuid().ToString();

            var json = Serialize(target);

            // ITaskRespository and ILogger constructor parameters are injected by Autofac 
            var newTarget = Deserialize(json);


            Assert.IsNotNull(newTarget.Child);
            Assert.AreEqual(child.ID, newTarget.Child.ID);
            Assert.AreEqual(child.Name, newTarget.Child.Name);

        }

        [TestMethod]
        public void FatClientBaseTests_Deserialize_Child_ParentRef()
        {

            var child = target.Child = scope.Resolve<IBaseObject>();

            child.ID = Guid.NewGuid();
            child.Name = Guid.NewGuid().ToString();

            var json = Serialize(target);

            // ITaskRespository and ILogger constructor parameters are injected by Autofac 
            var newTarget = Deserialize(json);


            Assert.IsNotNull(newTarget.Child);
            Assert.AreEqual(child.ID, newTarget.Child.ID);
            Assert.AreEqual(child.Name, newTarget.Child.Name);
            Assert.AreSame(newTarget.Child.Parent, newTarget);
             
        }

    }
}

