using System;
using System.Threading.Tasks;
using Nito.AsyncEx.Testing;
using Xunit;

namespace UnitTests
{
    public partial class UnitTests
    {
        [Fact]
        public async Task CancelsAsync_TaskDoesNotCancel_Fails()
        {
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                var tcs = new TaskCompletionSource<object>();
                var testTask = AsyncAssert.CancelsAsync(tcs.Task);
                tcs.SetResult(null);
                await testTask;
            });
        }

        [Fact]
        public async Task CancelsAsync_TaskThrowsWrongException_Fails()
        {
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                var tcs = new TaskCompletionSource<object>();
                var testTask = AsyncAssert.CancelsAsync(tcs.Task);
                tcs.SetException(new InvalidOperationException());
                await testTask;
            });
        }

        [Fact]
        public async Task CancelsAsync_TaskCancels_ReturnsException()
        {
            var expectedException = new OperationCanceledException();
            var tcs = new TaskCompletionSource<object>();
            var testTask = AsyncAssert.CancelsAsync(tcs.Task);
            tcs.SetException(expectedException);
            var result = await testTask;
            Assert.Same(expectedException, result);
        }

        [Fact]
        public async Task CancelsAsync_TaskCancelsWithDerivedException_ReturnsException()
        {
            var expectedException = new TaskCanceledException();
            var tcs = new TaskCompletionSource<object>();
            var testTask = AsyncAssert.CancelsAsync(tcs.Task);
            tcs.SetException(expectedException);
            var result = await testTask;
            Assert.Same(expectedException, result);
        }

        [Fact]
        public async Task CancelsAsync_TaskThrowsBaseException_Fails()
        {
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                var tcs = new TaskCompletionSource<object>();
                var testTask = AsyncAssert.CancelsAsync(tcs.Task);
                tcs.SetException(new Exception());
                await testTask;
            });
        }
    }
}
