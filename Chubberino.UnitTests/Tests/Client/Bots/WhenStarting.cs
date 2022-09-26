using System;
using System.Collections.Generic;
using Chubberino.Infrastructure.Credentials;
using FluentAssertions;
using LanguageExt;
using Moq;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Interfaces;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Bots;

public sealed class WhenStarting : UsingBot
{
    [Fact]
    public void GivenClientInitializesAndJoinsChannels_ThenLoginCredentialsReturned()
    {
        MockedTwitchClientManager
            .Setup(x => x.TryInitializeTwitchClient(
                Sut,
                It.IsAny<IClientOptions>(),
                It.IsAny<LoginCredentials>()))
            .Returns(LoginCredentials);

        MockedTwitchClientManager
            .Setup(x => x.TryJoinInitialChannels(It.IsAny<IReadOnlyList<JoinedChannel>>()))
            .Returns(true);

        var result = Sut.Start();

        result.IsSome.Should().BeTrue();
        result.IfSome(credentials => credentials.Should().Be(LoginCredentials));

    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    public void GivenClientDoesNotInitializeClientAndJoinChannels_ThenNothingIsReturned(
        Boolean successfullyInitializeClient,
        Boolean successfullyJoinChannels)
    {
        MockedTwitchClientManager
            .Setup(x => x.TryInitializeTwitchClient(
                Sut,
                It.IsAny<IClientOptions>(),
                It.IsAny<LoginCredentials>()))
            .Returns(successfullyInitializeClient ? LoginCredentials : Option<LoginCredentials>.None);

        MockedTwitchClientManager
            .Setup(x => x.TryJoinInitialChannels(It.IsAny<IReadOnlyList<JoinedChannel>>()))
            .Returns(successfullyJoinChannels);

        var result = Sut.Start();

        result.IsNone.Should().BeTrue();
    }
}
