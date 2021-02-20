namespace Chubberino.UnitTests.Tests.Client.Commands.Settings
{
    public abstract class UsingSetting : UsingCommand
    {
        protected SettingExtender Sut { get; }

        public UsingSetting()
        {
            Sut = new SettingExtender(MockedTwitchClientManager.Object, MockedConsole.Object);
        }
    }
}
