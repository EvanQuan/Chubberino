using Chubberino.Client.Abstractions;
using Moq;

namespace Chubberino.UnitTests.Tests.Client.Commands
{
    public abstract class UsingCommand
    {
        protected Mock<IExtendedClient> MockedTwitchClient { get; }

        protected Mock<IRepeater> MockedRepeater { get; }

        public UsingCommand()
        {
            MockedTwitchClient = new Mock<IExtendedClient>().SetupAllProperties();

            MockedRepeater = new Mock<IRepeater>().SetupAllProperties();
        }

    }
}
