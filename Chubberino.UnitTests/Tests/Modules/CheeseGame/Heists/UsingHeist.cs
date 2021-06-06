using Chubberino.Client;
using Chubberino.Database.Contexts;
using Chubberino.Modules.CheeseGame.Heists;
using Chubberino.Modules.CheeseGame.Models;
using Moq;
using System;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;
using TwitchLib.Client.Models.Builders;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Heists
{
    public abstract class UsingHeist
    {
        protected Heist Sut { get; }

        protected ChatMessage ChatMessage { get; }

        protected Mock<IApplicationContext> MockedContext { get; }

        protected Mock<Random> MockedRandom { get; }

        protected Mock<ITwitchClientManager> MockedTwitchClientManager { get; }

        protected Mock<ITwitchClient> MockedTwitchClient { get; }

        protected Player Player { get; }

        public UsingHeist()
        {
            ChatMessage = ChatMessageBuilder.Create()
                .WithChannel(Guid.NewGuid().ToString())
                .Build();

            MockedContext = new Mock<IApplicationContext>();

            MockedRandom = new Mock<Random>();

            MockedTwitchClientManager = new Mock<ITwitchClientManager>();

            MockedTwitchClient = new Mock<ITwitchClient>();

            MockedTwitchClientManager.Setup(x => x.Client).Returns(MockedTwitchClient.Object);

            Player = new Player()
            {
                TwitchUserID = Guid.NewGuid().ToString()
            };

            Sut = new Heist(ChatMessage, MockedRandom.Object, MockedTwitchClientManager.Object);
        }
    }
}
