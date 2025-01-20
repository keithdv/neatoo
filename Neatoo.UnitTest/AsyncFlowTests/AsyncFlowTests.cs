﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
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
        public void TestInitialize()
        {
            asyncValidateObject = new AsyncValidateObject(new ValidateBaseServices<AsyncValidateObject>());
            asyncValidateObject.PropertyChanged += AsyncValidateObject_PropertyChanged; 
            Assert.IsFalse(asyncValidateObject.IsBusy);
        }

        [TestCleanup]
        public async Task TestCleanup()
        {
            await asyncValidateObject.WaitForRules();
            Assert.IsFalse(asyncValidateObject.IsBusy);
            asyncValidateObject.PropertyChanged -= AsyncValidateObject_PropertyChanged;
        }

        private void AsyncValidateObject_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            propertyChangedCalls.Add(e.PropertyName);
        }

        [TestMethod]
        public void AsyncFlowTests_NoAwait_IsBusy()
        {
            // Need to implement PropertyValueManager.IsBusy
            // in the same way it is implemented for RulesManager

            asyncValidateObject.HasAsyncRules = "test";
            Assert.AreEqual(1, asyncValidateObject.AsyncDelayRule.RunCount);
            Assert.IsTrue(asyncValidateObject.IsBusy);
            
        }

        [TestMethod]
        public void AsyncFlowTests_SyncRulesOnly_NotIsBusy()
        {
            asyncValidateObject.SyncA = "test";
            // Should not be busy because there are no async rules
            Assert.IsFalse(asyncValidateObject.IsBusy);
            Assert.AreEqual(1, asyncValidateObject.SyncRuleA.RunCount);
            Assert.AreEqual(1, asyncValidateObject.NestedSyncRuleB.RunCount);

            CollectionAssert.AreEquivalent(new List<string>() {
                                    nameof(AsyncValidateObject.NestedSyncB),
                                    nameof(AsyncValidateObject.SyncA)
                                     }, propertyChangedCalls.ToList());
        }

    }
}
