using Chubberino.Client.Abstractions;
using Moq;
using System.IO;

namespace Chubberino.UnitTests.Tests.Client.Commands
{
    public abstract class UsingCommand
    {
        protected Mock<IExtendedClient> MockedTwitchClient { get; }

        protected Mock<IRepeater> MockedRepeater { get; }

        protected Mock<TextWriter> MockedConsole { get; }

        public UsingCommand()
        {
            MockedConsole = new Mock<TextWriter>().SetupAllProperties();

            MockedTwitchClient = new Mock<IExtendedClient>().SetupAllProperties();

            MockedRepeater = new Mock<IRepeater>().SetupAllProperties();
        }

    }
}
