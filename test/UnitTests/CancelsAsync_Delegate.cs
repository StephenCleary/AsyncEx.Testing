using System;
using System.Threading.Tasks;
using Nito.AsyncEx.Testing;
using Xunit;

namespace UnitTests
{
    public partial class UnitTests
    {
        [Fact]
        public async Task CancelsAsync_DelegateDoesNotCancel_Fails()
        {
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await AsyncAssert.CancelsAsync(async () => { await Task.Yield(); });
            });
        }

        [Fact]
        public async Task CancelsAsync_DelegateThrowsWrongException_Fails()
        {
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await AsyncAssert.CancelsAsync(async () => { await Task.Yield(); throw new InvalidOperationException(); });
            });
        }

        [Fact]
        public async Task CancelsAsync_DelegateCancels_ReturnsException()
        {
            var expectedException = new OperationCanceledException();
            var result = await AsyncAssert.CancelsAsync(async () => { await Task.Yield(); throw expectedException; });
            Assert.Same(expectedException, result);
        }

        [Fact]
        public async Task CancelsAsync_DelegateCancelsWithDerivedException_ReturnsException()
        {
            var expectedException = new TaskCanceledException();
            var result = await AsyncAssert.CancelsAsync(async () => { await Task.Yield(); throw expectedException; });
            Assert.Same(expectedException, result);
        }

        [Fact]
        public async Task CancelsAsync_DelegateThrowsBaseException_Fails()
        {
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await AsyncAssert.CancelsAsync(async () => { await Task.Yield(); throw new Exception(); });
            });
        }
    }
}
