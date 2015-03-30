using System;
using System.Threading.Tasks;
using Nito.AsyncEx.Testing;
using Xunit;

namespace UnitTests
{
    public partial class UnitTests
    {
        [Fact]
        public async Task CancelsAsync_SynchronousDelegateDoesNotCancel_Fails()
        {
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await AsyncAssert.CancelsAsync(() => Task.FromResult(0));
            });
        }

        [Fact]
        public async Task CancelsAsync_SynchronousDelegateThrowsWrongException_Fails()
        {
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await AsyncAssert.CancelsAsync(() => { throw new InvalidOperationException(); });
            });
        }

        [Fact]
        public async Task CancelsAsync_SynchronousDelegateCancels_ReturnsException()
        {
            var expectedException = new OperationCanceledException();
            var result = await AsyncAssert.CancelsAsync(() => { throw expectedException; });
            Assert.Same(expectedException, result);
        }

        [Fact]
        public async Task CancelsAsync_SynchronousDelegateCancelsWithDerivedException_ReturnsException()
        {
            var expectedException = new TaskCanceledException();
            var result = await AsyncAssert.CancelsAsync(() => { throw expectedException; });
            Assert.Same(expectedException, result);
        }

        [Fact]
        public async Task CancelsAsync_SynchronousDelegateThrowsBaseException_Fails()
        {
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await AsyncAssert.CancelsAsync(() => { throw new Exception(); });
            });
        }
    }
}
