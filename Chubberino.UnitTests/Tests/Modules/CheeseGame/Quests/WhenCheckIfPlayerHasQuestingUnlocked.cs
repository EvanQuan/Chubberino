using System;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.Quests;
using Xunit;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Quests
{
    public sealed class WhenCheckIfPlayerHasQuestingUnlocked
    {
        private Player Player { get; }

        public WhenCheckIfPlayerHasQuestingUnlocked()
        {
            Player = new();
        }

        [Theory]
        [InlineData(Int32.MaxValue)]
        [InlineData(1)]
        public void ShouldReturnTrue(Int32 questsUnlocked)
        {
            Player.QuestsUnlockedCount = questsUnlocked;

            var result = Player.HasQuestingUnlocked();

            Assert.True(result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(Int32.MinValue)]
        public void ShouldReturnFalse(Int32 questUnlocked)
        {
            Player.QuestsUnlockedCount = questUnlocked;

            var result = Player.HasQuestingUnlocked();

            Assert.False(result);
        }
    }
}
