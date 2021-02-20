using Chubberino.Client.Commands;
using Chubberino.Client.Commands.Pyramids;
using Moq;

namespace Chubberino.UnitTests.Tests.Client.Commands.Pyramids
{
    public abstract class UsingPyramidBuild : UsingCommand
    {
        protected PyramidBuild Sut { get; private set; }

        protected PyramidBuilder PyramidBuilder { get; private set; }

        public UsingPyramidBuild()
        {
            PyramidBuilder = new PyramidBuilder();
            Sut = new PyramidBuild(MockedTwitchClientManager.Object, MockedConsole.Object, PyramidBuilder);
        }
    }
}
