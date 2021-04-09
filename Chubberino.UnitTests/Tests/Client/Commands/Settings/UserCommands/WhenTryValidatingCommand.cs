using Chubberino.Client.Commands.Settings.UserCommands;
using System;
using System.Linq;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models.Builders;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.UserCommands
{
    public sealed class WhenTryValidatingCommand : UsingUserCommand
    {
        [Theory]
        [InlineData("!usercommandwrapper", new String[] { })]
        [InlineData("!userCommandWrapper", new String[] { })]
        [InlineData("!userCommandWrapper a", new String[] { "a" })]
        [InlineData("!userCommandWrapper a b", new String[] { "a", "b" })]
        public void ShouldValidateCommand(String input, String[] expectedWords)
        {
            Boolean result = Sut.TryValidateCommand(new OnMessageReceivedArgs()
            {
                ChatMessage = ChatMessageBuilder.Create()
                .WithMessage(input)
                .Build()
            },
            out var words);

            Assert.True(result);
            Assert.True(expectedWords.SequenceEqual(words));
        }

        [Theory]
        [InlineData("!usercommandwrapper")]
        [InlineData("!userCommandWrapper")]
        [InlineData("!userCommandWrapper a")]
        [InlineData("!userCommandWrapper a b")]
        public void ShouldIgnoreUser(String input)
        {
            foreach (String id in UserCommand.UserIdsToIgnore)
            {
                Boolean result = Sut.TryValidateCommand(new OnMessageReceivedArgs()
                {
                    ChatMessage = ChatMessageBuilder.Create()
                    .WithTwitchLibMessage(TwitchLibMessageBuilder.Create().WithUserId(id).Build())
                    .WithMessage(input)
                    .Build()
                },
                out var words);

                Assert.False(result);
                Assert.Null(words);
            }
        }

        [Theory]
        [InlineData("!a")]
        [InlineData("!usercommandwrappera")]
        [InlineData("usercommandwrapper")]
        [InlineData("userCommandWrapper")]
        [InlineData("!userCommandWrappera")]
        [InlineData("!userCommandWrappera a")]
        [InlineData("!userCommandWrappera a b")]
        public void ShouldInvalidateCommand(String input)
        {
            Boolean result = Sut.TryValidateCommand(new OnMessageReceivedArgs()
            {
                ChatMessage = ChatMessageBuilder.Create()
                .WithMessage(input)
                .Build()
            },
            out var words);

            Assert.False(result);
            Assert.Null(words);
        }
    }
}
