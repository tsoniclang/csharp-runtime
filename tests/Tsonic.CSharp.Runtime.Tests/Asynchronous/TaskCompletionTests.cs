using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tsonic.CSharp.Runtime.Tests.Asynchronous;

public sealed class TaskCompletionTests
{
    [Fact]
    public void Create_RejectsNullExecutor()
    {
        Assert.Throws<ArgumentNullException>(() => { _ = TaskCompletion.Create(null!); });
        Assert.Throws<ArgumentNullException>(() => { _ = TaskCompletion<int>.Create(null!); });
    }

    [Fact]
    public async Task Create_ResolvesVoidPromise()
    {
        var calls = 0;
        var task = TaskCompletion.Create((resolve, _) => { calls++; resolve(); });
        Assert.Equal(1, calls);
        await task;
        Assert.True(task.IsCompletedSuccessfully);
    }

    [Fact]
    public async Task Create_ResolvesTypedPromise()
    {
        var calls = 0;
        var task = TaskCompletion<int>.Create((resolve, _) => { calls++; resolve(42); });
        Assert.Equal(1, calls);
        Assert.Equal(42, await task);
    }

    [Fact]
    public async Task Create_PreservesNativeIntegerWidthAndValueIdentity()
    {
        const ulong wide = 9007199254740993UL;
        var value = new object();
        Assert.Equal(wide, await TaskCompletion<ulong>.Create((resolve, _) => resolve(wide)));
        Assert.Same(value, await TaskCompletion<object>.Create((resolve, _) => resolve(value)));
        Assert.Same(value, await TaskCompletion<object>.Create((resolve, _) => resolve(Task.FromResult(value))));
        Assert.Null(await TaskCompletion<object?>.Create((resolve, _) =>
            resolve(Union<object?, Task<object?>>.From1(null))));
    }

