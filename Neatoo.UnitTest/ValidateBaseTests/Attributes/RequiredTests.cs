using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.ValidateBaseTests.Attributes
{
    public class RequiredObject : ValidateBase<RequiredObject>
    {
        public RequiredObject() : base(new ValidateBaseServices<RequiredObject>()) { }

        [Required]
        public string StringValue { get => Getter<string>(); set => Setter(value); }

        [Required]
        public int IntValue { get => Getter<int>(); set => Setter(value); }

        [Required]
        public int? NullableValue { get => Getter<int?>(); set => Setter(value); }

        [Required]
        public List<int> ObjectValue { get => Getter<List<int>>(); set => Setter(value); }

    }

    [TestClass]
    public class RequiredAttributeTests
    {
        private RequiredObject requiredObject;

        [TestInitialize]
        public void TestInitialize()
        {
            requiredObject = new RequiredObject();
        }

        [TestMethod]
        public async Task RequiredAttribute_InValid()
        {

            await requiredObject.RunAllRules();
            Assert.IsFalse(requiredObject.IsValid);
        }

        [TestMethod]
        public void RequiredAttribute_Valid()
        {

            requiredObject.StringValue = "test";
            requiredObject.IntValue = 1;
            requiredObject.NullableValue = 1;
            requiredObject.ObjectValue = new List<int> { 1, 2, 3 };

            Assert.IsTrue(requiredObject.IsValid);
        }
    }
}
