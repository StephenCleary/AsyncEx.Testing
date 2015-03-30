using System;
using System.Threading.Tasks;
using Nito.AsyncEx.Testing;
using Xunit;

namespace UnitTests
{
    public partial class UnitTests
    {
        [Fact]
        public async Task CancelsAsync_SynchronousTaskDoesNotCancel_Fails()
        {
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await AsyncAssert.CancelsAsync(Task.FromResult(0));
            });
        }

        [Fact]
        public async Task CancelsAsync_SynchronousTaskThrowsWrongException_Fails()
        {
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await AsyncAssert.CancelsAsync(Task.FromException(new InvalidOperationException()));
            });
        }

        [Fact]
        public async Task CancelsAsync_SynchronousTaskCancels_ReturnsException()
        {
            var expectedException = new OperationCanceledException();
            var result = await AsyncAssert.CancelsAsync(Task.FromException(expectedException));
            Assert.Same(expectedException, result);
        }

        [Fact]
        public async Task CancelsAsync_SynchronousTaskCancelsWithDerivedException_ReturnsException()
        {
            var expectedException = new TaskCanceledException();
            var result = await AsyncAssert.CancelsAsync(Task.FromException(expectedException));
            Assert.Same(expectedException, result);
        }

        [Fact]
        public async Task CancelsAsync_SynchronousTaskThrowsBaseException_Fails()
        {
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await AsyncAssert.CancelsAsync(Task.FromException(new Exception()));
            });
        }
    }
}
