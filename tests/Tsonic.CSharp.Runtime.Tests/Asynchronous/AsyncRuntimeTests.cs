using System;
using System.Threading.Tasks;
using Xunit;

namespace Tsonic.CSharp.Runtime.Tests.Asynchronous;

public sealed class AsyncRuntimeTests
{
    [Fact]
    public async Task RunTasks_DrainsTheOriginalNativeTask()
    {
        var child = new TaskCompletionSource<int>();
        var calls = 0;
        var aggregate = AsyncRuntime.RunTasks(scope =>
        {
            calls += 1;
            scope.Spawn(child.Task);
            return Task.CompletedTask;
        });
        Assert.Equal(1, calls);
        Assert.False(aggregate.IsCompleted);
        child.SetResult(7);
        await aggregate;
        Assert.True(aggregate.IsCompletedSuccessfully);
    }

    [Fact]
    public async Task RunTasks_SelectsFirstCompletedFailureNotRegistrationOrder()
    {
        var first = new TaskCompletionSource();
        var second = new TaskCompletionSource();
        var expected = new InvalidOperationException("second registered, first completed");
        var aggregate = AsyncRuntime.RunTasks(scope =>
        {
            scope.Spawn(first.Task);
            scope.Spawn(second.Task);
            return Task.CompletedTask;
        });
        second.SetException(expected);
        Assert.False(aggregate.IsCompleted);
        first.SetException(new InvalidOperationException("later completion"));
        Assert.Same(expected, await Assert.ThrowsAsync<InvalidOperationException>(async () => await aggregate));
    }

    [Fact]
    public async Task RunTasks_MainFailureWinsOnlyAfterAllChildrenDrain()
    {
        var child = new TaskCompletionSource();
        var expected = new ArgumentException("main failure");
        var aggregate = AsyncRuntime.RunTasks(scope =>
        {
            scope.Spawn(child.Task);
            return Task.FromException(expected);
        });
        Assert.False(aggregate.IsCompleted);
        child.SetException(new InvalidOperationException("child failure"));
        Assert.Same(expected, await Assert.ThrowsAsync<ArgumentException>(async () => await aggregate));
    }

    [Fact]
    public async Task RunTasks_SynchronousMainFailureStillDrainsChildren()
    {
        var child = new TaskCompletionSource();
        var expected = new ArgumentException("synchronous main failure");
        var aggregate = AsyncRuntime.RunTasks(scope =>
        {
            scope.Spawn(child.Task);
            throw expected;
        });
        Assert.False(aggregate.IsCompleted);
        child.SetResult();
        Assert.Same(expected, await Assert.ThrowsAsync<ArgumentException>(async () => await aggregate));
    }

    [Fact]
    public async Task RunTasks_DrainsChildrenSpawnedByAnActiveChild()
    {
        var begin = new TaskCompletionSource();
        var finish = new TaskCompletionSource();
        var started = false;
        var aggregate = AsyncRuntime.RunTasks(scope =>
        {
            async Task Child()
            {
                started = true;
                await begin.Task;
                scope.Spawn(finish.Task);
            }
            scope.Spawn(Child());
            return Task.CompletedTask;
        });
        Assert.True(started);
        Assert.False(aggregate.IsCompleted);
        begin.SetResult();
        Assert.False(aggregate.IsCompleted);
        finish.SetResult();
        await aggregate;
        Assert.True(aggregate.IsCompletedSuccessfully);
    }

    [Fact]
    public async Task RunTasks_PreservesOriginalSourceExceptionPayloadIdentity()
    {
        var original = new object();
        var expected = TsThrownValueException.from(TsValue.from(original));
        var aggregate = AsyncRuntime.RunTasks(scope =>
        {
            scope.Spawn(Task.FromException(expected));
            return Task.CompletedTask;
        });
        var actual = await Assert.ThrowsAsync<TsThrownValueException>(async () => await aggregate);
        Assert.Same(expected, actual);
        Assert.Same(original, actual.value);
    }

    [Fact]
    public async Task RunTasks_PreservesNativeResultsWithoutANumericRoundTrip()
    {
        const ulong original = 9007199254740993UL;
        var result = await AsyncRuntime.RunTasks(scope =>
        {
            scope.Spawn(Task.CompletedTask);
            return Task.FromResult(original);
        });
        Assert.Equal(original, result);
        var identity = new object();
        Assert.Same(identity, await AsyncRuntime.RunTasks(_ => Task.FromResult(identity)));
    }

    [Fact]
    public async Task RunTasks_ACompletedScopeCannotAdmitAnUntrackedChild()
    {
        TaskScope? captured = null;
        await AsyncRuntime.RunTasks(scope =>
        {
            captured = scope;
            return Task.CompletedTask;
        });
        Assert.NotNull(captured);
        Assert.Throws<InvalidOperationException>(() => captured!.Spawn(Task.CompletedTask));
    }
}
