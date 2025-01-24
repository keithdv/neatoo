using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neatoo.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neatoo.UnitTest
{
    [TestClass]
    public class AsyncTaskSequencerTests
    {

        [TestMethod]
        public async Task AsyncTaskSequencer_AsyncForks()
        {
            var sequencer = new AsyncTaskSequencer();

            bool completedA = false;
            bool completedB = false;
            bool completedC = false;

            Func<Task> funcA = async () =>
            {
                await Task.Delay(5);
                Assert.IsFalse(completedA);
                Assert.IsFalse(completedB);
                Assert.IsFalse(completedC);
                completedA = true;
            };

            Func<Task> funcB = async () =>
            {
                await Task.Delay(10);
                Assert.IsTrue(completedA);
                Assert.IsFalse(completedB);
                Assert.IsFalse(completedC);
                completedB = true;
            };

            Func<Task> funcC = async () =>
            {
                await Task.Delay(15);
                Assert.IsTrue(completedA);
                Assert.IsTrue(completedB);
                Assert.IsFalse(completedC);
                completedC = true;
            };

            sequencer.AddTask(funcA);
            sequencer.AddTask(funcB);
            sequencer.AddTask(funcC);

            await sequencer.AllDone;

            Assert.IsTrue(completedA);
            Assert.IsTrue(completedB);
            Assert.IsTrue(completedC);
        }



        [TestMethod]
        public void AsyncTaskSequencer_NoAsyncForks()
        {
            var sequencer = new AsyncTaskSequencer();

            bool completedA = false;
            bool completedB = false;
            bool completedC = false;

            Func<Task> funcA = () =>
            {
                Assert.IsFalse(completedA);
                Assert.IsFalse(completedB);
                Assert.IsFalse(completedC);
                completedA = true;
                return Task.CompletedTask;
            };

            Func<Task> funcB = () =>
            {
                Assert.IsTrue(completedA);
                Assert.IsFalse(completedB);
                Assert.IsFalse(completedC);
                completedB = true;
                return Task.CompletedTask;
            };

            Func<Task> funcC = () =>
            {
                Assert.IsTrue(completedA);
                Assert.IsTrue(completedB);
                Assert.IsFalse(completedC);
                completedC = true;
                return Task.CompletedTask;
            };

            sequencer.AddTask(funcA);
            sequencer.AddTask(funcB);
            sequencer.AddTask(funcC);

            // Since there were no async forks
            // should not have to await
            // (In ValidateBase this means don't have to await AllRulesDone)
            Assert.IsTrue(completedA);
            Assert.IsTrue(completedB);
            Assert.IsTrue(completedC);
        }



        [TestMethod]
        public async Task AsyncTaskSequencer_AsyncForkException()
        {
            var sequencer = new AsyncTaskSequencer();

            bool completedA = false;
            bool completedB = false;
            bool completedC = false;
            Exception exception = new Exception("Test");

            Func<Task> funcA = async () =>
            {
                await Task.Delay(5);
                Assert.IsFalse(completedA);
                Assert.IsFalse(completedB);
                Assert.IsFalse(completedC);
                completedA = true;
            };

            Func<Task> funcB = async () =>
            {
                await Task.Delay(5);
                Assert.IsTrue(completedA);
                Assert.IsFalse(completedB);
                Assert.IsFalse(completedC);
                completedB = true;
                throw exception;
            };

            Func<Task> funcC = async () =>
            {
                await Task.Delay(5);
                Assert.IsTrue(completedA);
                Assert.IsTrue(completedB);
                Assert.IsFalse(completedC);
                completedC = true;
            };

            sequencer.AddTask(funcA);
            sequencer.AddTask(funcB);
            sequencer.AddTask(funcC);

            try
            {
                await sequencer.AllDone;
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.InnerException, exception);
            }
        }


        [TestMethod]
        public async Task AsyncTaskSequencer_NonAwaitedAsyncFork()
        {

            // This is the core issue
            // We update a property thru the setter
            // that has async rules
            // The setter does not await the async rules

            var sequencer = new AsyncTaskSequencer();

            bool completedA = false;
            bool completedB = false;
            bool completedC = false;

            Func<Task> funcC = async () =>
            {
                await Task.Delay(5);
                Assert.IsTrue(completedA);
                Assert.IsTrue(completedB);
                Assert.IsFalse(completedC);
                completedC = true;
            };

            Func<Task> funcB = async () =>
            {
                await Task.Delay(5);
                sequencer.AddTask(funcC);
                Assert.IsTrue(completedA);
                Assert.IsFalse(completedB);
                Assert.IsFalse(completedC);
                completedB = true;
            };

            Action funcA = () =>
            {
                sequencer.AddTask(funcB); // Not awaited - This is the core difficulty
                Assert.IsFalse(completedA);
                Assert.IsFalse(completedB);
                Assert.IsFalse(completedC);
                completedA = true;
            };

            funcA();

            Assert.IsTrue(completedA);
            Assert.IsFalse(completedB);
            Assert.IsFalse(completedC);

            await sequencer.AllDone;

            Assert.IsTrue(completedA);
            Assert.IsTrue(completedB);
            Assert.IsTrue(completedC);
        }


        [TestMethod]
        public void AsyncTaskSequencer_NoAsyncForks_KeepTask()
        {
            var sequencer = new AsyncTaskSequencer();

            bool completedA = false;
            bool completedB = false;
            bool completedC = false;

            Func<Task> funcA = () =>
            {
                Assert.IsFalse(completedA);
                Assert.IsFalse(completedB);
                Assert.IsFalse(completedC);
                completedA = true;
                return Task.CompletedTask;
            };

            Func<Task> funcB = () =>
            {
                Assert.IsTrue(completedA);
                Assert.IsFalse(completedB);
                Assert.IsFalse(completedC);
                completedB = true;
                return Task.CompletedTask;
            };

            Func<Task> funcC = () =>
            {
                Assert.IsTrue(completedA);
                Assert.IsTrue(completedB);
                Assert.IsFalse(completedC);
                completedC = true;
                return Task.CompletedTask;
            };

            Assert.IsTrue(sequencer.AddTask(funcA).IsCompleted);
            Assert.IsTrue(sequencer.AddTask(funcB).IsCompleted);
            Assert.IsTrue(sequencer.AddTask(funcC).IsCompleted);

            // Since there were no async forks
            // should not have to await
            // (In ValidateBase this means don't have to await AllRulesDone)
            Assert.IsTrue(completedA);
            Assert.IsTrue(completedB);
            Assert.IsTrue(completedC);
        }


        [TestMethod]
        public async Task AsyncTaskSequencer_AsyncForks_KeepTask()
        {
            var sequencer = new AsyncTaskSequencer();

            bool completedA = false;
            bool completedB = false;
            bool completedC = false;

            Func<Task> funcA = async () =>
            {
                await Task.Delay(5);
                Assert.IsFalse(completedA);
                Assert.IsFalse(completedB);
                Assert.IsFalse(completedC);
                completedA = true;
            };

            Func<Task> funcB = async () =>
            {
                await Task.Delay(10);
                Assert.IsTrue(completedA);
                Assert.IsFalse(completedB);
                Assert.IsFalse(completedC);
                completedB = true;
            };

            Func<Task> funcC = async () =>
            {
                await Task.Delay(15);
                Assert.IsTrue(completedA);
                Assert.IsTrue(completedB);
                Assert.IsFalse(completedC);
                completedC = true;
            };

            sequencer.AddTask(funcA);
            sequencer.AddTask(funcB).ContinueWith(t => {
                Assert.IsTrue(completedA);
                Assert.IsTrue(completedB);
                Assert.IsFalse(completedC);
            });
            sequencer.AddTask(funcC);

            await sequencer.AllDone;

            Assert.IsTrue(completedA);
            Assert.IsTrue(completedB);
            Assert.IsTrue(completedC);
        }
    }
}