using System;
using Chubberino.Client.Commands.Settings.UserCommands;
using Chubberino.Infrastructure.Client;
using Chubberino.Infrastructure.Commands.Settings.UserCommands;
using TwitchLib.Client.Models.Builders;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.UserCommands
{
    public sealed class WhenTryValidatingCommand : UsingUserCommand
    {
        [Theory]
        [InlineData("!name", "name", new String[] { })]
        [InlineData("!NAME", "name", new String[] { })]
        [InlineData("!nAmE", "name", new String[] { })]
        [InlineData("!name a", "name", new String[] { "a" })]
        [InlineData("!name A", "name", new String[] { "A" })]
        [InlineData("!name a b", "name", new String[] { "a", "b" })]
        public void ShouldValidateCommand(String input, String expectedUserCommandName, String[] expectedWords)
        {
            Boolean result = Sut.TryValidateCommand(
                ChatMessageBuilder.Create()
                    .WithMessage(input)
                    .Build(),
                out var userCommandName,
                out var args);

            Assert.True(result);
            Assert.Equal(expectedUserCommandName, userCommandName);
            Assert.Equal(expectedWords, args.Words);
        }

        [Theory]
        [InlineData("!name")]
        [InlineData("!nAmE")]
        [InlineData("!name a")]
        [InlineData("!name a b")]
        public void ShouldIgnoreUser(String input)
        {
            foreach (String id in TwitchUserIDs.ChannelBots)
            {
                Boolean result = Sut.TryValidateCommand(
                    ChatMessageBuilder.Create()
                    .WithTwitchLibMessage(TwitchLibMessageBuilder.Create().WithUserId(id).Build())
                    .WithMessage(input)
                    .Build(),
                    out var userCommandName,
                    out var args);

                Assert.False(result);
                Assert.Null(userCommandName);
                Assert.Null(args);
            }
        }

        [Theory]
        [InlineData("!")]
        [InlineData("! a")]
        [InlineData("a")]
        [InlineData("a a")]
        public void ShouldIgnoreNonCommand(String input)
        {
            Boolean result = Sut.TryValidateCommand(
                ChatMessageBuilder.Create()
                    .WithMessage(input)
                    .Build(),
                out var userCommandName,
                out var args);

            Assert.False(result);
            Assert.Null(userCommandName);
            Assert.Null(args);
        }
    }
}
