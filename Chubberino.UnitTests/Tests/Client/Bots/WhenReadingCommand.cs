using Chubberino.Client;
using Chubberino.Infrastructure.Client;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Bots
{
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
            MockedCommandRepository.Verify(x => x.Execute(It.IsAny<String>(), It.IsAny<IEnumerable<String>>()), Times.Never());
        }

        [Theory]
        [InlineData("a")]
        [InlineData("a b")]
        [InlineData("a  b")]
        [InlineData("a b c")]
        public void ShouldExecuteCommand(String command)
        {
            MockedCommandRepository
                .Setup(x => x.Execute(It.IsAny<String>(), It.IsAny<IEnumerable<String>>()))
                .Callback((String command, IEnumerable<String> args) =>
                {
                    Assert.DoesNotContain(args, x => String.IsNullOrWhiteSpace(x));
                });

            Sut.ReadCommand(command);

            Assert.Equal(BotState.ShouldContinue, Sut.State);
            MockedCommandRepository.Verify(x => x.Execute("a", It.IsAny<IEnumerable<String>>()), Times.Once());
        }
    }
}
