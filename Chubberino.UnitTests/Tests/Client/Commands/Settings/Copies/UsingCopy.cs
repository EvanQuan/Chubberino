using Chubberino.Bots.Common.Commands.Settings;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.Copies
{
    public abstract class UsingCopy : UsingCommand
    {
        protected Copy Sut { get; private set; }

        public UsingCopy()
        {
            Sut = new Copy(MockedTwitchClientManager.Object, MockedWriter.Object);
        }
    }
}
