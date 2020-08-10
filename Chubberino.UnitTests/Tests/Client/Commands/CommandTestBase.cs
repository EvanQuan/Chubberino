using Chubberino.Client.Abstractions;
using Chubberino.Client.Commands.Strategies;
using Moq;
using TwitchLib.Client.Interfaces;

namespace Chubberino.UnitTests.Tests.Client.Commands
{
    public abstract class CommandTestBase
    {
        protected Mock<ITwitchClient> MockedTwitchClient { get; }

        protected Mock<IMessageSpooler> MockedSpooler { get; }

        protected Mock<IRepeater> MockedRepeater { get; }

        protected Mock<IStopSettingStrategy> MockedStopSettingStrategy { get; }

        public CommandTestBase()
        {
            MockedTwitchClient = new Mock<ITwitchClient>().SetupAllProperties();

            MockedSpooler = new Mock<IMessageSpooler>().SetupAllProperties();

            MockedRepeater = new Mock<IRepeater>().SetupAllProperties();

            MockedStopSettingStrategy = new Mock<IStopSettingStrategy>().SetupAllProperties();
        }

    }
}
