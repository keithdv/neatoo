using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Neatoo.Core
{
    /// <summary>
    /// Manages the sequence of asynchronous tasks, ensuring they are executed in order.
    /// </summary>
    public sealed class AsyncTaskSequencer
    {
        protected readonly object lockObject = new object();
        private AsyncTaskSequencerFunction firstTask;
        private AsyncTaskSequencerFunction currentTask;
        private TaskCompletionSource<bool> allDoneCompletionSource;
        private CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// Indicates whether the sequencer is currently running tasks.
        /// </summary>
        public bool IsRunning => !this.allDoneCompletionSource?.Task.IsCompleted ?? false;

        /// <summary>
        /// Function to be called when the full sequence is complete.
        /// </summary>
        public Func<Task> OnFullSequenceComplete { get; set; } = () => Task.CompletedTask;

        /// <summary>
        /// Adds a new task to the sequence.
        /// </summary>
        /// <param name="task">The task to be added.</param>
        /// <param name="runOnException">Indicates whether the task should run on exception.</param>
        /// <returns>A task representing the added task.</returns>
        public Task AddTask(Func<Task, Task> task, bool runOnException = false)
        {
            lock (lockObject)
            {
                // If the sequencer is faulted, return the faulted task.
                if (allDoneCompletionSource != null && allDoneCompletionSource.Task.IsFaulted)
                {
                    return allDoneCompletionSource.Task;
                }

                // If the sequencer is not running or has completed, start a new sequence.
                if (allDoneCompletionSource == null || allDoneCompletionSource.Task.IsCompleted)
                {
                    Task result = task(Task.CompletedTask);

                    if (result.IsFaulted)
                    {
                        throw result.Exception;
                    }

                    //if (result.IsCompleted)
                    //{
                    //    var t = OnFullSequenceComplete();
                    //    Debug.Assert(t.IsCompleted, "Not sure what to do about this yet");
                    //}

                    if (result.IsFaulted)
                    {
                        throw result.Exception;
                    }

                    if (result.IsCompleted)
                    {
                        return result;
                    }

                    allDoneCompletionSource = new TaskCompletionSource<bool>();

                    Func<Task, Task> continueWith = async (t) =>
                    {
                        var allDoneCompletionSource = this.allDoneCompletionSource;

                        try
                        {
                            await OnFullSequenceComplete();
                        }
                        catch (Exception ex)
                        {
                            allDoneCompletionSource.SetException(t.Exception);
                        }


                        if (t.IsFaulted)
                        {
                            allDoneCompletionSource.SetException(t.Exception);
                        }
                        else
                        {
                            allDoneCompletionSource.SetResult(true);
                        }

                        await allDoneCompletionSource.Task;
                    };

                    firstTask = new AsyncTaskSequencerFunction(result => result, continueWith, lockObject, true);
                    currentTask = firstTask;
                    firstTask.Execute(result);

                    return firstTask.Task; // Await AllDone if you want to wait for the sequence to complete
                }
                else
                {
                    // Change the ContinueWith stored in a delegate variable of the current task
                    // before it finishes so that it now calls the next task instead of the "AllDone".
                    var nextTask = new AsyncTaskSequencerFunction(task, currentTask.ContinueWith, lockObject, runOnException);

                    currentTask.ContinueWith = (t) =>
                    {
                        return nextTask.Execute(t);
                    };

                    currentTask = nextTask;

                    return currentTask.Task;
                }
            }
        }

        /// <summary>
        /// Gets a task that completes when all tasks in the sequence are done.
        /// </summary>
        public Task AllDone => allDoneCompletionSource?.Task ?? Task.CompletedTask;

        /// <summary>
        /// Represents a single task in the sequence.
        /// </summary>
        public sealed class AsyncTaskSequencerFunction
        {
            private bool callOnce = false;
            private readonly Func<Task, Task> task;
            private readonly object lockObject;
            private readonly bool runOnException;

            /// <summary>
            /// Initializes a new instance of the <see cref="AsyncTaskSequencerFunction"/> class.
            /// </summary>
            /// <param name="task">The task to be executed.</param>
            /// <param name="initialContinueWith">The initial continuation function.</param>
            /// <param name="lockObject">The lock object for synchronization.</param>
            /// <param name="runOnException">Indicates whether the task should run on exception.</param>
            public AsyncTaskSequencerFunction(Func<Task, Task> task, Func<Task, Task> initialContinueWith, object lockObject, bool runOnException)
            {
                this.task = task;
                ContinueWith = initialContinueWith;
                this.lockObject = lockObject;
                this.runOnException = runOnException;
            }

            /// <summary>
            /// Executes the task.
            /// </summary>
            /// <param name="t">The previous task in the sequence.</param>
            /// <returns>A task representing the execution of the task.</returns>
            public Task Execute(Task t)
            {
                lock (lockObject)
                {
                    Debug.Assert(!callOnce, "Task has already been executed");
                    callOnce = true;

                    if (!t.IsFaulted || runOnException)
                    {
                        Task finishTask(Task t)
                        {
                            if (t.IsFaulted)
                            {
                                TaskCompletionSource.SetException(t.Exception);
                            }
                            else
                            {
                                TaskCompletionSource.SetResult(null);
                            }

                            return ContinueWith(t);
                        };

                        try
                        {
                            var result = task(t);

                            if (result.IsCompleted)
                            {
                                return finishTask(result);
                            }
                            else
                            {
                                return result.ContinueWith((t) =>
                                {
                                    try
                                    {
                                        finishTask(t);
                                    } catch(Exception ex)
                                    {
                                        TaskCompletionSource.SetException(ex);
                                    }
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            TaskCompletionSource.SetException(ex);
                        }


                    }

                    return ContinueWith(t);
                }
            }

            public CancellationTokenSource CancellationTokenSource { get; set; }

            private TaskCompletionSource<Exception> TaskCompletionSource = new TaskCompletionSource<Exception>();
            public Task Task => TaskCompletionSource.Task;

            /// <summary>
            /// This is the key piece.
            /// Switching this causes the task to do something different when it completes.
            /// Then original. Like call ("await") a different task.
            /// </summary>
            public Func<Task, Task> ContinueWith { get; set; }
        }
    }
}