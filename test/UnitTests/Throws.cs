using System;
using System.Threading.Tasks;
using Nito.AsyncEx.Testing;
using Xunit;

namespace UnitTests
{
    public partial class UnitTests
    {
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
