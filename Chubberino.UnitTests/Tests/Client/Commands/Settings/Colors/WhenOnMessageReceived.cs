﻿using Chubberino.Infrastructure.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models.Builders;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.Colors;

public sealed class WhenOnMessageReceived : UsingColor
{
    public WhenOnMessageReceived()
    {
        Sut.AddColorSelector(MockedSelector1.Object);
    }

    [Fact]
    public void ShouldIgnoreNonBotUsername()
    {
        var args = new OnMessageReceivedArgs()
        {
            ChatMessage = ChatMessageBuilder
                .Create()
                .WithTwitchLibMessage(TwitchLibMessageBuilder
                    .Create()
                    .WithUsername(Username)
                    .WithBotUserName(BotUsername))
                .WithMessage(Message)
                .Build()
        };

        Sut.Client_OnMessageReceived(null, args);

        MockedTwitchClientManager
            .Verify(x => x.SpoolMessage(PrimaryChannelName, It.IsAny<String>(), It.IsAny<Priority>()), Times.Never());
    }

    [Fact]
    public void ShouldSetColorFromColorSelector()
    {
        var args = new OnMessageReceivedArgs()
        {
            ChatMessage = ChatMessageBuilder
                .Create()
                .WithTwitchLibMessage(TwitchLibMessageBuilder
                    .Create()
                    .WithUsername(BotUsername)
                    .WithBotUserName(BotUsername))
                .WithMessage(Message)
                .Build()
        };

        Sut.Client_OnMessageReceived(null, args);

        MockedTwitchClientManager
            .Verify(x => x.SpoolMessage(PrimaryChannelName, $".color {Color1}", Priority.Medium), Times.Once());
    }
}
