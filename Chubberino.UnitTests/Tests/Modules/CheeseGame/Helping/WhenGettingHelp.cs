using Chubberino.Client;
using Chubberino.Modules.CheeseGame.Helping;
using Moq;
using System;
using TwitchLib.Client.Models;
using TwitchLib.Client.Models.Builders;
using Xunit;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Helping
{
    public sealed class WhenGettingHelp : UsingHelpManager
    {
        [Theory]
        [InlineData("!cheese help", HelpManager.Messages.Generic)]
        [InlineData("!cheese h", HelpManager.Messages.Generic)]
        [InlineData("!cheese help ", HelpManager.Messages.Generic)]
        [InlineData("!cheese h ", HelpManager.Messages.Generic)]
        [InlineData("!cheese help S", HelpManager.Messages.Storage)]
        [InlineData("!cheese help sToRaGe", HelpManager.Messages.Storage)]
        [InlineData("!cheese help p", HelpManager.Messages.Population)]
        public void ShouldSendMessage(String message, String expectedMessage)
        {
            ChatMessage chatMessage = ChatMessageBuilder
                .Create()
                .WithMessage(message)
                .WithChannel(ChannelName)
                .WithTwitchLibMessage(TwitchLibMessageBuilder
                    .Create()
                    .WithDisplayName(Players[0].Name))
                .Build();

            Sut.GetHelp(chatMessage);

            MockedClient.Verify(
                x => x.SpoolMessage(
                    ChannelName,
                    It.Is<String>(x => x.Contains(expectedMessage)),
                    Priority.Low),
                Times.Once());
        }

        [Theory]
        [InlineData("!cheese help invalid", "invalid")]
        [InlineData("!cheese h invalid", "invalid")]
        [InlineData("!cheese help iNvAlId", "iNvAlId")]
        [InlineData("!cheese h iNvAlId", "iNvAlId")]
        public void ShouldSendInvalidItemMessage(String message, String expectedItem)
        {
            ChatMessage chatMessage = ChatMessageBuilder
                .Create()
                .WithMessage(message)
                .WithChannel(ChannelName)
                .WithTwitchLibMessage(TwitchLibMessageBuilder
                    .Create()
                    .WithDisplayName(Players[0].Name))
                .Build();

            Sut.GetHelp(chatMessage);

            var expectedMessage = String.Format(HelpManager.Messages.Invalid, expectedItem);

            MockedClient.Verify(
                x => x.SpoolMessage(
                    ChannelName,
                    It.Is<String>(x => x.Contains(expectedMessage)),
                    Priority.Low),
                Times.Once());
        }
    }
}
