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
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(Int32.MaxValue)]
        public void ShouldNotAddNewInfestationAtBronze(Int32 mouseTrapCount)
        {
            MockedRandom.SetupReturnMaximum();
            MockedRandom.Setup(x => x.NextDouble()).Returns(0);
            Player.Rank = Rank.Bronze;
            Player.MouseTrapCount = mouseTrapCount;

            var outputMessage = Sut.UpdateInfestationStatus(Player);

            Assert.NotNull(outputMessage);
            Assert.Empty(outputMessage);
            Assert.Equal(0, Player.RatCount);
            Assert.Equal(mouseTrapCount, Player.MouseTrapCount);
            MockedApplicationContext.Verify(x => x.SaveChanges(), Times.Never());
        }

        [Theory]
        [InlineData(Rank.Silver, 0)]
        [InlineData(Rank.Gold, 0)]
        [InlineData(Rank.Legend, 0)]
        [InlineData(Rank.Silver, 1)]
        [InlineData(Rank.Gold, 1)]
        [InlineData(Rank.Legend, 1)]
        public void ShouldAddNewInfestationUncontested(Rank rank, Int32 initialRatCount)
        {
            MockedRandom.SetupReturnMaximum();
            MockedRandom.Setup(x => x.NextDouble()).Returns(0);
            Player.Rank = rank;
            Player.RatCount = initialRatCount;
            var expectedRatCount = HazardManager.InfestationMaximums[rank] + initialRatCount;

            var outputMessage = Sut.UpdateInfestationStatus(Player);

            Assert.NotNull(outputMessage);
            Assert.NotEmpty(outputMessage);
            Assert.Equal(expectedRatCount, Player.RatCount);
            Assert.Equal(0, Player.MouseTrapCount);
            MockedApplicationContext.Verify(x => x.SaveChanges(), Times.Once());
        }

        [Theory]
        [InlineData(Rank.Silver, 0, HazardManager.InfestationMaximum.Silver, 0)]
        [InlineData(Rank.Silver, 1, HazardManager.InfestationMaximum.Silver + 1, 0)]
        [InlineData(Rank.Silver, 1, HazardManager.InfestationMaximum.Silver + 2, 1)]
        public void ShouldAddNewInfestationEliminated(
            Rank rank,
            Int32 oldRatCount,
            Int32 initialMouseTrapCount,
            Int32 expectedNewMouseTrapcount)
        {
            MockedRandom.SetupReturnMaximum();
            MockedRandom.Setup(x => x.NextDouble()).Returns(0);
            Player.Rank = rank;
            Player.RatCount = oldRatCount;
            Player.MouseTrapCount = initialMouseTrapCount;

            var outputMessage = Sut.UpdateInfestationStatus(Player);

            Assert.NotNull(outputMessage);
            Assert.NotEmpty(outputMessage);
            Assert.Equal(0, Player.RatCount);
            Assert.Equal(expectedNewMouseTrapcount, Player.MouseTrapCount);
            MockedApplicationContext.Verify(x => x.SaveChanges(), Times.Once());
        }

        [Theory]
        [InlineData(Rank.Silver, 0, 1, HazardManager.InfestationMaximum.Silver - 1)]
        [InlineData(Rank.Silver, 1, 1, HazardManager.InfestationMaximum.Silver)]
        [InlineData(Rank.Silver, 1, HazardManager.InfestationMaximum.Silver, 1)]
        [InlineData(Rank.Silver, 2, HazardManager.InfestationMaximum.Silver + 1, 1)]
        [InlineData(Rank.Silver, 3, HazardManager.InfestationMaximum.Silver + 1, 2)]
        public void ShouldAddNewInfestationContested(
            Rank rank,
            Int32 oldRatCount,
            Int32 initialMouseTrapCount,
            Int32 expectedNewRatCount)
        {
            MockedRandom.SetupReturnMaximum();
            MockedRandom.Setup(x => x.NextDouble()).Returns(0);
            Player.Rank = rank;
            Player.RatCount = oldRatCount;
            Player.MouseTrapCount = initialMouseTrapCount;

            var outputMessage = Sut.UpdateInfestationStatus(Player);

            Assert.NotNull(outputMessage);
            Assert.NotEmpty(outputMessage);
            Assert.Equal(expectedNewRatCount, Player.RatCount);
            Assert.Equal(0, Player.MouseTrapCount);
            MockedApplicationContext.Verify(x => x.SaveChanges(), Times.Once());
        }

        [Theory]
        [InlineData(Rank.Silver, 1)]
        [InlineData(Rank.Silver, Int32.MaxValue)]
        [InlineData(Rank.Gold, 1)]
        [InlineData(Rank.Gold, Int32.MaxValue)]
        [InlineData(Rank.Legend, 1)]
        [InlineData(Rank.Legend, Int32.MaxValue)]
        public void ShouldMaintainOldInfestationUncontested(Rank rank, Int32 expectedRatCount)
        {
            MockedRandom.SetupReturnMaximum();
            Player.Rank = rank;
            Player.RatCount = expectedRatCount;

            var outputMessage = Sut.UpdateInfestationStatus(Player);

            Assert.NotNull(outputMessage);
            Assert.NotEmpty(outputMessage);
            Assert.Equal(expectedRatCount, Player.RatCount);
            Assert.Equal(0, Player.MouseTrapCount);
            MockedApplicationContext.Verify(x => x.SaveChanges(), Times.Never());
        }

        [Theory]
        [InlineData(Rank.Silver, 1, 1, 0)]
        [InlineData(Rank.Silver, 1, 2, 1)]
        [InlineData(Rank.Gold, 1, 1, 0)]
        [InlineData(Rank.Gold, 1, 2, 1)]
        public void ShouldEliminateOldInfestation(
            Rank rank,
            Int32 oldRatCount,
            Int32 initialMouseTrapCount,
            Int32 expectedNewMouseTrapcount)
        {
            MockedRandom.SetupReturnMaximum();
            Player.Rank = rank;
            Player.RatCount = oldRatCount;
            Player.MouseTrapCount = initialMouseTrapCount;

            var outputMessage = Sut.UpdateInfestationStatus(Player);

            Assert.NotNull(outputMessage);
            Assert.NotEmpty(outputMessage);
            Assert.Equal(0, Player.RatCount);
            Assert.Equal(expectedNewMouseTrapcount, Player.MouseTrapCount);
            MockedApplicationContext.Verify(x => x.SaveChanges(), Times.Once());
        }

        [Theory]
        [InlineData(Rank.Silver, HazardManager.InfestationMaximum.Silver, 1, HazardManager.InfestationMaximum.Silver - 1)]
        [InlineData(Rank.Legend, HazardManager.InfestationMaximum.Silver, HazardManager.InfestationMaximum.Silver - 1, 1)]
        [InlineData(Rank.Legend, HazardManager.InfestationMaximum.Silver, 1, HazardManager.InfestationMaximum.Silver - 1)]
        public void ShouldMaintainOldInfestationContested(
            Rank rank,
            Int32 oldRatCount,
            Int32 initialMouseTrapCount,
            Int32 expectedNewRatCount)
        {
            MockedRandom.SetupReturnMaximum();
            Player.Rank = rank;
            Player.RatCount = oldRatCount;
            Player.MouseTrapCount = initialMouseTrapCount;

            var outputMessage = Sut.UpdateInfestationStatus(Player);

            Assert.NotNull(outputMessage);
            Assert.NotEmpty(outputMessage);
            Assert.Equal(expectedNewRatCount, Player.RatCount);
            Assert.Equal(0, Player.MouseTrapCount);
            MockedApplicationContext.Verify(x => x.SaveChanges(), Times.Once());
        }

        [Theory]
        [InlineData(Rank.Bronze, 0)]
        [InlineData(Rank.Bronze, 1)]
        [InlineData(Rank.Silver, 0)]
        [InlineData(Rank.Silver, 1)]
        [InlineData(Rank.Legend, 0)]
        [InlineData(Rank.Legend, 1)]
        public void ShouldDoNothing(Rank rank, Int32 expectedMouseTrapCount)
        {
            MockedRandom.SetupReturnMaximum();
            Player.Rank = rank;
            Player.MouseTrapCount = expectedMouseTrapCount;

            var outputMessage = Sut.UpdateInfestationStatus(Player);

            Assert.NotNull(outputMessage);
            Assert.Empty(outputMessage);
            Assert.Equal(0, Player.RatCount);
            Assert.Equal(expectedMouseTrapCount, Player.MouseTrapCount);
            MockedApplicationContext.Verify(x => x.SaveChanges(), Times.Never());
        }
    }
}
