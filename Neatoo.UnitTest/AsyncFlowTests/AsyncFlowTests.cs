using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.AsyncFlowTests
{

    [TestClass]
    public class AsyncFlowTests
    {
        AsyncValidateObject asyncValidateObject;
        ConcurrentBag<string> propertyChangedCalls = new ConcurrentBag<string>();
        ConcurrentBag<string> propertyValuePropertyChangedCalls = new ConcurrentBag<string>();

        [TestInitialize]
        public async Task TestInitialize()
        {
            asyncValidateObject = new AsyncValidateObject(new ValidateBaseServices<AsyncValidateObject>());
            asyncValidateObject.Child = new AsyncValidateObject(new ValidateBaseServices<AsyncValidateObject>());
            await asyncValidateObject.WaitForTasks();
            asyncValidateObject.PropertyChanged += AsyncValidateObject_PropertyChanged; 
            Assert.IsFalse(asyncValidateObject.IsBusy);
        }

        [TestCleanup]
        public async Task TestCleanup()
        {
            await asyncValidateObject.WaitForTasks();
            Assert.IsFalse(asyncValidateObject.IsBusy);
            asyncValidateObject.PropertyChanged -= AsyncValidateObject_PropertyChanged;
        }

        private void AsyncValidateObject_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            propertyChangedCalls.Add(e.PropertyName);
        }

        [TestMethod]
        public async Task AsyncFlowTests_CheckAllRules()
        {
            // Just ensure there's not a circular reference
            await asyncValidateObject.RunAllRules(); 

            Assert.AreEqual(5, asyncValidateObject.AsyncDelayRule.RunCount);
            Assert.AreEqual(4, asyncValidateObject.Child.AsyncDelayRule.RunCount);

        }

        [TestMethod]
        public async Task AsyncFlowTests_NoAwait_IsBusy()
        {
            // Need to implement PropertyManager.IsBusy
            // in the same way it is implemented for RulesManager

            asyncValidateObject.AsyncDelayRuleValue = "test";
            Assert.IsTrue(asyncValidateObject.IsBusy);

            while (asyncValidateObject.IsBusy)
            {
                await Task.Yield();
                await Task.Delay(1);
            }

            Assert.AreEqual(3, asyncValidateObject.AsyncDelayRule.RunCount);
            Assert.AreEqual(2, asyncValidateObject.Child.AsyncDelayRule.RunCount);

            //CollectionAssert.Contains(propertyChangedCalls, "IsBusy");
            //CollectionAssert.Contains(propertyChangedCalls, "IsSelfBusy");
        }

        [TestMethod]
        public void AsyncFlowTests_SyncRulesOnly_NotIsBusy()
        {
            asyncValidateObject.SyncA = "test";
            // Should not be busy because there are no async rules
            Assert.IsFalse(asyncValidateObject.IsBusy);
            Assert.AreEqual(1, asyncValidateObject.SyncRuleA.RunCount);
            Assert.AreEqual(1, asyncValidateObject.NestedSyncRuleB.RunCount);

            //CollectionAssert.AreEquivalent(new List<string>() {
            //                        nameof(AsyncValidateObject.NestedSyncB),
            //                        nameof(AsyncValidateObject.SyncA)
            //                         }, propertyChangedCalls.ToList());
        }

        [TestMethod]
        public async Task AsyncFlowTests_CallerCanWaitForPropertyUpdate_NotAllOnly()
        {
            asyncValidateObject.AsyncRuleCanWaitProperty.Value = "test";

            Assert.IsTrue(asyncValidateObject.IsBusy);
            Assert.AreNotEqual("Ran", asyncValidateObject.AsyncRulesCanWaitNested);

            await asyncValidateObject.AsyncRuleCanWaitProperty.Task;

            Assert.IsTrue(asyncValidateObject.IsBusy);
            Assert.AreEqual("Ran", asyncValidateObject.AsyncRulesCanWaitNested);

            await asyncValidateObject.WaitForTasks();
            Assert.IsFalse(asyncValidateObject.IsBusy);
        }

        [TestMethod]
        public async Task AsyncFlowTests_RuleCanWaitForAnotherRule()
        {
            asyncValidateObject.AsyncRuleCanWaitProperty.Value = "Wait";

            Assert.IsTrue(asyncValidateObject.IsBusy);
            Assert.AreNotEqual("Ran", asyncValidateObject.AsyncRulesCanWaitNested);

            await asyncValidateObject.AsyncRuleCanWaitProperty.Task;

            Assert.IsTrue(asyncValidateObject.IsBusy);
            Assert.AreEqual("Ran", asyncValidateObject.AsyncRulesCanWaitNested);

            await asyncValidateObject.WaitForTasks();
            Assert.IsFalse(asyncValidateObject.IsBusy);
        }

        [TestMethod]
        public async Task AsyncFlowTests_Property_PropertyChangedEvents()
        {

            PropertyChangedEventHandler propertyChanged = (s, e) =>
            {
                propertyValuePropertyChangedCalls.Add(e.PropertyName);
            };

            try
            {
                asyncValidateObject.AsyncRuleCanWaitProperty.PropertyChanged += propertyChanged;

                asyncValidateObject.AsyncRuleCanWaitProperty.Value = "Wait";

                await Task.Delay(2);

                Assert.IsTrue(asyncValidateObject.IsBusy);
                Assert.IsTrue(asyncValidateObject.IsSelfBusy);
                CollectionAssert.Contains(propertyChangedCalls, "IsBusy");
                CollectionAssert.Contains(propertyChangedCalls, "IsSelfBusy");
                CollectionAssert.Contains(propertyValuePropertyChangedCalls, "IsBusy");
                CollectionAssert.Contains(propertyValuePropertyChangedCalls, "IsSelfBusy");

                Assert.IsTrue(asyncValidateObject.IsBusy);
                Assert.AreNotEqual("Ran", asyncValidateObject.AsyncRulesCanWaitNested);

                propertyChangedCalls.Clear();
                propertyValuePropertyChangedCalls.Clear();

                await asyncValidateObject.AsyncRuleCanWaitProperty.Task;

                Assert.IsTrue(asyncValidateObject.IsBusy);
                Assert.AreEqual("Ran", asyncValidateObject.AsyncRulesCanWaitNested);

                await asyncValidateObject.WaitForTasks();

                Assert.IsFalse(asyncValidateObject.IsBusy);

                await Task.Delay(5);

                CollectionAssert.Contains(propertyChangedCalls, "IsBusy");
                CollectionAssert.Contains(propertyChangedCalls, "IsSelfBusy");
                CollectionAssert.Contains(propertyValuePropertyChangedCalls, "IsBusy");
                CollectionAssert.Contains(propertyValuePropertyChangedCalls, "IsSelfBusy");

            }
            finally
            {
                asyncValidateObject.AsyncRuleCanWaitProperty.PropertyChanged -= propertyChanged;
            }


        }
    }
}
