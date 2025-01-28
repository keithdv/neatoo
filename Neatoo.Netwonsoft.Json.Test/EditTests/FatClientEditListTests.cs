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

        private IEditObjectList Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<IEditObjectList>(json, new JsonSerializerSettings
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

            var newTarget = Deserialize(json);
        }

        //[TestMethod]
        //public void FatClientEditList_Deserialize_Child()
        //{

        //    var child = target.Child = scope.Resolve<IEditObject>();

        //    child.ID = Guid.NewGuid();
        //    child.Name = Guid.NewGuid().ToString();

        //    var json = Serialize(target);

        //    var newTarget = Deserialize(json);

        //    Assert.IsNotNull(newTarget.Child);
        //    Assert.AreEqual(child.ID, newTarget.Child.ID);
        //    Assert.AreEqual(child.Name, newTarget.Child.Name);

        //}

        //[TestMethod]
        //public void FatClientEditList_Deserialize_Child_ParentRef()
        //{

        //    var child = target.Child = scope.Resolve<IEditObject>();

        //    child.ID = Guid.NewGuid();
        //    child.Name = Guid.NewGuid().ToString();
        //    child.Parent = target;

        //    var json = Serialize(target);

        //    // ITaskRespository and ILogger constructor parameters are injected by Autofac 
        //    var newTarget = Deserialize(json);

        //    Assert.IsNotNull(newTarget.Child);
        //    Assert.AreEqual(child.ID, newTarget.Child.ID);
        //    Assert.AreEqual(child.Name, newTarget.Child.Name);
        //    Assert.AreSame(newTarget.Child.Parent, newTarget);
             
        //}

        [TestMethod]
        public void FatClientEditList_IsModified()
        {

            var json = Serialize(target);

            var newTarget = Deserialize(json);

            Assert.IsTrue(newTarget.IsModified);
            Assert.IsTrue(newTarget.IsSelfModified);

        }

        [TestMethod]
        public void FatClientEditList_IsModified_False()
        {

            target.MarkUnmodified();
            var json = Serialize(target);

            var newTarget = Deserialize(json);

            Assert.IsFalse(newTarget.IsModified);
            Assert.IsFalse(newTarget.IsSelfModified);

        }

        [TestMethod]
        public void FatClientEditList_IsNew()
        {

            target.MarkNew();
            var json = Serialize(target);

            var newTarget = Deserialize(json);

            Assert.IsTrue(newTarget.IsNew);

        }

        [TestMethod]
        public void FatClientEditList_IsNew_False()
        {

            target.MarkOld();

            var json = Serialize(target);

            var newTarget = Deserialize(json);

            Assert.IsFalse(newTarget.IsNew);

        }

        [TestMethod]
        public void FatClientEditList_IsChild()
        {

            target.MarkAsChild();

            var json = Serialize(target);

            var newTarget = Deserialize(json);

            Assert.IsTrue(newTarget.IsChild);

        }

        [TestMethod]
        public void FatClientEditList_IsChild_False()
        {

            var json = Serialize(target);

            var newTarget = Deserialize(json);

            Assert.IsFalse(newTarget.IsChild);

        }

    }
}

