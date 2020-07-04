using Moq;
using MouseBot.Implementation.Commands;
using System.Diagnostics;
using TwitchLib.Client.Interfaces;

namespace MouseBot.UnitTests.Tests.Implementation.UsingTimedQueue
{
    public abstract class MessageTimerTestBase
    {
        protected MessageTimer Sut { get; }

        protected Mock<ITwitchClient> TwitchClient { get; }

        protected Stopwatch Stopwatch { get; } = new Stopwatch();

        public MessageTimerTestBase()
        {
            TwitchClient = new Mock<ITwitchClient>();
            Sut = new MessageTimer(TwitchClient.Object);
        }
    }
}
