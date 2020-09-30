using Chubberino.Client.Commands.Settings;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.Logs
{
    public abstract class UsingLog : UsingCommand
    {
        protected Log Sut { get; private set; }

        public UsingLog()
        {
            Sut = new Log(MockedTwitchClient.Object, MockedConsole.Object);
        }
    }
}
