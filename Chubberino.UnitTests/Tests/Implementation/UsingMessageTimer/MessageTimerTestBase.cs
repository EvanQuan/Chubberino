using Chubberino.Implementation.Commands;
using Moq;
using System.Diagnostics;
using TwitchLib.Client.Interfaces;

namespace Chubberino.UnitTests.Tests.Implementation.UsingMessageTimer
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
