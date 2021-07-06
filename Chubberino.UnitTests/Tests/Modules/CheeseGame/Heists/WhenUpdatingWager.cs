using Chubberino.Client;
using Chubberino.Infrastructure.Client;
using Chubberino.Modules.CheeseGame.Heists;
using Moq;
using System;
using Xunit;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Heists
{
    public sealed class WhenUpdatingWager : UsingHeist
    {
        /// <summary>
        /// No matter what is wagered, if the player has no cheese, a message
        /// indicating they have no cheese to wager anything should appear.
        /// </summary>
        /// <param name="points"></param>
        [Theory]
        [InlineData(1)]
        [InlineData(0)]
        [InlineData(-1)]
        public void ShouldFailToJoinHeistDueToNoCheese(Int32 points)
        {
            Sut.UpdateWager(MockedContext.Object, Player, p => points);

            MockedTwitchClientManager.Verify(x => x.SpoolMessage(ChatMessage.Channel, It.Is<String>(x => x.Contains(Heist.FailToJoinHeistBecauseNoCheeseMessage)), Priority.Low), Times.Once());
        }

        /// <summary>
        /// Non-positive wagers should respond with message stating the wager
        /// should be positive.
        /// </summary>
        /// <param name="points"></param>
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void ShouldFailToJoinHeistDueToNonPositiveWager(Int32 points)
        {
            Player.Points = 1;
            Sut.UpdateWager(MockedContext.Object, Player, p => points);

            MockedTwitchClientManager.Verify(x => x.SpoolMessage(ChatMessage.Channel, It.Is<String>(x => x.Contains(Heist.FailToJoinHeistMessage)), Priority.Low), Times.Once());
        }

        /// <summary>
        /// If the wager is valid, and the player has not already wagered, a
        /// message indicating the player has joined the heist should appear.
        /// </summary>
        /// <param name="playerStorage"></param>
        /// <param name="pointsWagered"></param>
        /// <param name="playerPoints"></param>
        /// <param name="expectedPointsToWager"></param>
        /// <param name="expectedPlayerPointsAfterWager"></param>
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
            Sut.UpdateWager(MockedContext.Object, Player, p => pointsWagered);

            Assert.Contains(Sut.Wagers, x => x.PlayerTwitchID.Equals(Player.TwitchUserID) && x.WageredPoints == expectedPointsToWager);
            MockedTwitchClientManager.Verify(x => x.SpoolMessage(ChatMessage.Channel, It.Is<String>(x => x.Contains(String.Format(Heist.SucceedToJoinHeistMessage, expectedPointsToWager))), Priority.Medium), Times.Once());
            Assert.Equal(expectedPlayerPointsAfterWager, Player.Points);
        }

        /// <summary>
        /// If the wager is valid, and the player has already wagered, a
        /// message indicating the player has updated their the heist wager
        /// should appear.
        /// </summary>
        /// <param name="playerStorage"></param>
        /// <param name="playerPoints"></param>
        /// <param name="initialPointsWagered"></param>
        /// <param name="newPointsWagered"></param>
        /// <param name="expectedNewPointsWagered"></param>
        /// <param name="expectedPlayerPointsAfterWager"></param>
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

            Sut.UpdateWager(MockedContext.Object, Player, p => newPointsWagered);

            Assert.Contains(Sut.Wagers, x => x.PlayerTwitchID.Equals(Player.TwitchUserID) && x.WageredPoints == expectedNewPointsWagered);
            MockedTwitchClientManager.Verify(x => x.SpoolMessage(ChatMessage.Channel, It.Is<String>(x => x.Contains(String.Format(Heist.SucceedToUpdateHeistMessage, initialPointsWagered, expectedNewPointsWagered))), Priority.Medium), Times.Once());
            Assert.Equal(expectedPlayerPointsAfterWager, Player.Points);
        }

        /// <summary>
        /// When updating a wager to the same value as it was before, 
        /// a message indicating the wager is unchanged should appear.
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
        public void ShouldRespondWithUnchangedWagerMessage(
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

            Sut.UpdateWager(MockedContext.Object, Player, p => newPointsWagered);

            Assert.Contains(Sut.Wagers, x => x.PlayerTwitchID.Equals(Player.TwitchUserID) && x.WageredPoints == expectedNewPointsWagered);
            MockedTwitchClientManager.Verify(x => x.SpoolMessage(ChatMessage.Channel, It.Is<String>(x => x.Contains(String.Format(Heist.WagerIsUnchangedMessage, initialPointsWagered))), Priority.Low), Times.Once());
            Assert.Equal(expectedPlayerPointsAfterWager, Player.Points);
        }

        /// <summary>
        /// If the <paramref name="newPointsWagered"/> is non-positive, any
        /// pre-existing wager is refunded, and the player leaves the heist.
        /// </summary>
        /// <param name="playerStorage"></param>
        /// <param name="playerPoints"></param>
        /// <param name="initialPointsWagered"></param>
        /// <param name="newPointsWagered"></param>
        /// <param name="expectedPlayerPointsAfterWager"></param>
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

            Sut.UpdateWager(MockedContext.Object, Player, p => newPointsWagered);

            Assert.DoesNotContain(Sut.Wagers, x => x.PlayerTwitchID.Equals(Player.TwitchUserID));
            MockedTwitchClientManager.Verify(x => x.SpoolMessage(ChatMessage.Channel, It.Is<String>(x => x.Contains(String.Format(Heist.SucceedToLeaveHeistMessage, initialPointsWagered))), Priority.Medium), Times.Once());
            Assert.Equal(expectedPlayerPointsAfterWager, Player.Points);
        }
    }
}
