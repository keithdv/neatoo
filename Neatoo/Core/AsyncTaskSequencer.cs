using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Neatoo.Core
{
    public sealed class AsyncTaskSequencer
    {

        protected readonly object lockObject = new object();
        private AsyncTaskSequencerFunction firstTask;
        private AsyncTaskSequencerFunction currentTask;
        private TaskCompletionSource<bool> allDoneCompletionSource;
        private CancellationTokenSource cancellationTokenSource;
        public bool IsRunning => !this.allDoneCompletionSource?.Task.IsCompleted ?? false;

        public Func<Task, Task> OnFullSequenceComplete { get; set; } = (T) => Task.CompletedTask;

        public Task AddTask(Func<Task, Task> task, bool runOnException = false)
        {
            lock (lockObject)
            {
                // For now, exceptions mean the Business Object is more or less dead
                if (allDoneCompletionSource != null && allDoneCompletionSource.Task.IsFaulted)
                {
                    return allDoneCompletionSource.Task;
                }

                if (allDoneCompletionSource == null || allDoneCompletionSource.Task.IsCompleted)
                {
                    // TODO : I think I need another lock to ensure that the task 
                    // is not completed and then the completed with is set
                    // I think AsyncTaskSequencer and all AsyncTaskSequencyTasks should share a lock

                    Task result = task(Task.CompletedTask);

                    if (result.IsFaulted)
                    {
                        throw result.Exception;
                    }

                    if (result.IsCompleted)
                    {
                        result = OnFullSequenceComplete(result);
                    }

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
                        // TODO: There's an issue here
                        // if a sequence is started while waiting for this to complete
                        var allDoneCompletionSource = this.allDoneCompletionSource;

                        try
                        {
                            await OnFullSequenceComplete(t);
                        }
                        catch (Exception ex)
                        {
                            allDoneCompletionSource.SetException(t.Exception);
                        }

                        await OnFullSequenceComplete(t);

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
                    // We change the ContinueWith stored in a delegate variable of the current task
                    // before it finishes
                    // so that it now does calls the next task instead of the "AllDone"

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

        public Task AllDone => allDoneCompletionSource?.Task ?? Task.CompletedTask;




        public sealed class AsyncTaskSequencerFunction
        {
            // Be clear which Func<Task, Task> is being used

            public AsyncTaskSequencerFunction(Func<Task, Task> task, Func<Task, Task> initialContinueWith, object lockObject, bool runOnException)
            {
                this.task = task;
                ContinueWith = initialContinueWith;
                this.lockObject = lockObject;
                this.runOnException = runOnException;
            }

            private bool callOnce = false;
            private readonly Func<Task, Task> task;
            private readonly object lockObject;
            private readonly bool runOnException;

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

                        var result = task(t);

                        if (result.IsCompleted)
                        {
                            return finishTask(result);
                        }
                        else
                        {
                            return result.ContinueWith((t) => finishTask(t));

                        }
                    }

                    return ContinueWith(t);
                }
            }

            public CancellationTokenSource CancellationTokenSource { get; set; }

            private TaskCompletionSource<Exception> TaskCompletionSource = new TaskCompletionSource<Exception>();
            public Task Task => TaskCompletionSource.Task;

            /// <summary>
            /// This is the key piece
            /// Switching this causes the task to do something different when it completes
            /// Then original. Like call ("await") a different task.
            /// </summary>
            public Func<Task, Task> ContinueWith { get; set; }
        }
    }
}
