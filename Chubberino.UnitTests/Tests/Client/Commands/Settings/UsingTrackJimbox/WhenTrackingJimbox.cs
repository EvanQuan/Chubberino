using Chubberino.Client;
using Chubberino.Client.Abstractions;
using Chubberino.Client.Commands.Settings;
using Chubberino.UnitTests.Utilities.TwitchLib;
using Moq;
using System;
using System.Collections.Generic;
using TwitchLib.Client.Interfaces;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.UsingTrackJimbox
{
    public sealed class WhenTrackingJimbox
    {
        private TrackJimbox Sut { get; }

        private Mock<IExtendedClient> TwitchClient { get; }

        public WhenTrackingJimbox()
        {
            TwitchClient = new Mock<IExtendedClient>();

            Sut = new TrackJimbox(TwitchClient.Object);
        }

        [Theory]
        [MemberData(nameof(ValidJimboxes))]
        public void ShouldDetectJimbox(
            (String Username, String Message)[] messages,
            String[] expectedContributors,
            String expectedBorder)
        {
            TwitchClient
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
                Sut.TwitchClient_OnMessageReceived(null, TwitchLibUtilities.GetOnMessageReceivedArgs(username, message));
            }

            TwitchClient.Verify(x => x.SpoolMessage(It.IsAny<String>()), Times.Once());
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
