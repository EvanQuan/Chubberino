using Chubberino.Client.Abstractions;
using Chubberino.Client.Commands.Strategies;
using Moq;
using TwitchLib.Client.Interfaces;

namespace Chubberino.UnitTests.Tests.Client.Commands
{
    public abstract class CommandTestBase
    {
        protected Mock<IExtendedClient> MockedTwitchClient { get; }

        protected Mock<IRepeater> MockedRepeater { get; }

        public CommandTestBase()
        {
            MockedTwitchClient = new Mock<IExtendedClient>().SetupAllProperties();

            MockedRepeater = new Mock<IRepeater>().SetupAllProperties();
        }

    }
}
