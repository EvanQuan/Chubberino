using Chubberino.Client;
using Chubberino.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Hazards;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.Rankings;
using Chubberino.UnitTests.Utility;
using Moq;
using System;
using Xunit;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Hazards.HazardManagers
{
    public sealed class WhenUpdatingInfestationStatus
    {
        private HazardManager Sut { get; }

        private Mock<IApplicationContext> MockedApplicationContext { get; }

        private Mock<ITwitchClientManager> MockedTwitchClientManager { get; }

        private Mock<Random> MockedRandom { get; }

        private Mock<IEmoteManager> MockedEmoteManager { get; }

        private Player Player { get; }

        public WhenUpdatingInfestationStatus()
        {
            MockedApplicationContext = new Mock<IApplicationContext>();
            MockedTwitchClientManager = new Mock<ITwitchClientManager>();
            MockedRandom = new Mock<Random>();
            MockedEmoteManager = new Mock<IEmoteManager>();

            Sut = new HazardManager(
                MockedApplicationContext.Object,
                MockedTwitchClientManager.Object,
                MockedRandom.Object,
                MockedEmoteManager.Object);

            Player = new Player();
        }

        [Theory]
        [InlineData(Rank.Silver)]
        [InlineData(Rank.Gold)]
        [InlineData(Rank.Legend)]
        public void ShouldAddNewInfestationUncontested(Rank rank)
        {
            MockedRandom.SetupReturnMaximum();
            MockedRandom.Setup(x => x.NextDouble()).Returns(0);
            Player.Rank = rank;
            var expectedRatCount = HazardManager.InfestationMaximum[rank];

            var outputMessage = Sut.UpdateInfestationStatus(Player);

            Assert.NotNull(outputMessage);
            Assert.Equal(expectedRatCount, Player.MouseCount);
            Assert.Equal(0, Player.MouseTrapCount);
        }
    }
}
