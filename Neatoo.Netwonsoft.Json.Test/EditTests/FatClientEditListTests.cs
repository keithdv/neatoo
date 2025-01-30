using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Neatoo.Newtonsoft.Json;
using Neatoo.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neatoo.Netwonsoft.Json.Test.EditTests
{

    [TestClass]
    public class FatClientEditListTests
    {
        IServiceScope scope;
        IEditObjectList target;
        FatClientContractResolver resolver;

        [TestInitialize]
        public void TestInitailize()
        {
            scope = AutofacContainer.GetLifetimeScope().Resolve<IServiceScope>();
            target = scope.Resolve<IEditObjectList>();
            resolver = scope.Resolve<FatClientContractResolver>();
        }

        private string Serialize(object target)
        {
            return JsonConvert.SerializeObject(target, new JsonSerializerSettings()
            {
                ContractResolver = resolver,
                TypeNameHandling = TypeNameHandling.All,
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                Formatting = Formatting.Indented,
                Converters = new List<JsonConverter>() { scope.Resolve<ListBaseCollectionConverter>() }
            });
        }

        private T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
            {
                ContractResolver = resolver,
                TypeNameHandling = TypeNameHandling.All,
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                Formatting = Formatting.Indented,
                Converters = new List<JsonConverter>() { scope.Resolve<ListBaseCollectionConverter>() }
            });
        }

        [TestMethod]
        public void FatClientEditList_Serialize()
        {

            var result = Serialize(target);
        }

        [TestMethod]
        public void FatClientEditList_Deserialize()
        {

            var json = Serialize(target);

            var newTarget = Deserialize<IEditObjectList>(json);
        }

        [TestMethod]
        public void FatClientEditList_Deserialize_Child()
        {
            var child = scope.Resolve<IEditObject>();
            target.Add(child);

            child.ID = Guid.NewGuid();
            child.Name = Guid.NewGuid().ToString();

            var json = Serialize(target);

            var newTarget = Deserialize<IEditObjectList>(json);

            Assert.IsNotNull(newTarget.Single());
            Assert.AreEqual(child.ID, newTarget.Single().ID);
            Assert.AreEqual(child.Name, newTarget.Single().Name);

        }

        [TestMethod]
        public void FatClientEditList_Deserialize_Child_ParentRef()
        {
            var parent = scope.Resolve<IEditObject>();
            parent.ChildList = target;

            var child = scope.Resolve<IEditObject>();
            target.Add(child);

            Assert.AreSame(child.Parent, parent);

            var json = Serialize(parent);

            // ITaskRespository and ILogger constructor parameters are injected by Autofac 
            var newParent = Deserialize<IEditObject>(json);

            Assert.IsNotNull(newParent);
            var newChild = newParent.ChildList.Single();

            Assert.AreSame(child.Parent, parent);

            Assert.AreEqual(child.ID, newChild.ID);
            Assert.AreEqual(child.Name, newChild.Name);
        }

        [TestMethod]
        public void FatClientEditList_IsModified()
        {
            var child = scope.Resolve<IEditObject>();
            target.Add(child);

            child.ID = Guid.NewGuid();
            child.Name = Guid.NewGuid().ToString();

            Assert.IsTrue(target.IsModified);
            Assert.IsFalse(target.IsSelfModified);


            var json = Serialize(target);
            var newTarget = Deserialize<IEditObjectList>(json);

            Assert.IsTrue(newTarget.IsModified);
            Assert.IsFalse(newTarget.IsSelfModified);

        }

        [TestMethod]
        public void FatClientEditList_IsModified_False()
        {
            var child = scope.Resolve<IEditObject>();
            target.Add(child);

            child.ID = Guid.NewGuid();
            child.Name = Guid.NewGuid().ToString();

            child.MarkUnmodified();

            Assert.IsFalse(target.IsModified);
            Assert.IsFalse(target.IsSelfModified);

            var json = Serialize(target);

            var newTarget = Deserialize<IEditObjectList>(json);

            Assert.IsFalse(newTarget.IsModified);
            Assert.IsFalse(newTarget.IsSelfModified);
        }

        [TestMethod]
        public void FatClientEditList_IsNew_False()
        {
            var child = scope.Resolve<IEditObject>();
            target.Add(child);

            child.ID = Guid.NewGuid();
            child.Name = Guid.NewGuid().ToString();
            child.MarkOld();

            Assert.IsFalse(target.IsNew);

            var json = Serialize(target);

            var newTarget = Deserialize<IEditObjectList>(json);

            Assert.IsFalse(newTarget.IsNew);

        }
    }
}

