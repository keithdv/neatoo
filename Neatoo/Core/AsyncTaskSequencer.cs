using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;

namespace Neatoo.Core
{
    /// <summary>
    /// Manages the sequence of asynchronous tasks, ensuring they are executed in order.
    /// The key is the last task that complete 'AllDone' always stays the last task
    /// even as more tasks are added to the sequence.
    /// This way anything awaiting the 'AllDone' task will not be completed until all tasks
    /// are done INCLUDING added tasks.
    /// This is key for cascading rules.
    /// </summary>
    public sealed class AsyncTaskSequencer
    {
        // TODO: Add cancellation token

        private readonly object lockObject = new object();
        private SequencedAsyncFunction? lastTask;
        private TaskCompletionSource<bool>? allDoneCompletionSource;

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
        /// <param name="task">The task to be added. Needs to be a Func because it may not actually be time to execute</param>
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

                    if (result.Exception != null)
                    {
                        throw result.Exception;
                    }

                    // Seems like I should do this but it causes too many calls to OnFullSequenceComplete
                    //if (result.IsCompleted)
                    //{
                    //    var t = OnFullSequenceComplete();
                    //    Debug.Assert(t.IsCompleted, "Not sure what to do about this yet");
                    //}

                    if (result.IsCompleted)
                    {
                        return result;
                    }

                    allDoneCompletionSource = new TaskCompletionSource<bool>();

                    lastTask = new SequencedAsyncFunction(result => result, SequenceCompleted, lockObject, true);
                    lastTask.Execute(result, new List<Exception>());

                    return lastTask.Task; // Await AllDone if you want to wait for the sequence to complete
                }
                else
                {
                    // Change the ContinueWith stored in a delegate variable of the current task
                    // before it finishes so that it now calls the next task instead of the "AllDone".
                    var newLastTask = new SequencedAsyncFunction(task, SequenceCompleted, lockObject, runOnException);

                    lastTask!.nextTask = newLastTask;
                    lastTask = newLastTask;

                    return newLastTask.Task;
                }
            }
        }

        /// <summary>
        /// Gets a task that completes when all tasks in the sequence are done.
        /// </summary>
        public Task AllDone => allDoneCompletionSource?.Task ?? Task.CompletedTask;

        private async Task SequenceCompleted(Task previousTask, List<Exception> exceptions)
        {
            var completionSource = this.allDoneCompletionSource ?? throw new ArgumentNullException($"{nameof(allDoneCompletionSource)} should not be null");

            try
            {
                await OnFullSequenceComplete();
            }
            catch (AggregateException ex)
            {
                exceptions.AddRange(ex.InnerExceptions);
            }

            if (exceptions.Count > 0)
            {
                completionSource.SetException(new AggregateException(exceptions));
            }
            else
            {
                completionSource.SetResult(true);
            }

            await completionSource.Task;
        }

        /// <summary>
        /// Represents a single task in the sequence.
        /// </summary>
        public sealed class SequencedAsyncFunction
        {
            private bool callOnce = false;
            private readonly Func<Task, Task> taskFunc;
            private readonly object lockObject;
            private readonly bool runOnException;

            /// <summary>
            /// Initializes a new instance of the <see cref="SequencedAsyncFunction"/> class.
            /// </summary>
            /// <param name="task">The task to be executed.</param>
            /// <param name="sequenceCompleted">The initial continuation function.</param>
            /// <param name="lockObject">The lock object for synchronization.</param>
            /// <param name="runOnException">Indicates whether the task should run on exception.</param>
            public SequencedAsyncFunction(Func<Task, Task> task, Func<Task, List<Exception>, Task> sequenceCompleted, object lockObject, bool runOnException)
            {
                this.taskFunc = task;
                Completed = sequenceCompleted;
                this.lockObject = lockObject;
                this.runOnException = runOnException;
            }


            /// <summary>
            /// Executes the task.
            /// </summary>
            /// <param name="previousTask">The previous task in the sequence.</param>
            /// <returns>A task representing the execution of the task.</returns>
            public Task Execute(Task previousTask, List<Exception> exceptions)
            {
                lock (lockObject)
                {
                    Debug.Assert(!callOnce, "Task has already been executed");
                    callOnce = true;

                    Task continueSequence(Task completedTask)
                    {
                        if (completedTask.Exception is AggregateException aggregateException)
                        {
                            exceptions.AddRange(aggregateException.InnerExceptions);
                            TaskCompletionSource.SetException(completedTask.Exception);
                        }
                        else
                        {
                            TaskCompletionSource.SetResult(null);
                        }

                        return nextTask?.Execute(completedTask, exceptions) ?? Completed!(completedTask, exceptions);
                    };

                    if (exceptions.Count == 0 || runOnException)
                    {
                        var currentTask = taskFunc(previousTask); // Execute function - been delayed until it's turn in the sequence

                        if (currentTask.IsCompleted)
                        {
                            return continueSequence(currentTask);
                        }
                        else
                        {
                            return currentTask.ContinueWith((completedTask) =>
                            {
                                lock (lockObject)
                                {
                                    return continueSequence(completedTask);
                                }
                            });
                        }
                    }

                    return nextTask?.Execute(previousTask, exceptions) ?? Completed!(previousTask, exceptions);
                }
            }

            private TaskCompletionSource<object?> TaskCompletionSource = new TaskCompletionSource<object?>();
            public Task Task => TaskCompletionSource.Task;

            /// <summary>
            /// This is the key piece.
            /// Switching this causes the task to do something different when it completes.
            /// Then original. Like call ("await") a different task.
            /// </summary>
            public Func<Task, List<Exception>, Task>? Completed { get; set; }
            public SequencedAsyncFunction? nextTask { get; set; }
        }
    }
}