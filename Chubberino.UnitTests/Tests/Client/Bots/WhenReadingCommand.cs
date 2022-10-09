using Chubberino.Common.ValueObjects;
using Chubberino.Infrastructure.Client;
using System.Collections.Generic;
using System.Linq;

namespace Chubberino.UnitTests.Tests.Client.Bots;

public sealed class WhenReadingCommand : UsingBot
{
    [Theory]
    [InlineData("quit")]
    [InlineData("QUIT")]
    [InlineData("qUiT")]
    public void ShouldSetStateToStop(String quitCommand)
    {
        Sut.ReadCommand(quitCommand);

        Assert.Equal(BotState.ShouldStop, Sut.State);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("  ")]
    public void ShouldDoNothing(String command)
    {
        Sut.ReadCommand(command);

        Assert.Equal(BotState.ShouldContinue, Sut.State);
        MockedCommandRepository.Verify(x => x.Execute(It.IsAny<Name>(), It.IsAny<IEnumerable<String>>()), Times.Never());
    }

    [Theory]
    [InlineData("a")]
    [InlineData("a b")]
    [InlineData("a  b")]
    [InlineData("a b c")]
    public void ShouldExecuteCommand(String command)
    {
        Sut.ReadCommand(command);

        Assert.Equal(BotState.ShouldContinue, Sut.State);
        MockedCommandRepository.Verify(x => x
            .Execute(
                Name.From("a"),
                It.Is<IEnumerable<String>>(args => !args.Any(String.IsNullOrWhiteSpace))),
            Times.Once);
    }
}
