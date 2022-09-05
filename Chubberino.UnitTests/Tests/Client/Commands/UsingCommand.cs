using System;
using System.IO;
using Chubberino.Bots.Common.Commands;
using Chubberino.Database.Contexts;
using Chubberino.Infrastructure.Client;
using Chubberino.Infrastructure.Client.TwitchClients;
using Chubberino.Infrastructure.Commands;
using Moq;
using TwitchLib.Client.Interfaces;
using TwitchLib.Communication.Models;

namespace Chubberino.UnitTests.Tests.Client.Commands;

public abstract class UsingCommand
{
    protected Mock<IApplicationContextFactory> MockedContextFactory { get; }

    protected Mock<IApplicationContext> MockedContext { get; }

    protected Mock<ITwitchClientManager> MockedTwitchClientManager { get; }

    protected Mock<ITwitchClient> MockedTwitchClient { get; }

    protected Mock<IRepeater> MockedRepeater { get; }

    protected Mock<TextWriter> MockedWriter { get; }

    protected Mock<ICommandRepository> MockedCommandRepository { get; }

    protected Mock<IBot> MockedBot { get; }

    protected String PrimaryChannelName { get; }

    public UsingCommand()
    {
        PrimaryChannelName = Guid.NewGuid().ToString();

        MockedBot = new Mock<IBot>().SetupAllProperties();

        var moderatorOptions =  new ClientOptions();
        var regularOptions = new ClientOptions();

        MockedWriter = new();

        MockedContextFactory = new();

        MockedContext = new Mock<IApplicationContext>();

        MockedContextFactory.Setup(x => x.GetContext()).Returns(MockedContext.Object);

        MockedTwitchClient = new Mock<ITwitchClient>();

        MockedTwitchClientManager = new Mock<ITwitchClientManager>();

        MockedTwitchClientManager.Setup(x => x.PrimaryChannelName).Returns(PrimaryChannelName);

        MockedTwitchClientManager.Setup(x => x.Client).Returns(MockedTwitchClient.Object);

        MockedRepeater = new Mock<IRepeater>().SetupAllProperties();

        MockedCommandRepository = new Mock<ICommandRepository>().SetupAllProperties();
    }

}
