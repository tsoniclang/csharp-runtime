using System;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace Tsonic.CSharp.Runtime;

public sealed class TaskScope
{
    private int _pending;
    private bool _mainCompleted;
    private bool _completed;
    private ExceptionDispatchInfo? _firstError;
    private TaskCompletionSource? _drained;

    public void Spawn(Task task)
    {
        ArgumentNullException.ThrowIfNull(task);
        lock (this)
        {
            if (_completed)
            {
                throw new InvalidOperationException("A completed native task scope cannot receive a new child.");
            }
            checked { _pending += 1; }
        }
        if (task.IsCompleted)
        {
            Observe(task);
        }
        else
        {
            _ = task.ContinueWith(static (completed, state) => ((TaskScope)state!).Observe(completed),
                this, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
        }
    }

    internal Task CompleteMain()
    {
        lock (this)
        {
            if (_mainCompleted)
            {
                throw new InvalidOperationException("The native task scope main completion was already published.");
            }
            _mainCompleted = true;
            if (_pending == 0)
            {
                _completed = true;
                return Task.CompletedTask;
            }
            _drained = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            return _drained.Task;
        }
    }

    internal void ThrowFirstError()
    {
        ExceptionDispatchInfo? failure;
        lock (this)
        {
            if (!_completed)
            {
                throw new InvalidOperationException("A native task scope must drain before selecting its result.");
            }
            failure = _firstError;
        }
        failure?.Throw();
    }

    private void Observe(Task task)
    {
        ExceptionDispatchInfo? failure = null;
        if (!task.IsCompletedSuccessfully)
        {
            try
            {
                task.GetAwaiter().GetResult();
            }
            catch (Exception error)
            {
                failure = ExceptionDispatchInfo.Capture(error);
            }
        }
        TaskCompletionSource? drained = null;
        lock (this)
        {
            _firstError ??= failure;
            _pending -= 1;
            if (_mainCompleted && _pending == 0)
            {
                _completed = true;
                drained = _drained;
            }
        }
        drained?.SetResult();
    }
}

public static class AsyncRuntime
{
    public static async Task RunTasks(Func<TaskScope, Task> main)
    {
        ArgumentNullException.ThrowIfNull(main);
        var scope = new TaskScope();
        ExceptionDispatchInfo? mainError = null;
        try
        {
            await main(scope).ConfigureAwait(false);
        }
        catch (Exception error)
        {
            mainError = ExceptionDispatchInfo.Capture(error);
        }
        await scope.CompleteMain().ConfigureAwait(false);
        mainError?.Throw();
        scope.ThrowFirstError();
    }

    public static async Task<TResult> RunTasks<TResult>(Func<TaskScope, Task<TResult>> main)
    {
        ArgumentNullException.ThrowIfNull(main);
        var scope = new TaskScope();
        ExceptionDispatchInfo? mainError = null;
        TResult result = default!;
        try
        {
            result = await main(scope).ConfigureAwait(false);
        }
        catch (Exception error)
        {
            mainError = ExceptionDispatchInfo.Capture(error);
        }
        await scope.CompleteMain().ConfigureAwait(false);
        mainError?.Throw();
        scope.ThrowFirstError();
        return result;
    }
}
