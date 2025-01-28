using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Neatoo.Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neatoo.Netwonsoft.Json.Test.ValidateTests
{

    [TestClass]
    public class FatClientValidateListTests
    {
        IServiceScope scope;
        IValidateObjectList target;
        IValidateObject child;
        FatClientContractResolver resolver;

        [TestInitialize]
        public void TestInitailize()
        {
            scope = AutofacContainer.GetLifetimeScope().Resolve<IServiceScope>();
            target = scope.Resolve<IValidateObjectList>();
            resolver = scope.Resolve<FatClientContractResolver>();

            child = scope.Resolve<IValidateObject>();
            child.ID = Guid.NewGuid();
            child.Name = Guid.NewGuid().ToString();
            target.Add(child);
        }

        [TestMethod]
        public void FatClientListValidate_Serialize()
        {

            var result = Serialize(target);

            Assert.IsTrue(result.Contains(child.ID.ToString()));
            Assert.IsTrue(result.Contains(child.Name));
        }

        [TestMethod]
        public void FatClientListValidate_Serialize_Invalid()
        {
            var result = Serialize(target);

            Assert.IsFalse(target.IsValid);
            Assert.IsTrue(result.Contains("Error")); // Weak check
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

        private IValidateObjectList Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<IValidateObjectList>(json, new JsonSerializerSettings
            {
                ContractResolver = resolver,
                TypeNameHandling = TypeNameHandling.All,
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                Converters = new List<JsonConverter>() { scope.Resolve<ListBaseCollectionConverter>() }
            });
        }

        [TestMethod]
        public void FatClientListValidate_Deserialize()
        {

            var json = Serialize(target);

            var newTarget = Deserialize(json);
        }


        [TestMethod]
        public void FatClientListValidate_Deserialize_Child()
        {

            var json = Serialize(target);

            var newTarget = Deserialize(json);

            Assert.AreEqual(child.ID, newTarget.Single().ID);
            Assert.AreEqual(child.Name, newTarget.Single().Name);

        }

        [TestMethod]
        public void FatClientListValidate_Deserialize_Child_RuleManager()
        {

            child.Name = "Error";
            Assert.IsFalse(child.IsValid);

            var json = Serialize(target);
            var newTarget = Deserialize(json);


            Assert.IsFalse(newTarget.IsValid);
            Assert.IsTrue(newTarget.IsSelfValid);

            Assert.IsFalse(newTarget.Single().IsValid);
            Assert.IsFalse(newTarget.Single().IsSelfValid);
            Assert.AreEqual(child.RuleRunCount, newTarget.Single().RuleRunCount);

        }


    }
}

