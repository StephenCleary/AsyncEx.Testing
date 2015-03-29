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

        [Fact]
        public void Throws_DelegateDoesNotThrow_Fails()
        {
            Assert.Throws<Exception>(() =>
            {
                AsyncAssert.Throws(() => { });
            });
        }

        [Fact]
        public void Throws_DelegateThrows_ReturnsException()
        {
            var expectedException = new InvalidOperationException();
            var result = AsyncAssert.Throws(() => { throw expectedException; });
            Assert.Same(expectedException, result);
        }

        [Fact]
        public void Throws_DelegateThrowsWrongException_Fails()
        {
            Assert.Throws<Exception>(() =>
            {
                AsyncAssert.Throws<NotImplementedException>(() => { throw new InvalidOperationException(); });
            });
        }

        [Fact]
        public void Throws_DelegateThrowsExpectedException_ReturnsException()
        {
            var expectedException = new InvalidOperationException();
            var result = AsyncAssert.Throws<InvalidOperationException>(() => { throw expectedException; });
            Assert.Same(expectedException, result);
        }

        [Fact]
        public void Throws_DelegateThrowsDerivedException_ReturnsException()
        {
            var expectedException = new TaskCanceledException();
            var result = AsyncAssert.Throws<OperationCanceledException>(() => { throw expectedException; });
            Assert.Same(expectedException, result);
        }

        [Fact]
        public void Throws_DelegateThrowsBaseException_Fails()
        {
            Assert.Throws<Exception>(() =>
            {
                AsyncAssert.Throws<TaskCanceledException>(() => { throw new OperationCanceledException(); });
            });
        }

        [Fact]
        public void Throws_ExpectingSpecificException_DelegateThrowsDerivedException_Fails()
        {
            Assert.Throws<Exception>(() =>
            {
                AsyncAssert.Throws<OperationCanceledException>(() => { throw new TaskCanceledException(); }, false);
            });
        }
    }
}
