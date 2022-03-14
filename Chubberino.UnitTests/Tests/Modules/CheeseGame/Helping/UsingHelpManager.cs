using System;
using System.Collections.Generic;
using Chubberino.Database.Contexts;
using Chubberino.Infrastructure.Client.TwitchClients;
using Chubberino.Modules.CheeseGame.Helping;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.UnitTestQualityTools.Extensions;
using Moq;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Helping
{
    public abstract class UsingHelpManager
    {
        protected Mock<IApplicationContextFactory> MockedContextFactory { get; }

        protected Mock<IApplicationContext> MockedContext { get; }

        protected Mock<ITwitchClientManager> MockedClient { get; }

        protected HelpManager Sut { get; }

        protected List<Player> Players { get; }

        protected String ChannelName { get; }

        protected UsingHelpManager()
        {
            MockedContextFactory = new();
            MockedContext = MockedContextFactory.SetupContext();
            MockedClient = new();
            Sut = new(MockedContextFactory.Object, MockedClient.Object);

            Players = new()
            {
                new Player()
                {
                    Name = "a",

                }
            };

            MockedContext.Setup(x => x.Players).Returns(Players.ToDbSet());

            ChannelName = "b";
        }
    }
}
