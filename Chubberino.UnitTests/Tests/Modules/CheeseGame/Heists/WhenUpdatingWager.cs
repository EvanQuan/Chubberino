using Chubberino.Client;
using Chubberino.Modules.CheeseGame.Heists;
using Moq;
using System;
using Xunit;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Heists
{
    public sealed class WhenUpdatingWager : UsingHeist
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void ShouldFailToJoinHeist(Int32 points)
        {
            Sut.UpdateWager(Player, p => points);

            MockedTwitchClientManager.Verify(x => x.SpoolMessage(ChatMessage.Channel, It.Is<String>(x => x.Contains(Heist.FailToJoinHeistMessage)), Priority.Medium), Times.Once());
        }

        [Theory]
        [InlineData(2, 1, 1, 1, 0)]
        [InlineData(2, 1, 2, 1, 1)]
        [InlineData(2, 2, 1, 1, 0)]
        public void ShouldJoinHeistForFirstTime(
            Int32 playerStorage,
            Int32 pointsWagered,
            Int32 playerPoints,
            Int32 expectedPointsToWager,
            Int32 expectedPlayerPointsAfterWager)
        {
            Player.MaximumPointStorage = playerStorage;
            Player.Points = playerPoints;
            Sut.UpdateWager(Player, p => pointsWagered);

            Assert.Contains(Sut.Wagers, x => x.PlayerTwitchID.Equals(Player.TwitchUserID) && x.WageredPoints == expectedPointsToWager);
            MockedTwitchClientManager.Verify(x => x.SpoolMessage(ChatMessage.Channel, It.Is<String>(x => x.Contains(String.Format(Heist.SucceedToJoinHeistMessage, expectedPointsToWager))), Priority.Medium), Times.Once());
            Assert.Equal(expectedPlayerPointsAfterWager, Player.Points);
        }

        [Theory]
        [InlineData(2, 2, 1, 2, 2, 0)]
        [InlineData(2, 2, 1, 3, 2, 0)]
        [InlineData(2, 2, 2, 1, 1, 1)]
        public void ShouldUpdatePreexistingWager(
            Int32 playerStorage,
            Int32 playerPoints,
            Int32 initialPointsWagered,
            Int32 newPointsWagered,
            Int32 expectedNewPointsWagered,
            Int32 expectedPlayerPointsAfterWager)
        {
            Player.MaximumPointStorage = playerStorage;
            Player.Points = playerPoints;
            Sut.Wagers.Add(new Wager(Player.TwitchUserID, initialPointsWagered));

            Sut.UpdateWager(Player, p => newPointsWagered);

            Assert.Contains(Sut.Wagers, x => x.PlayerTwitchID.Equals(Player.TwitchUserID) && x.WageredPoints == expectedNewPointsWagered);
            MockedTwitchClientManager.Verify(x => x.SpoolMessage(ChatMessage.Channel, It.Is<String>(x => x.Contains(String.Format(Heist.SucceedToUpdateHeistMessage, initialPointsWagered, expectedNewPointsWagered))), Priority.Medium), Times.Once());
            Assert.Equal(expectedPlayerPointsAfterWager, Player.Points);
        }

        /// <summary>
        /// When updating a wager to the same value as it was before, no
        /// message should appear in response.
        /// </summary>
        /// <param name="playerStorage"></param>
        /// <param name="playerPoints"></param>
        /// <param name="initialPointsWagered"></param>
        /// <param name="newPointsWagered"></param>
        /// <param name="expectedNewPointsWagered"></param>
        /// <param name="expectedPlayerPointsAfterWager"></param>
        [Theory]
        [InlineData(2, 2, 2, 2, 2, 0)]
        [InlineData(2, 2, 1, 1, 1, 1)]
        public void ShouldNotRespondToNonChangingWagerUpdate(
            Int32 playerStorage,
            Int32 playerPoints,
            Int32 initialPointsWagered,
            Int32 newPointsWagered,
            Int32 expectedNewPointsWagered,
            Int32 expectedPlayerPointsAfterWager)
        {
            Player.MaximumPointStorage = playerStorage;
            Player.Points = playerPoints;
            Sut.Wagers.Add(new Wager(Player.TwitchUserID, initialPointsWagered));

            Sut.UpdateWager(Player, p => newPointsWagered);

            Assert.Contains(Sut.Wagers, x => x.PlayerTwitchID.Equals(Player.TwitchUserID) && x.WageredPoints == expectedNewPointsWagered);
            MockedTwitchClientManager.Verify(x => x.SpoolMessage(ChatMessage.Channel, It.IsAny<String>(), It.IsAny<Priority>()), Times.Never());
            Assert.Equal(expectedPlayerPointsAfterWager, Player.Points);
        }

        [Theory]
        [InlineData(1, 0, 2, 0, 1)]
        [InlineData(1, 0, 2, -1, 1)]
        [InlineData(1, 0, 1, 0, 1)]
        [InlineData(1, 0, 1, -1, 1)]
        public void ShouldLeaveHeist(
            Int32 playerStorage,
            Int32 playerPoints,
            Int32 initialPointsWagered,
            Int32 newPointsWagered,
            Int32 expectedPlayerPointsAfterWager)
        {
            Player.MaximumPointStorage = playerStorage;
            Player.Points = playerPoints;
            Sut.Wagers.Add(new Wager(Player.TwitchUserID, initialPointsWagered));

            Sut.UpdateWager(Player, p => newPointsWagered);

            Assert.DoesNotContain(Sut.Wagers, x => x.PlayerTwitchID.Equals(Player.TwitchUserID));
            MockedTwitchClientManager.Verify(x => x.SpoolMessage(ChatMessage.Channel, It.Is<String>(x => x.Contains(String.Format(Heist.SucceedToLeaveHeistMessage, initialPointsWagered))), Priority.Medium), Times.Once());
            Assert.Equal(expectedPlayerPointsAfterWager, Player.Points);
        }
    }
}
