using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neatoo.Rules;
using Neatoo.UnitTest.PersonObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.ValidateBaseTests
{


    [TestClass]
    public class ValidateListBaseAsyncBaseAsyncTests
    {

        ILifetimeScope scope;
        IValidateAsyncObjectList List;
        IValidateAsyncObject Child;

        [TestInitialize]
        public void TestInitailize()
        {
            scope = AutofacContainer.GetLifetimeScope();
            List = scope.Resolve<IValidateAsyncObjectList>();
            Child = scope.Resolve<IValidateAsyncObject>();
            List.Add(Child);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Assert.IsFalse(List.IsBusy);
            Assert.IsFalse(List.IsSelfBusy);
        }

        [TestMethod]
        public void ValidateListBaseAsync_Constructor()
        {

        }



        [TestMethod]
        public async Task ValidateListBaseAsync_ChildInvalid()
        {
            Child.FirstName = "Error";
            await List.WaitForRules();
            Assert.IsFalse(Child.IsValid);
            Assert.IsFalse(Child.IsSelfValid);
            Assert.IsFalse(List.IsBusy);
            Assert.IsFalse(List.IsValid);
            Assert.IsTrue(List.IsSelfValid);
        }

        [TestMethod]
        public async Task ValidateListBaseAsync_Child_IsBusy()
        {
            Child.FirstName = "Error";

            Assert.IsTrue(List.IsBusy);
            Assert.IsFalse(List.IsSelfBusy);
            Assert.IsTrue(Child.IsBusy);
            Assert.IsTrue(Child.IsSelfBusy);

            await List.WaitForRules();

            Assert.IsFalse(List.IsBusy);
            Assert.IsFalse(List.IsValid);
            Assert.IsTrue(List.IsSelfValid);
            Assert.IsFalse(Child.IsValid);
            Assert.IsFalse(Child.IsSelfValid);
        }

    }
}
