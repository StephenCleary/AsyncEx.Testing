using System;
using System.Threading.Tasks;
using Nito.AsyncEx.Testing;
using Xunit;

namespace UnitTests
{
    public partial class UnitTests
    {
        [Fact]
        public async Task ThrowsAsync_DelegateDoesNotThrow_Fails()
        {
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await AsyncAssert.ThrowsAsync(async () =>
                {
                    await Task.Yield();
                });
            });
        }
        
        [Fact]
        public async Task ThrowsAsync_DelegateThrows_ReturnsException()
        {
            var expectedException = new InvalidOperationException();
            var result = await AsyncAssert.ThrowsAsync(async () =>
            {
                await Task.Yield();
                throw expectedException;
            });
            Assert.Same(expectedException, result);
        }

        [Fact]
        public async Task ThrowsAsync_DelegateThrowsWrongException_Fails()
        {
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await AsyncAssert.ThrowsAsync<NotImplementedException>(async () =>
                {
                    await Task.Yield();
                    throw new InvalidOperationException();
                });
            });
        }

        [Fact]
        public async Task ThrowsAsync_DelegateThrowsExpectedException_ReturnsException()
        {
            var expectedException = new InvalidOperationException();
            var result = await AsyncAssert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await Task.Yield();
                throw expectedException;
            });
            Assert.Same(expectedException, result);
        }

        [Fact]
        public async Task ThrowsAsync_DelegateThrowsDerivedException_ReturnsException()
        {
            var expectedException = new TaskCanceledException();
            var result = await AsyncAssert.ThrowsAsync<OperationCanceledException>(async () =>
            {
                await Task.Yield();
                throw expectedException;
            });
            Assert.Same(expectedException, result);
        }

        [Fact]
        public async Task ThrowsAsync_DelegateThrowsBaseException_Fails()
        {
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await AsyncAssert.ThrowsAsync<TaskCanceledException>(async () =>
                {
                    await Task.Yield();
                    throw new OperationCanceledException();
                });
            });
        }

        [Fact]
        public async Task ThrowsAsync_ExpectingSpecificException_DelegateThrowsDerivedException_Fails()
        {
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await AsyncAssert.ThrowsAsync<OperationCanceledException>(async () =>
                {
                    await Task.Yield();
                    throw new TaskCanceledException();
                }, false);
            });
        }
    }
}
