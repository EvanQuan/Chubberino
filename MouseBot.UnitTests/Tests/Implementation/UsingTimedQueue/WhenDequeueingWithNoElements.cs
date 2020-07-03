using Xunit;

namespace MouseBot.UnitTests.Tests.Implementation.UsingTimedQueue
{
    public sealed class WhenDequeueingWithNoElements : TimedQueueTestBase
    {
        [Fact]
        public void ShouldReturnFalse()
        {
            Assert.False(Sut.TryDequeue(out _));
        }
    }
}
