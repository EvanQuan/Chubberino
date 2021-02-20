using Chubberino.Client.Commands.Settings;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.Repeats
{
    public abstract class UsingRepeat : UsingCommand
    {
        protected Repeat Sut { get; private set; }

        public UsingRepeat()
        {
            Sut = new Repeat(MockedTwitchClientManager.Object, MockedRepeater.Object, MockedConsole.Object);
        }
    }
}
