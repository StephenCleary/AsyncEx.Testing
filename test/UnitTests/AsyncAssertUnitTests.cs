using System;
using System.Threading.Tasks;
using Nito.AsyncEx.Testing;
using Xunit;

namespace UnitTests
{
    public class UnitTests
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
        public async Task ThrowsAsync_DelegateThrowsExpectedException_Passes()
        {
            await AsyncAssert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await Task.Yield();
                throw new InvalidOperationException();
            });
        }

        [Fact]
        public async Task ThrowsAsync_DelegateThrowsDerivedException_Passes()
        {
            await AsyncAssert.ThrowsAsync<OperationCanceledException>(async () =>
            {
                await Task.Yield();
                throw new TaskCanceledException();
            });
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
