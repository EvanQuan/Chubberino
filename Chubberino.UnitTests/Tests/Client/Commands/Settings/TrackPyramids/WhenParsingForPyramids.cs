using Chubberino.Client;
using Chubberino.Client.Commands.Settings;
using Chubberino.UnitTests.Tests.Client.Commands;
using Moq;
using System;
using System.Collections.Generic;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Client.Models.Builders;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.UsingTrackPyramids
{
    public sealed class WhenParsingForPyramids : UsingCommand
    {
        private TrackPyramids Sut { get; }

        public WhenParsingForPyramids()
        {
            Sut = new TrackPyramids(MockedTwitchClientManager.Object, MockedConsole.Object);
        }

        [Theory]
        [MemberData(nameof(ValidPyramids))]
        public void ShouldDetectPyramid(
            (String displayName, String message)[] messages,
            String[] expectedContributors,
            String expectedPyramidBlock,
            Int32 expectedPyramidHeight)
        {
            MockedTwitchClientManager
                .Setup(x => x.SpoolMessage(It.IsAny<String>()))
                .Callback((String message) =>
                {
                    Assert.Contains(expectedPyramidHeight.ToString(), message);
                    Assert.Contains(expectedPyramidBlock.ToString(), message);

                    foreach (String contributor in expectedContributors)
                    {
                        Assert.Contains($"@{contributor}", message);
                    }

                    Assert.Contains("peepoClap", message);
                });

            foreach ((String displayName, String message) in messages)
            {
                Sut.TwitchClient_OnMessageReceived(null, new OnMessageReceivedArgs()
                {
                    ChatMessage = ChatMessageBuilder
                    .Create()
                    .WithTwitchLibMessage(TwitchLibMessageBuilder
                        .Create()
                        .WithDisplayName(displayName)
                        .Build())
                    .WithMessage(message)
                    .Build()
                });
            }

            Assert.Equal(expectedPyramidBlock, Sut.Pyramid.Block);

            MockedTwitchClient.Verify(x => x.SpoolMessage(It.IsAny<String>(), It.IsAny<String>()), Times.Once());
        }

        public static IEnumerable<Object[]> ValidPyramids { get; } = new List<Object[]>
        {
            // 3 height
            new Object[]
            {
                new (String displayName, String Message)[]
                {
                    ("a", "x"),
                    ("a", "x x"),
                    ("a", "x x x"),
                    ("a", "x x"),
                    ("a", "x"),
                },
                new String[] { "a" },
                "x",
                3
            },
            // 3 height with invisible characters
            new Object[]
            {
                new (String displayName, String Message)[]
                {
                    ("a", "x " + Data.InvisibleCharacter),
                    ("a", "x x"),
                    ("a", "x x x"),
                    ("a", "x x"),
                    ("a", "x"),
                },
                new String[] { "a" },
                "x",
                3
            },
            // 3 height with invisible characters
            new Object[]
            {
                new (String displayName, String Message)[]
                {
                    ("a", "x"),
                    ("a", "x x " + Data.InvisibleCharacter),
                    ("a", "x x x"),
                    ("a", "x x"),
                    ("a", "x"),
                },
                new String[] { "a" },
                "x",
                3
            },
            // 3 height with invisible characters
            new Object[]
            {
                new (String displayName, String Message)[]
                {
                    ("a", "x"),
                    ("a", "x x"),
                    ("a", "x x x " + Data.InvisibleCharacter),
                    ("a", "x x"),
                    ("a", "x"),
                },
                new String[] { "a" },
                "x",
                3
            },
            // 3 height with invisible characters
            new Object[]
            {
                new (String displayName, String Message)[]
                {
                    ("a", "x"),
                    ("a", "x x"),
                    ("a", "x x x"),
                    ("a", "x x " + Data.InvisibleCharacter),
                    ("a", "x"),
                },
                new String[] { "a" },
                "x",
                3
            },
            // 3 height with invisible characters
            new Object[]
            {
                new (String displayName, String Message)[]
                {
                    ("a", "x"),
                    ("a", "x x"),
                    ("a", "x x x"),
                    ("a", "x x"),
                    ("a", "x " + Data.InvisibleCharacter),
                },
                new String[] { "a" },
                "x",
                3
            },
            // 3 height with whitespace
            new Object[]
            {
                new (String displayName, String message)[]
                {
                    ("a", "x "),
                    ("a", "x  x"),
                    ("a", "x  x x"),
                    ("a", "x x "),
                    ("a", "x"),
                },
                new String[] { "a" },
                "x",
                3
            },
            // 4 height
            new Object[]
            {
                new (String displayName, String message)[]
                {
                    ("a", "x"),
                    ("a", "x x"),
                    ("a", "x x x"),
                    ("a", "x x x x"),
                    ("a", "x x x"),
                    ("a", "x x"),
                    ("a", "x"),
                },
                new String[] { "a" },
                "x",
                4
            },
        };

        [Theory]
        [MemberData(nameof(RuinedPyramids))]
        public void ShouldDetectRuinedPyramid(
            (String displayName, String message)[] messages,
            String[] expectedContributors,
            String expectedPyramidBlock,
            Int32 expectedPyramidHeight)
        {
            MockedTwitchClientManager.Setup(x => x.SpoolMessage(It.IsAny<String>()))
                .Callback((String message) =>
                {
                    Assert.Contains(expectedPyramidHeight.ToString(), message);
                    Assert.Contains(expectedPyramidBlock.ToString(), message);

                    foreach (String contributor in expectedContributors)
                    {
                        Assert.Contains($"@{contributor}", message);
                    }

                    Assert.Contains("ruined", message);
                });

            foreach ((String displayName, String message) in messages)
            {
                ChatMessage chatMessage = ChatMessageBuilder
                    .Create()
                    .WithTwitchLibMessage(TwitchLibMessageBuilder
                        .Create()
                        .WithDisplayName(displayName))
                    .WithMessage(message)
                    .Build();

                Sut.TwitchClient_OnMessageReceived(null, new OnMessageReceivedArgs()
                {
                    ChatMessage = chatMessage
                });
            }

            Assert.Equal(expectedPyramidBlock, Sut.Pyramid.Block);

            MockedTwitchClientManager.Verify(x => x.SpoolMessage(It.IsAny<String>()), Times.Once());
        }

        public static IEnumerable<Object[]> RuinedPyramids { get; } = new List<Object[]>
        {
            // 3 height
            new Object[]
            {
                new (String displayName, String message)[]
                {
                    ("a", "x"),
                    ("a", "x x"),
                    ("a", "x x x"),
                    ("a", "x"),
                },
                new String[] { "a" },
                "x",
                3
            },
        };

        /// <summary>
        /// Invalid pyramids should not be detected. The last starting pyramid
        /// block detected should be considered as starting a new pyramid.
        /// </summary>
        /// <param name="messages">Display name and message pairs for all messages received.</param>
        /// <param name="expectedContributors">Expected pyramid contributors.</param>
        /// <param name="expectedPyramidBlock">Expected pyramid block.</param>
        /// <param name="expectedPyramidHeight">Expected tallest pyramid height.</param>
        [Theory]
        [MemberData(nameof(InvalidPyramids))]
        public void ShouldNotDetectPyramid(
            (String displayName, String message)[] messages,
            String[] expectedContributors,
            String expectedPyramidBlock,
            Int32 expectedPyramidHeight)
        {
            foreach ((String displayName, String message) in messages)
            {
                var chatMessage = ChatMessageBuilder
                    .Create()
                    .WithTwitchLibMessage(TwitchLibMessageBuilder
                        .Create()
                        .WithDisplayName(displayName)
                        .Build())
                    .WithMessage(message)
                    .Build();

                Sut.TwitchClient_OnMessageReceived(null, new OnMessageReceivedArgs()
                {
                    ChatMessage = chatMessage
                });
            }

            Assert.Equal(expectedPyramidBlock, Sut.Pyramid.Block);
            Assert.Equal(expectedContributors, Sut.Pyramid.ContributorDisplayNames);
            Assert.Equal(expectedPyramidHeight, Sut.Pyramid.TallestHeight);

            MockedTwitchClientManager.Verify(x => x.SpoolMessage(It.IsAny<String>()), Times.Never());
        }

        public static IEnumerable<Object[]> InvalidPyramids { get; } = new List<Object[]>
        {
            // 1 height
            new Object[]
            {
                new (String displayName, String message)[]
                {
                    ("a", "x"),
                },
                new String[] { "a" },
                "x",
                1
            },
            // 2 height
            new Object[]
            {
                new (String displayName, String message)[]
                {
                    ("a", "x"),
                    ("a", "x x"),
                    ("a", "x"),
                },
                new String[] { "a" },
                "x",
                1
            },
            // 2 authors
            new Object[]
            {
                new (String displayName, String message)[]
                {
                    ("b", "x x"),
                    ("a", "x"),
                },
                new String[] { "a", },
                "x",
                1
            },
            // 2 authors
            new Object[]
            {
                new (String displayName, String message)[]
                {
                    ("a", "x"),
                    ("b", "x"),
                },
                new String[] { "b", },
                "x",
                1
            },
            // 2 authors
            new Object[]
            {
                new (String displayName, String message)[]
                {
                    ("a", "x"),
                    ("b", "x x"),
                    ("a", "x"),
                },
                new String[] { "a" },
                "x",
                1
            },
            // 2 authors
            new Object[]
            {
                new (String displayName, String message)[]
                {
                    ("a", "x"),
                    ("a", "x x"),
                    ("b", "x"),
                },
                new String[] { "b", },
                "x",
                1
            },
            // 2 authors
            new Object[]
            {
                new (String displayName, String message)[]
                {
                    ("a", "x"),
                    ("b", "x x"),
                    ("b", "x"),
                },
                new String[] { "b" },
                "x",
                1
            },
            // 3 authors
            new Object[]
            {
                new (String displayName, String message)[]
                {
                    ("a", "x"),
                    ("b", "x x"),
                    ("c", "x"),
                },
                new String[] { "c", },
                "x",
                1
            },
            // 3 height
            new Object[]
            {
                new (String displayName, String message)[]
                {
                    ("a", "x"),
                    ("a", "x x x"),
                    ("a", "x x"),
                    ("a", "x"),
                },
                new String[] { "a" },
                "x",
                1
            },
        };
    }
}
