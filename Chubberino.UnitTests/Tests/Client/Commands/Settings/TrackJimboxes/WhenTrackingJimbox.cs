﻿using System.Collections.Generic;
using Chubberino.Bots.Common.Commands.Settings;
using Chubberino.Infrastructure.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models.Builders;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.TrackJimboxes;

public sealed class WhenTrackingJimbox : UsingCommand
{
    private TrackJimbox Sut { get; }

    public WhenTrackingJimbox()
    {
        Sut = new TrackJimbox(MockedTwitchClientManager.Object, MockedWriter.Object);
    }

    [Theory]
    [MemberData(nameof(ValidJimboxes))]
    public void ShouldDetectJimbox(
        (String Username, String Message)[] messages,
        String[] expectedContributors,
        String expectedBorder)
    {
        MockedTwitchClientManager
            .Setup(x => x.SpoolMessage(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<Priority>()))
            .Callback((String channelName, String message, Priority priority) =>
            {
                Assert.Equal(PrimaryChannelName, channelName);

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

        MockedTwitchClientManager.Verify(x => x.SpoolMessage(PrimaryChannelName, It.IsAny<String>(), Priority.Medium), Times.Once());
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
