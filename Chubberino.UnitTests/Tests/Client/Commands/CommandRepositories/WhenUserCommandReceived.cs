using Chubberino.Client.Commands.Settings.UserCommands;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Client.Models.Builders;

namespace Chubberino.UnitTests.Tests.Client.Commands.CommandRepositories;

public sealed class WhenUserCommandReceived : UsingCommandRepository
{
    private ChatMessage ChatMessage { get; }

    private OnMessageReceivedArgs MessageArgs { get; }
    private OnUserCommandReceivedArgs UserCommandArgs { get; }

    public WhenUserCommandReceived()
    {
        Sut
            .AddCommand(MockedUserCommand1.Object)
            .AddCommand(MockedUserCommand2.Object);

        ChatMessage = ChatMessageBuilder
            .Create()
            .Build();

        Sut.Enable(MockedUserCommand1.Object.Name);

        var userCommandName = MockedUserCommand1.Object.Name;

        UserCommandArgs = new OnUserCommandReceivedArgs(ChatMessage, Array.Empty<String>());

        var userCommandArgs = UserCommandArgs;

        MockedUserCommandValidator
            .Setup(x => x.TryValidateCommand(ChatMessage, out userCommandName, out userCommandArgs))
            .Returns(true);

        MessageArgs = new OnMessageReceivedArgs()
        {
            ChatMessage = ChatMessage
        };

        MockedTwitchClient.Raise(x => x.OnMessageReceived += null, MessageArgs);
    }

    [Fact]
    public void ShouldInvokeRelevantUserCommand()
    {
        MockedUserCommand1.Verify(x => x.Invoke(Sut, UserCommandArgs), Times.Once());
        MockedUserCommand2.Verify(x => x.Invoke(Sut, UserCommandArgs), Times.Never());
    }
}
