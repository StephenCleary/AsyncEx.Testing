using System;
using System.Threading.Tasks;
using Nito.AsyncEx.Testing;
using Xunit;

namespace UnitTests
{
    public partial class UnitTests
    {
        [Fact]
        public async Task NeverCompletesAsync_TaskCompletes_Fails()
        {
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                var tcs = new TaskCompletionSource<object>();
                var testTask = AsyncAssert.NeverCompletesAsync(tcs.Task);
                tcs.SetResult(null);
                await testTask;
            });
        }

        [Fact]
        public async Task NeverCompletesAsync_TaskFaults_Fails()
        {
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                var tcs = new TaskCompletionSource<object>();
                var testTask = AsyncAssert.NeverCompletesAsync(tcs.Task);
                tcs.SetException(new Exception());
                await testTask;
            });
        }

        [Fact]
        public async Task NeverCompletesAsync_SynchronousTaskCompletes_Fails()
        {
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await AsyncAssert.NeverCompletesAsync(Task.FromResult(0));
            });
        }

        [Fact]
        public async Task NeverCompletesAsync_NeverCompletes_Passes()
        {
            var tcs = new TaskCompletionSource<object>();
            await AsyncAssert.NeverCompletesAsync(tcs.Task);
        }
    }
}
