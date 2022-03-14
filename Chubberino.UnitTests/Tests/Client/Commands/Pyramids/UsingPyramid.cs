using Chubberino.Bots.Common.Commands.Settings.Pyramids;
using Chubberino.Client.Commands.Settings.UserCommands;

namespace Chubberino.UnitTests.Tests.Client.Commands.Pyramids
{
    public abstract class UsingPyramid : UsingCommand
    {
        protected Pyramid Sut { get; private set; }

        protected PyramidBuilder PyramidBuilder { get; private set; }

        public UsingPyramid()
        {
            PyramidBuilder = new();
            Sut = new(MockedTwitchClientManager.Object, MockedWriter.Object, PyramidBuilder);
        }
    }
}
