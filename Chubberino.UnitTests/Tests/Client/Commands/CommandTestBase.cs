using Chubberino.Client.Abstractions;
using Moq;
using TwitchLib.Client.Interfaces;

namespace Chubberino.UnitTests.Tests.Client.Commands
{
    public abstract class CommandTestBase
    {
        protected Mock<ITwitchClient> TwitchClient { get; }

        protected Mock<IMessageSpooler> Spooler { get; }

        public CommandTestBase()
        {
            TwitchClient = new Mock<ITwitchClient>().SetupAllProperties();

            Spooler = new Mock<IMessageSpooler>().SetupAllProperties();
        }

    }
}
