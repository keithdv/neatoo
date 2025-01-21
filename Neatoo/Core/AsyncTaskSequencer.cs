using System;
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

        public Action OnCompleted { get; set; }

        public void AddTask(Func<Task> task)
        {
            lock (lockObject)
            {
                if (allDoneCompletionSource == null || allDoneCompletionSource.Task.IsCompleted)
                {
                    // TODO : I think I need another lock to ensure that the task 
                    // is not completed and then the completed with is set
                    // I think AsyncTaskSequencer and all AsyncTaskSequencyTasks should share a lock

                    // First task is a special case
                    // If there is no async fork don't create one
                    var eTask = task();

                    if (eTask.IsCompleted)
                    {
                        if (eTask.IsFaulted)
                        {
                            throw eTask.Exception;
                        }

                        OnCompleted?.Invoke();

                        return;
                    }

                    allDoneCompletionSource = new TaskCompletionSource<bool>();

                    Func<Task, Task> continueWith = async (t) =>
                    {
                        OnCompleted?.Invoke();

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

                    firstTask = new AsyncTaskSequencerTask(eTask, continueWith);

                    currentTask = firstTask;
                }
                else
                {
                    // We change the ContinueWith stored in a delegate variable of the current task
                    // before it finishes
                    // so that it now does calls the next task instead of the "AllDone"

                    var nextTask = new AsyncTaskSequencerFunction(task, currentTask.ContinueWith);

                    currentTask.ContinueWith = (t) =>
                    {
                        if (t.IsFaulted)
                        {
                            allDoneCompletionSource.SetException(t.Exception);
                            return allDoneCompletionSource.Task;
                        }
                        else
                        {
                            return nextTask.Execute();
                        }
                    };

                    currentTask = nextTask;
                }
            }
        }

        public Task AllDone => allDoneCompletionSource?.Task ?? Task.CompletedTask;

    }

    public interface IContinueWith
    {
        public Func<Task, Task> ContinueWith { get; set; }
    }

    public class AsyncTaskSequencerTask : IContinueWith
    {
        // Be clear which Func<Task, Task> is being used
        public AsyncTaskSequencerTask(Task task, Func<Task, Task> initialContinueWith)
        {
            ContinueWith = initialContinueWith;
            Task = task.ContinueWith((t) => ContinueWith(t));
        }

        public Task Task { get; }

        public CancellationTokenSource CancellationTokenSource { get; set; }
        public Func<Task, Task> ContinueWith { get; set; }
    }

    public class AsyncTaskSequencerFunction : IContinueWith
    {
        // Be clear which Func<Task, Task> is being used
        public AsyncTaskSequencerFunction(Func<Task> task, Func<Task, Task> initialContinueWith)
        {
            this.task = task;
            ContinueWith = initialContinueWith;
        }

        private bool callOnce = false;
        private readonly Func<Task> task;

        public Task Execute()
        {
            Debug.Assert(!callOnce, "Task has already been executed");
            callOnce = true;
            return task().ContinueWith((t) => ContinueWith(t));
        }

        public CancellationTokenSource CancellationTokenSource { get; set; }
        public Func<Task, Task> ContinueWith { get; set; }
    }
}
