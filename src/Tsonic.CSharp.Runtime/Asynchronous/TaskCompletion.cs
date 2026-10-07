using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tsonic.CSharp.Runtime;

public delegate void TaskResolve(Task? value = null);
public delegate void TaskResolve<T>(Union<T, Task<T>> value);
public delegate void TaskReject(TsValue reason = default);
public delegate void TaskExecutor(TaskResolve resolve, TaskReject reject);
public delegate void TaskExecutor<T>(TaskResolve<T> resolve, TaskReject reject);

public static class TaskCompletion
{
    public static Task Create(TaskExecutor executor)
    {
        ArgumentNullException.ThrowIfNull(executor);
        var completion = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var claimed = 0;

        void Resolve(Task? value = null)
        {
            if (Interlocked.CompareExchange(ref claimed, 1, 0) != 0) return;
            if (value is null)
            {
                completion.TrySetResult();
            }
            else if (ReferenceEquals(value, completion.Task))
            {
                completion.TrySetException(new InvalidOperationException("A Task cannot adopt its own completion."));
            }
            else
            {
                _ = Adopt(value);
            }
        }

        async Task Adopt(Task value)
        {
            try
            {
                await value.ConfigureAwait(false);
                completion.TrySetResult();
            }
            catch (OperationCanceledException exception) when (value.IsCanceled)
            {
                completion.TrySetCanceled(exception.CancellationToken);
            }
            catch (Exception exception)
            {
                completion.TrySetException(exception);
            }
        }

        void Reject(TsValue reason = default)
        {
            if (Interlocked.CompareExchange(ref claimed, 1, 0) == 0)
                completion.TrySetException(TsThrownValueException.from(reason));
        }

        try
        {
            executor(Resolve, Reject);
        }
        catch (Exception exception)
        {
            if (Interlocked.CompareExchange(ref claimed, 1, 0) == 0)
                completion.TrySetException(exception);
        }
        return completion.Task;
    }
}

public static class TaskCompletion<T>
{
    public static Task<T> Create(TaskExecutor<T> executor)
    {
        ArgumentNullException.ThrowIfNull(executor);
        var completion = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);
        var claimed = 0;

        void Resolve(Union<T, Task<T>> value)
        {
            if (Interlocked.CompareExchange(ref claimed, 1, 0) != 0) return;
            if (value.Is1())
            {
                completion.TrySetResult(value.As1());
            }
            else if (!value.Is2())
            {
                completion.TrySetException(new InvalidOperationException("Task resolution requires a selected value or Task arm."));
            }
            else
            {
                var adopted = value.As2();
                if (adopted is null)
                {
                    completion.TrySetException(new ArgumentNullException(nameof(value), "An adopted Task cannot be null."));
                }
                else if (ReferenceEquals(adopted, completion.Task))
                {
                    completion.TrySetException(new InvalidOperationException("A Task cannot adopt its own completion."));
                }
                else
                {
                    _ = Adopt(adopted);
                }
            }
        }

        async Task Adopt(Task<T> value)
        {
            try
            {
                completion.TrySetResult(await value.ConfigureAwait(false));
            }
            catch (OperationCanceledException exception) when (value.IsCanceled)
            {
                completion.TrySetCanceled(exception.CancellationToken);
            }
            catch (Exception exception)
            {
                completion.TrySetException(exception);
            }
        }

        void Reject(TsValue reason = default)
        {
            if (Interlocked.CompareExchange(ref claimed, 1, 0) == 0)
                completion.TrySetException(TsThrownValueException.from(reason));
        }

        try
        {
            executor(Resolve, Reject);
        }
        catch (Exception exception)
        {
            if (Interlocked.CompareExchange(ref claimed, 1, 0) == 0)
                completion.TrySetException(exception);
        }
        return completion.Task;
    }
}
