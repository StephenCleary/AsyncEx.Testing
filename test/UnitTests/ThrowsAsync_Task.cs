using System;
using System.Threading.Tasks;
using Nito.AsyncEx.Testing;
using Xunit;

namespace UnitTests
{
    public partial class UnitTests
    {
        [Fact]
        public async Task ThrowsAsync_TaskDoesNotThrow_Fails()
        {
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                var tcs = new TaskCompletionSource<object>();
                var testTask = AsyncAssert.ThrowsAsync(tcs.Task);
                tcs.SetResult(null);
                await testTask;
            });
        }

        [Fact]
        public async Task ThrowsAsync_TaskThrows_ReturnsException()
        {
            var expectedException = new InvalidOperationException();
            var tcs = new TaskCompletionSource<object>();
            var testTask = AsyncAssert.ThrowsAsync(tcs.Task);
            tcs.SetException(expectedException);
            var result = await testTask;
            Assert.Same(expectedException, result);
        }

        [Fact]
        public async Task ThrowsAsync_TaskThrowsWrongException_Fails()
        {
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                var tcs = new TaskCompletionSource<object>();
                var testTask = AsyncAssert.ThrowsAsync<NotImplementedException>(tcs.Task);
                tcs.SetException(new InvalidOperationException());
                await testTask;
            });
        }

        [Fact]
        public async Task ThrowsAsync_TaskThrowsExpectedException_ReturnsException()
        {
            var expectedException = new InvalidOperationException();
            var tcs = new TaskCompletionSource<object>();
            var testTask = AsyncAssert.ThrowsAsync<InvalidOperationException>(tcs.Task);
            tcs.SetException(expectedException);
            var result = await testTask;
            Assert.Same(expectedException, result);
        }

        [Fact]
        public async Task ThrowsAsync_TaskThrowsDerivedException_ReturnsException()
        {
            var expectedException = new TaskCanceledException();
            var tcs = new TaskCompletionSource<object>();
            var testTask = AsyncAssert.ThrowsAsync<OperationCanceledException>(tcs.Task);
            tcs.SetException(expectedException);
            var result = await testTask;
            Assert.Same(expectedException, result);
        }

        [Fact]
        public async Task ThrowsAsync_TaskThrowsBaseException_Fails()
        {
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                var tcs = new TaskCompletionSource<object>();
                var testTask = AsyncAssert.ThrowsAsync<TaskCanceledException>(tcs.Task);
                tcs.SetException(new OperationCanceledException());
                await testTask;
            });
        }

        [Fact]
        public async Task ThrowsAsync_ExpectingSpecificException_TaskThrowsDerivedException_Fails()
        {
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                var tcs = new TaskCompletionSource<object>();
                var testTask = AsyncAssert.ThrowsAsync<OperationCanceledException>(tcs.Task, false);
                tcs.SetException(new TaskCanceledException());
                await testTask;
            });
        }
    }
}
