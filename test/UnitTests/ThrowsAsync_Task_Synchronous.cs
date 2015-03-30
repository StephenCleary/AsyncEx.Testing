using System;
using System.Threading.Tasks;
using Nito.AsyncEx.Testing;
using Xunit;

namespace UnitTests
{
    public partial class UnitTests
    {
        [Fact]
        public async Task ThrowsAsync_SynchronousTaskDoesNotThrow_Fails()
        {
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await AsyncAssert.ThrowsAsync(Task.FromResult(0));
            });
        }

        [Fact]
        public async Task ThrowsAsync_SynchronousTaskThrows_ReturnsException()
        {
            var expectedException = new InvalidOperationException();
            var result = await AsyncAssert.ThrowsAsync(Task.FromException(expectedException));
            Assert.Same(expectedException, result);
        }

        [Fact]
        public async Task ThrowsAsync_SynchronousTaskThrowsWrongException_Fails()
        {
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await AsyncAssert.ThrowsAsync<NotImplementedException>(Task.FromException(new InvalidOperationException()));
            });
        }

        [Fact]
        public async Task ThrowsAsync_SynchronousTaskThrowsExpectedException_ReturnsException()
        {
            var expectedException = new InvalidOperationException();
            var result = await AsyncAssert.ThrowsAsync<InvalidOperationException>(Task.FromException(expectedException));
            Assert.Same(expectedException, result);
        }

        [Fact]
        public async Task ThrowsAsync_SynchronousTaskThrowsDerivedException_ReturnsException()
        {
            var expectedException = new TaskCanceledException();
            var result = await AsyncAssert.ThrowsAsync<OperationCanceledException>(Task.FromException(expectedException));
            Assert.Same(expectedException, result);
        }

        [Fact]
        public async Task ThrowsAsync_SynchronousTaskThrowsBaseException_Fails()
        {
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await AsyncAssert.ThrowsAsync<TaskCanceledException>(Task.FromException(new OperationCanceledException()));
            });
        }

        [Fact]
        public async Task ThrowsAsync_ExpectingSpecificException_SynchronousTaskThrowsDerivedException_Fails()
        {
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await AsyncAssert.ThrowsAsync<OperationCanceledException>(Task.FromException(new TaskCanceledException()), false);
            });
        }
    }
}
