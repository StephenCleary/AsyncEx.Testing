using System;
using System.Threading.Tasks;
using Nito.AsyncEx.Testing;
using Xunit;

namespace UnitTests
{
    public partial class UnitTests
    {
        [Fact]
        public void Cancels_DelegateDoesNotCancel_Fails()
        {
            Assert.Throws<Exception>(() =>
            {
                AsyncAssert.Cancels(() => { });
            });
        }

        [Fact]
        public void Cancels_DelegateThrowsWrongException_Fails()
        {
            Assert.Throws<Exception>(() =>
            {
                AsyncAssert.Cancels(() => { throw new InvalidOperationException(); });
            });
        }

        [Fact]
        public void Cancels_DelegateCancels_ReturnsException()
        {
            var expectedException = new OperationCanceledException();
            var result = AsyncAssert.Cancels(() => { throw expectedException; });
            Assert.Same(expectedException, result);
        }

        [Fact]
        public void Cancels_DelegateCancelsWithDerivedException_ReturnsException()
        {
            var expectedException = new TaskCanceledException();
            var result = AsyncAssert.Cancels(() => { throw expectedException; });
            Assert.Same(expectedException, result);
        }

        [Fact]
        public void Cancels_DelegateThrowsBaseException_Fails()
        {
            Assert.Throws<Exception>(() =>
            {
                AsyncAssert.Cancels(() => { throw new Exception(); });
            });
        }
    }
}
