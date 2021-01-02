using Chubberino.Client.Commands;

namespace Chubberino.UnitTests.Tests.Client.Commands.Pyramids
{
    public abstract class UsingPyramid : UsingCommand
    {
        protected PyramidBuild Sut { get; private set; }

        public UsingPyramid()
        {
            Sut = new Pyramid(MockedTwitchClient.Object, MockedConsole.Object);
        }
    }
}
