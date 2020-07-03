using MouseBot.Implementation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using Xunit;

namespace MouseBot.UnitTests.Tests.Implementation.UsingTimedQueue
{
    public sealed class WhenDequeuingWithElements : TimedQueueTestBase
    {
        private Stopwatch Stopwatch { get; } = new Stopwatch();

        [Fact]
        public void ShouldDequeueImmediatelyForImmediateEnqueue()
        {
            Sut.Enqueue(String.Empty);
            Stopwatch.Start();
            Boolean dequeueSuccessful = Sut.TryDequeue(out String message);
            Stopwatch.Stop();

            Assert.True(dequeueSuccessful, "Expected dequeue to succeed, but failed.");
            Assert.Equal(String.Empty, message);
            Assert.True(Stopwatch.Elapsed < Sut.Interval, $"Expected dequeue time to be less than {Sut.Interval}, but was {Stopwatch.Elapsed}");
        }

        [Fact]
        public void ShouldNotDequeueIfIntervalNotElapsed()
        {
            Sut.Enqueue(String.Empty);
            Sut.Enqueue(String.Empty);

            Sut.TryDequeue(out _);

            Boolean dequeueSuccessful = Sut.TryDequeue(out _);

            Assert.False(dequeueSuccessful, "Expected dequeue to fail, but succeeded.");
        }

        [Fact]
        public void ShouldDequeueIfIntervalElapsed()
        {
            const String expectedMessage = "second";
            Sut.Enqueue(String.Empty);
            Sut.Enqueue(expectedMessage);

            Sut.TryDequeue(out _);

            SpinWait.SpinUntil(() => false, Sut.Interval + TimeSpan.FromSeconds(0.1));

            Boolean dequeueSuccessful = Sut.TryDequeue(out String message);

            Assert.True(dequeueSuccessful, "Expected dequeue to succeed, but failed.");
            Assert.Equal(expectedMessage, message);
        }
    }
}
