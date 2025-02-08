﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using Neatoo.Portal;

namespace Neatoo.UnitTest.SystemTextJson.BaseTests
{

    [TestClass]
    public class FatClientListBaseTests
    {
        IServiceScope scope;
        IBaseObjectList target;
        NeatooJsonSerializer resolver;
        IBaseObject child;

        [TestInitialize]
        public void TestInitailize()
        {
            scope = UnitTestServices.GetLifetimeScope();
            target = scope.GetRequiredService<IBaseObjectList>();
            resolver = scope.GetRequiredService<NeatooJsonSerializer>();

            child = scope.GetRequiredService<IBaseObject>();
            child.ID = Guid.NewGuid();
            child.Name = Guid.NewGuid().ToString();
            target.Add(child);
        }

        private string Serialize(object target)
        {
            return resolver.Serialize(target);
        }

        private IBaseObjectList Deserialize(string json)
        {
            return resolver.Deserialize<IBaseObjectList>(json);
        }

        [TestMethod]
        public void FatClientListBaseTests_Serialize()
        {

            var result = Serialize(target);

            Assert.IsTrue(result.Contains(child.ID.ToString()));
            Assert.IsTrue(result.Contains(child.Name));
        }

        [TestMethod]
        public void FatClientListBaseTests_Deserialize()
        {

            var json = Serialize(target);

            var newTarget = Deserialize(json);

        }

        [TestMethod]
        public void FatClientListBaseTests_Deserialize_Child()
        {

            var json = Serialize(target);

            // ITaskRespository and ILogger constructor parameters are injected by Autofac 
            var newTarget = Deserialize(json);


            Assert.IsNotNull(newTarget.SingleOrDefault());
            Assert.AreEqual(child.ID, newTarget.Single().ID);
            Assert.AreEqual(child.Name, newTarget.Single().Name);

        }

        [TestMethod]
        public void FatClientListBaseTests_Deserialize_Modify()
        {

            var json = Serialize(target);

            // ITaskRespository and ILogger constructor parameters are injected by Autofac 
            var newTarget = Deserialize(json);

            var newId = Guid.NewGuid();
        }

    }
}

