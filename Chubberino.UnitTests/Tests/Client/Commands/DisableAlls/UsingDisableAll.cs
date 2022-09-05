using Chubberino.Bots.Common.Commands;

namespace Chubberino.UnitTests.Tests.Client.Commands.DisableAlls;

public abstract class UsingDisableAll : UsingCommand
{
    protected DisableAll Sut { get; private set; }

    public UsingDisableAll()
    {
        Sut = new DisableAll(MockedTwitchClientManager.Object, MockedCommandRepository.Object, MockedWriter.Object);
    }
}
