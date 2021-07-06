using Chubberino.Bots.Common.Commands.Settings;
using Chubberino.Bots.Common.Commands.Settings.Strategies;
using Moq;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.ModChecks
{
    public abstract class UsingModCheck :  UsingCommand
    {
        protected ModCheck Sut { get; private set; }

        protected Mock<IStopSettingStrategy> MockedStopSettingStrategy { get; private set; }

        public UsingModCheck()
        {
            MockedStopSettingStrategy = new Mock<IStopSettingStrategy>().SetupAllProperties();

            Sut = new ModCheck(
                MockedTwitchClientManager.Object,
                MockedWriter.Object,
                MockedCommandRepository.Object,
                MockedStopSettingStrategy.Object);
        }
    }
}
