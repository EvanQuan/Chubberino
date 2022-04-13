using Chubberino.Bots.Common.Commands.Settings.UserCommands;

namespace Chubberino.UnitTests.Tests.Client.Commands.Pyramids;

public abstract class UsingPyramid : UsingCommand
{
    protected Pyramid Sut { get; private set; }

    public UsingPyramid()
    {
        Sut = new(MockedTwitchClientManager.Object, MockedWriter.Object);
    }
}
