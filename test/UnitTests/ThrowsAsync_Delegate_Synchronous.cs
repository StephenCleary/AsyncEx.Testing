using System;
using System.Threading.Tasks;
using Nito.AsyncEx.Testing;
using Xunit;

namespace UnitTests
{
    public partial class UnitTests
    {
        [Fact]
        public async Task ThrowsAsync_SynchronousDelegateDoesNotThrow_Fails()
        {
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await AsyncAssert.ThrowsAsync(() => Task.FromResult(0));
            });
        }

        [Fact]
        public async Task ThrowsAsync_SynchronousDelegateThrows_ReturnsException()
        {
            var expectedException = new InvalidOperationException();
            var result = await AsyncAssert.ThrowsAsync(() =>
            {
                throw expectedException;
            });
            Assert.Same(expectedException, result);
        }

        [Fact]
        public async Task ThrowsAsync_SynchronousDelegateThrowsWrongException_Fails()
        {
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await AsyncAssert.ThrowsAsync<NotImplementedException>(() =>
                {
                    throw new InvalidOperationException();
                });
            });
        }

        [Fact]
        public async Task ThrowsAsync_SynchronousDelegateThrowsExpectedException_ReturnsException()
        {
            var expectedException = new InvalidOperationException();
            var result = await AsyncAssert.ThrowsAsync<InvalidOperationException>(() =>
            {
                throw expectedException;
            });
            Assert.Same(expectedException, result);
        }

        [Fact]
        public async Task ThrowsAsync_SynchronousDelegateThrowsDerivedException_ReturnsException()
        {
            var expectedException = new TaskCanceledException();
            var result = await AsyncAssert.ThrowsAsync<OperationCanceledException>(() =>
            {
                throw expectedException;
            });
            Assert.Same(expectedException, result);
        }

        [Fact]
        public async Task ThrowsAsync_SynchronousDelegateThrowsBaseException_Fails()
        {
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await AsyncAssert.ThrowsAsync<TaskCanceledException>(() =>
                {
                    throw new OperationCanceledException();
                });
            });
        }

        [Fact]
        public async Task ThrowsAsync_ExpectingSpecificException_SynchronousDelegateThrowsDerivedException_Fails()
        {
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await AsyncAssert.ThrowsAsync<OperationCanceledException>(() =>
                {
                    throw new TaskCanceledException();
                }, false);
            });
        }
    }
}