    [Fact]
    public async Task Create_AssimilatesTypedTask()
    {
        var nested = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);
        var task = TaskCompletion<int>.Create((resolve, _) => resolve(nested.Task));
        Assert.False(task.IsCompleted);
        nested.SetResult(42);
        Assert.Equal(42, await task);
    }

    [Fact]
    public async Task Create_AssimilatesVoidTask()
    {
        var nested = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var task = TaskCompletion.Create((resolve, _) => resolve(nested.Task));
        Assert.False(task.IsCompleted);
        nested.SetResult();
        await task;
        Assert.True(task.IsCompletedSuccessfully);
    }

    [Fact]
    public async Task Create_UsesFirstSettlement()
    {
        var task = TaskCompletion<int>.Create((resolve, reject) =>
        {
            resolve(7);
            reject(TsValue.from(new InvalidOperationException("late rejection")));
            resolve(9);
        });
        Assert.Equal(7, await task);
    }

    [Fact]
    public async Task Create_PendingTypedAdoptionReservesFirstSettlement()
    {
        var pending = new TaskCompletionSource<ulong>(TaskCreationOptions.RunContinuationsAsynchronously);
        var task = TaskCompletion<ulong>.Create((resolve, reject) =>
        {
            resolve(pending.Task);
            reject(TsValue.from(new InvalidOperationException("late rejection")));
            resolve(0UL);
            throw new InvalidOperationException("late throw");
        });
        Assert.False(task.IsCompleted);
        pending.SetResult(9007199254740993UL);
        Assert.Equal(9007199254740993UL, await task);
    }

    [Fact]
    public async Task Create_PendingVoidAdoptionReservesFirstSettlement()
    {
        var pending = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var task = TaskCompletion.Create((resolve, reject) =>
        {
            resolve(pending.Task);
            reject(TsValue.from(new InvalidOperationException("late rejection")));
            resolve();
            throw new InvalidOperationException("late throw");
        });
        Assert.False(task.IsCompleted);
        var expected = new InvalidOperationException("adopted rejection");
        pending.SetException(expected);
        Assert.Same(expected, await Assert.ThrowsAsync<InvalidOperationException>(async () => await task));
    }

    [Fact]
    public async Task Create_RejectsWithExceptionReason()
    {
        var expected = new InvalidOperationException("rejected");
        var task = TaskCompletion<int>.Create((_, reject) => reject(TsValue.from(expected)));
        Assert.Same(expected, await Assert.ThrowsAsync<InvalidOperationException>(async () => await task));
        var untyped = TaskCompletion.Create((_, reject) => reject(TsValue.from(expected)));
        Assert.Same(expected, await Assert.ThrowsAsync<InvalidOperationException>(async () => await untyped));
    }

    [Fact]
    public async Task Create_UsesCanonicalClosedValueRejection()
    {
        var task = TaskCompletion<int>.Create((_, reject) => reject(TsValue.from("reason")));
        var actual = await Assert.ThrowsAsync<TsThrownValueException>(async () => await task);
        Assert.Equal("reason", TsValue.UnwrapClosedValue(actual.value));
        var reason = new Reason();
        var closed = TaskCompletion.Create((_, reject) => reject(TsValue.from(reason)));
        var original = await Assert.ThrowsAsync<TsThrownValueException>(async () => await closed);
        Assert.Same(reason, TsValue.UnwrapClosedValue(original.value));
    }

    [Fact]
    public async Task Create_OmittedAndExplicitAbsentRejectionHaveOneState()
    {
        var implicitAbsence = TaskCompletion.Create((_, reject) => reject());
        var explicitAbsence = TaskCompletion<int>.Create((_, reject) => reject(TsValue.undefined()));
        var omitted = await Assert.ThrowsAsync<TsThrownValueException>(async () => await implicitAbsence);
        var supplied = await Assert.ThrowsAsync<TsThrownValueException>(async () => await explicitAbsence);
        Assert.True(omitted.value.isUndefined());
        Assert.True(supplied.value.isUndefined());
    }

    [Fact]
    public async Task Create_RejectsSynchronousExecutorThrow()
    {
        var expected = new InvalidOperationException("executor failed");
        var task = TaskCompletion<int>.Create((_, _) => throw expected);
        Assert.Same(expected, await Assert.ThrowsAsync<InvalidOperationException>(async () => await task));
        var untyped = TaskCompletion.Create((_, _) => throw expected);
        Assert.Same(expected, await Assert.ThrowsAsync<InvalidOperationException>(async () => await untyped));
    }

    [Fact]
    public async Task Create_IgnoresThrowAfterResolution()
    {
        var task = TaskCompletion<int>.Create((resolve, _) =>
        {
            resolve(11);
            throw new InvalidOperationException("late throw");
        });
        Assert.Equal(11, await task);
        var untyped = TaskCompletion.Create((resolve, _) =>
        {
            resolve();
            throw new InvalidOperationException("late throw");
        });
        await untyped;
        Assert.True(untyped.IsCompletedSuccessfully);
    }

    [Fact]
    public async Task Create_RetainedCallbacksOwnCompletionAfterExecutorReturns()
    {
        TaskResolve<int>? resolveLater = null;
        TaskReject? rejectLater = null;
        var resolved = TaskCompletion<int>.Create((resolve, _) => resolveLater = resolve);
        var rejected = TaskCompletion.Create((_, reject) => rejectLater = reject);
        Assert.False(resolved.IsCompleted);
        Assert.False(rejected.IsCompleted);
        resolveLater!(31);
        var expected = new InvalidOperationException("retained callback");
        rejectLater!(TsValue.from(expected));
        Assert.Equal(31, await resolved);
        Assert.Same(expected, await Assert.ThrowsAsync<InvalidOperationException>(async () => await rejected));
    }

    [Fact]
    public async Task Create_ConcurrentSettlementsPublishOnlyOneResult()
    {
        TaskResolve<int>? resolveLater = null;
        TaskReject? rejectLater = null;
        var task = TaskCompletion<int>.Create((resolve, reject) =>
        {
            resolveLater = resolve;
            rejectLater = reject;
        });
        var expected = new InvalidOperationException("competing rejection");
        using var start = new ManualResetEventSlim();
        var resolveWork = Task.Run(() => { start.Wait(); resolveLater!(17); });
        var rejectWork = Task.Run(() => { start.Wait(); rejectLater!(TsValue.from(expected)); });
        start.Set();
        await Task.WhenAll(resolveWork, rejectWork).WaitAsync(TimeSpan.FromSeconds(5));
        if (task.IsCompletedSuccessfully)
        {
            Assert.Equal(17, await task);
            rejectLater!(TsValue.from(expected));
            Assert.Equal(17, await task);
        }
        else
        {
            Assert.True(task.IsFaulted);
            Assert.Same(expected, await Assert.ThrowsAsync<InvalidOperationException>(async () => await task));
            resolveLater!(17);
            Assert.Same(expected, await Assert.ThrowsAsync<InvalidOperationException>(async () => await task));
        }
    }

    [Fact]
    public async Task Create_ConcurrentVoidSettlementsPublishOnlyOneCompletion()
    {
        TaskResolve? resolveLater = null;
        TaskReject? rejectLater = null;
        var task = TaskCompletion.Create((resolve, reject) =>
        {
            resolveLater = resolve;
            rejectLater = reject;
        });
        var expected = new InvalidOperationException("competing void rejection");
        using var start = new ManualResetEventSlim();
        var resolveWork = Task.Run(() => { start.Wait(); resolveLater!(); });
        var rejectWork = Task.Run(() => { start.Wait(); rejectLater!(TsValue.from(expected)); });
        start.Set();
        await Task.WhenAll(resolveWork, rejectWork).WaitAsync(TimeSpan.FromSeconds(5));
        if (task.IsCompletedSuccessfully)
        {
            await task;
            rejectLater!(TsValue.from(expected));
            Assert.True(task.IsCompletedSuccessfully);
        }
        else
        {
            Assert.True(task.IsFaulted);
            Assert.Same(expected, await Assert.ThrowsAsync<InvalidOperationException>(async () => await task));
            resolveLater!();
            Assert.Same(expected, await Assert.ThrowsAsync<InvalidOperationException>(async () => await task));
        }
    }

    [Fact]
    public async Task Create_AdoptsNativeCancellationWithItsToken()
    {
        using var cancellation = new CancellationTokenSource();
        var typed = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);
        var untyped = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var typedResult = TaskCompletion<int>.Create((resolve, _) => resolve(typed.Task));
        var untypedResult = TaskCompletion.Create((resolve, _) => resolve(untyped.Task));
        cancellation.Cancel();
        typed.SetCanceled(cancellation.Token);
        untyped.SetCanceled(cancellation.Token);
        var typedFailure = await Assert.ThrowsAnyAsync<OperationCanceledException>(async () => await typedResult);
        var untypedFailure = await Assert.ThrowsAnyAsync<OperationCanceledException>(async () => await untypedResult);
        Assert.Equal(cancellation.Token, typedFailure.CancellationToken);
        Assert.Equal(cancellation.Token, untypedFailure.CancellationToken);
        Assert.True(typedResult.IsCanceled);
        Assert.True(untypedResult.IsCanceled);
    }

    [Fact]
    public async Task Create_DoesNotReclassifyFaultedCancellationException()
    {
        var expected = new OperationCanceledException("fault, not cancellation");
        var typed = TaskCompletion<int>.Create((resolve, _) => resolve(Task.FromException<int>(expected)));
        var untyped = TaskCompletion.Create((resolve, _) => resolve(Task.FromException(expected)));
        Assert.Same(expected, await Assert.ThrowsAsync<OperationCanceledException>(async () => await typed));
        Assert.Same(expected, await Assert.ThrowsAsync<OperationCanceledException>(async () => await untyped));
        Assert.True(typed.IsFaulted);
        Assert.True(untyped.IsFaulted);
    }

    [Fact]
    public async Task Create_RejectsSelfAdoptionWithoutHanging()
    {
        TaskResolve<int>? typedResolve = null;
        TaskResolve? voidResolve = null;
        var typed = TaskCompletion<int>.Create((resolve, _) => typedResolve = resolve);
        var untyped = TaskCompletion.Create((resolve, _) => voidResolve = resolve);
        typedResolve!(typed);
        voidResolve!(untyped);
        Assert.True(typed.IsFaulted);
        Assert.True(untyped.IsFaulted);
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await typed);
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await untyped);
    }

    [Fact]
    public async Task Create_RejectsMalformedSelectedResolution()
    {
        var missing = TaskCompletion<int>.Create((resolve, _) => resolve(default));
        var nullTask = TaskCompletion<int>.Create((resolve, _) =>
            resolve(Union<int, Task<int>>.From2(null!)));
        Assert.True(missing.IsFaulted);
        Assert.True(nullTask.IsFaulted);
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await missing);
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await nullTask);
    }

    private sealed class Reason : ITsClosedValueCarrier {}
}
