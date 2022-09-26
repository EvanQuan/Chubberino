using System;
using System.Collections.Generic;
using System.IO;
using Chubberino.Common.Services;
using Chubberino.Common.ValueObjects;
using Chubberino.Database.Contexts;
using Chubberino.Database.Models;
using Chubberino.Infrastructure.Client;
using Chubberino.Infrastructure.Client.TwitchClients;
using Chubberino.Infrastructure.Commands;
using Chubberino.Infrastructure.Credentials;
using Chubberino.UnitTestQualityTools.Extensions;
using Moq;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Interfaces;

namespace Chubberino.UnitTests.Tests.Client.Bots;

public abstract class UsingBot
{
    protected Bot Sut { get; }

    protected Mock<IApplicationContext> MockedContext { get; }

    protected Mock<ICredentialsManager> MockedCredentialsManager { get; }

    protected ApplicationCredentials ApplicationCredentials { get; }

    protected Mock<TextWriter> MockedWriter { get; }

    protected LoginCredentials LoginCredentials { get; }

    protected String Username { get; }

    protected String TwitchOAuth { get; }

    protected String ChannelName { get; }

    protected Mock<ICommandRepository> MockedCommandRepository { get; }

    protected Mock<ITwitchClientManager> MockedTwitchClientManager { get; }

    protected IRegularClientOptions RegularClientOptions { get; }

    protected IModeratorClientOptions ModeratorClientOptions { get; }

    protected Mock<ITwitchClient> MockedClient { get; }

    protected IList<JoinedChannel> JoinedChannels { get; set; }

    protected Mock<IThreadService> MockedThreadService { get; }

    protected IList<StartupChannel> StartupChannels { get; }

    protected IList<Player> Players { get; }

    protected Name PrimaryChannelName { get; }

    protected String CommandStatus { get; }

    public UsingBot()
    {
        StartupChannels = new List<StartupChannel>
        {
            new StartupChannel()
            {
                ID = 1,
                UserID = "1",
                DisplayName = "a"
            }
        };

        Players = new List<Player>
        {
            new Player()
            {
                ID = 1,
                TwitchUserID = "1",
                Name = "a"
            }
        };

        PrimaryChannelName = Name.From("p");
        CommandStatus = "s";

        MockedContext = new Mock<IApplicationContext>();

        MockedCredentialsManager = new Mock<ICredentialsManager>();

        MockedContext.Setup(x => x.StartupChannels).Returns(() => StartupChannels.ToDbSet());
        MockedContext.Setup(x => x.Players).Returns(() => Players.ToDbSet());

        Username = Guid.NewGuid().ToString();
        TwitchOAuth = Guid.NewGuid().ToString();
        ChannelName = Guid.NewGuid().ToString();

        ApplicationCredentials = new ApplicationCredentials()
        {
            ID = 1,
            InitialTwitchPrimaryChannelName = ChannelName,
            TwitchAPIClientID = Guid.NewGuid().ToString(),
            WolframAlphaAppID = Guid.NewGuid().ToString()
        };

        MockedWriter = new();

        JoinedChannels = new List<JoinedChannel>();

        LoginCredentials = new(new(Username, TwitchOAuth, disableUsernameCheck: true), true, PrimaryChannelName);

        MockedCredentialsManager
            .Setup(x => x.TryUpdateLoginCredentials(It.IsAny<LoginCredentials>()))
            .Returns(LoginCredentials);

        MockedCommandRepository = new Mock<ICommandRepository>().SetupAllProperties();

        MockedCommandRepository.Setup(x => x.GetStatus()).Returns(CommandStatus);

        MockedTwitchClientManager = new Mock<ITwitchClientManager>().SetupAllProperties();

        MockedTwitchClientManager
            .Setup(x => x.TryInitializeTwitchClient(
                It.IsAny<IBot>(),
                It.IsAny<IClientOptions>(),
                It.IsAny<LoginCredentials>()))
            .Returns(LoginCredentials);
        MockedTwitchClientManager
            .Setup(x => x.TryJoinInitialChannels(It.IsAny<IReadOnlyList<JoinedChannel>>()))
            .Returns(true);

        MockedClient = new Mock<ITwitchClient>();

        RegularClientOptions = new RegularClientOptions();

        ModeratorClientOptions = new ModeratorClientOptions();

        MockedTwitchClientManager
            .Setup(x => x.Client)
            .Returns(MockedClient.Object);

        MockedClient
            .Setup(x => x.Connect())
            .Callback(() =>
            {
                MockedClient
                    .Setup(x => x.IsConnected)
                    .Returns(true);
            });

        MockedClient
            .Setup(x => x.JoinedChannels)
            .Returns((IReadOnlyList<JoinedChannel>)JoinedChannels);

        MockedClient
            .Setup(x => x.JoinChannel(It.IsAny<String>(), It.IsAny<Boolean>()))
            .Callback((String channel, Boolean overrideCheck) =>
            {
                JoinedChannels.Add(new JoinedChannel(channel));
                if (MockedTwitchClientManager.Object.PrimaryChannelName == null)
                {
                    MockedTwitchClientManager.Object.PrimaryChannelName = channel;
                }
            });

        MockedTwitchClientManager
            .Setup(x => x.EnsureJoinedToChannel(It.IsAny<String>()))
            .Callback((String channel) =>
            {
                MockedClient.Object.JoinChannel(channel);
                MockedClient.Setup(x => x.IsConnected).Returns(true);
            })
            .Returns(true);

        Sut = new(
            MockedWriter.Object,
            MockedCommandRepository.Object,
            ModeratorClientOptions,
            RegularClientOptions,
            MockedTwitchClientManager.Object)
        {
            LoginCredentials = LoginCredentials
        };

        MockedTwitchClientManager.Invocations.Clear();
        MockedCommandRepository.Invocations.Clear();
        MockedClient.Invocations.Clear();
    }
}
