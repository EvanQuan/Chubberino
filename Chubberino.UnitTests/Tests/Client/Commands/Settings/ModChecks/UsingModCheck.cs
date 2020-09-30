using Chubberino.Client.Abstractions;
using Chubberino.Client.Commands.Settings;
using Chubberino.Client.Commands.Strategies;
using Moq;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.ModChecks
{
    public abstract class UsingModCheck :  UsingCommand
    {
        protected ModCheck Sut { get; private set; }

        protected Mock<ICommandRepository> MockedCommandRepository { get; private set; }

        protected Mock<IStopSettingStrategy> MockedStopSettingStrategy { get; private set; }

        public UsingModCheck()
        {
            MockedCommandRepository = new Mock<ICommandRepository>().SetupAllProperties();
            MockedStopSettingStrategy = new Mock<IStopSettingStrategy>().SetupAllProperties();

            Sut = new ModCheck(
                MockedTwitchClient.Object,
                MockedConsole.Object,
                MockedCommandRepository.Object,
                MockedStopSettingStrategy.Object);
        }
    }
}
