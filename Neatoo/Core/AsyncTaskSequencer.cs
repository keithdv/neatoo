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
    public class AsyncTaskSequencer
    {

        private readonly object lockObject = new object();
        private IContinueWith firstTask;
        private IContinueWith currentTask;
        private TaskCompletionSource<bool> allDoneCompletionSource;
        private CancellationTokenSource cancellationTokenSource;
        public bool IsRunning => !this.allDoneCompletionSource?.Task.IsCompleted ?? false;

        public ConcurrentBag<Action> OnFullSequenceComplete { get; } = new ConcurrentBag<Action>();

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

                    // First task is a special case
                    // If there is no async fork don't create one
                    var eTask = task(Task.CompletedTask);

                    if (eTask.IsCompleted)
                    {
                        foreach (var action in OnFullSequenceComplete.ToList())
                        {
                            action();
                        }

                        if (eTask.IsFaulted)
                        {
                            throw eTask.Exception;
                        }

                        return eTask;
                    }

                    allDoneCompletionSource = new TaskCompletionSource<bool>();

                    Func<Task, Task> continueWith = (t) =>
                    {
                        // TODO: There's an issue here
                        // if a sequence is started while waiting for this to complete
                        var allDoneCompletionSource = this.allDoneCompletionSource;

                        // Needs to be here so that it's known that the rules are done
                        foreach (var action in OnFullSequenceComplete.ToList())
                        {
                            action();
                        }


                        if (t.IsFaulted)
                        {
                            allDoneCompletionSource.SetException(t.Exception);
                        }
                        else
                        {
                            allDoneCompletionSource.SetResult(true);
                        }


                        return allDoneCompletionSource.Task;
                    };

                    firstTask = new AsyncTaskSequencerTask(eTask, continueWith);

                    currentTask = firstTask;

                    return firstTask.Task;
                }
                else
                {
                    // We change the ContinueWith stored in a delegate variable of the current task
                    // before it finishes
                    // so that it now does calls the next task instead of the "AllDone"

                    var nextTask = new AsyncTaskSequencerFunction(task, currentTask.ContinueWith, runOnException);

                    currentTask.IsLast = false;

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

    }

    public interface IContinueWith
    {
        public Func<Task, Task> ContinueWith { get; set; }
        public Task Task { get; }
        public bool IsLast { get; set; }
    }

    public class AsyncTaskSequencerTask : IContinueWith
    {
        private readonly bool runOnException;

        // Be clear which Func<Task, Task> is being used
        public AsyncTaskSequencerTask(Task task, Func<Task, Task> initialContinueWith)
        {
            ContinueWith = initialContinueWith;
            Task = task.ContinueWith((t) =>
            {
                return ContinueWith(t);
            });
        }

        public Task Task { get; }

        public CancellationTokenSource CancellationTokenSource { get; set; }
        public Func<Task, Task> ContinueWith { get; set; }
        public bool IsLast { get; set; }
    }

    public class AsyncTaskSequencerFunction : IContinueWith
    {
        // Be clear which Func<Task, Task> is being used
        public AsyncTaskSequencerFunction(Func<Task, Task> task, Func<Task, Task> initialContinueWith, bool runOnException)
        {
            this.task = task;
            ContinueWith = initialContinueWith;
            this.runOnException = runOnException;
        }

        private bool callOnce = false;
        private readonly Func<Task, Task> task;
        private readonly bool runOnException;
        private readonly TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
        public Task Task => taskCompletionSource.Task;
        public Task Execute(Task t)
        {
            Debug.Assert(!callOnce, "Task has already been executed");
            callOnce = true;
            if (!t.IsFaulted || runOnException || IsLast)
            {
                return task(t).ContinueWith((t2) =>
                {
                    if (t.IsFaulted) { t2 = t; } // Pass thru the failure
                    return ContinueWith(t2);
                });
            } 
            return ContinueWith(t);
        }

        public CancellationTokenSource CancellationTokenSource { get; set; }

        /// <summary>
        /// This is the key piece
        /// Switching this causes the task to do something different when it completes
        /// Then original. Like call ("await") a different task.
        /// </summary>
        public Func<Task, Task> ContinueWith { get; set; }
        public bool IsLast { get; set; } = true;
    }
}
