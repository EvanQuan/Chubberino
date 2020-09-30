using Chubberino.Client;
using Chubberino.Client.Commands.Settings;
using Moq;
using System;
using System.Collections.Generic;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models.Builders;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.TrackJimboxes
{
    public sealed class WhenTrackingJimbox : UsingCommand
    {
        private TrackJimbox Sut { get; }

        public WhenTrackingJimbox()
        {
            Sut = new TrackJimbox(MockedTwitchClient.Object, MockedConsole.Object);
        }

        [Theory]
        [MemberData(nameof(ValidJimboxes))]
        public void ShouldDetectJimbox(
            (String Username, String Message)[] messages,
            String[] expectedContributors,
            String expectedBorder)
        {
            MockedTwitchClient
                .Setup(x => x.SpoolMessage(It.IsAny<String>()))
                .Callback((String message) =>
                {
                    Assert.Contains(expectedBorder, message);

                    foreach (String contributor in expectedContributors)
                    {
                        Assert.Contains($"@{contributor}", message);
                    }

                    Assert.Contains("jimbox", message);
                    Assert.Contains("peepoClap", message);
                });

            foreach ((String username, String message) in messages)
            {

                Sut.TwitchClient_OnMessageReceived(null, new OnMessageReceivedArgs()
                {
                    ChatMessage = ChatMessageBuilder.Create()
                        .WithTwitchLibMessage(TwitchLibMessageBuilder.Create().WithDisplayName(username).Build())
                        .WithMessage(message)
                        .Build()
                });
            }

            MockedTwitchClient.Verify(x => x.SpoolMessage(It.IsAny<String>()), Times.Once());
        }

        public static IEnumerable<Object[]> ValidJimboxes { get; } = new List<Object[]>
        {
            // One contributor
            new Object[]
            {
                new (String Username, String Message)[]
                {
                    ("a", "x x x x"),
                    ("a", "x yyj1 yyj2 x"),
                    ("a", "x yyj3 yyj4 x"),
                    ("a", "x x x x"),
                },
                new String[] { "a" },
                "x"
            },
            // One contributor with invisible characters
            new Object[]
            {
                new (String Username, String Message)[]
                {
                    ("a", "x x x x " + Data.InvisibleCharacter),
                    ("a", "x yyj1 yyj2 x"),
                    ("a", "x yyj3 yyj4 x"),
                    ("a", "x x x x"),
                },
                new String[] { "a" },
                "x"
            },
            // One contributor with invisible characters
            new Object[]
            {
                new (String Username, String Message)[]
                {
                    ("a", "x x x x"),
                    ("a", "x yyj1 yyj2 x " + Data.InvisibleCharacter),
                    ("a", "x yyj3 yyj4 x"),
                    ("a", "x x x x"),
                },
                new String[] { "a" },
                "x"
            },
            // One contributor with invisible characters
            new Object[]
            {
                new (String Username, String Message)[]
                {
                    ("a", "x x x x"),
                    ("a", "x yyj1 yyj2 x"),
                    ("a", "x yyj3 yyj4 x " + Data.InvisibleCharacter),
                    ("a", "x x x x"),
                },
                new String[] { "a" },
                "x"
            },
            // One contributor with invisible characters
            new Object[]
            {
                new (String Username, String Message)[]
                {
                    ("a", "x x x x"),
                    ("a", "x yyj1 yyj2 x"),
                    ("a", "x yyj3 yyj4 x "),
                    ("a", "x x x x " + Data.InvisibleCharacter),
                },
                new String[] { "a" },
                "x"
            },
            // Multiple contributors
            new Object[]
            {
                new (String Username, String Message)[]
                {
                    ("a", "x x x x"),
                    ("b", "x yyj1 yyj2 x"),
                    ("c", "x yyj3 yyj4 x"),
                    ("d", "x x x x"),
                },
                new String[] { "a", "b", "c", "d" },
                "x"
            },

        };
    }
}
