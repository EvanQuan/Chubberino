using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.Rankings;
using Chubberino.Modules.CheeseGame.Upgrades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Upgrades
{
    public sealed class WhenGettingNextUpgradeToUnlock
    {
        public Player Player { get; }

        public WhenGettingNextUpgradeToUnlock()
        {
            Player = new();
        }

        [Fact]
        public void ShouldReturnCheeseModifierForNewPlayer()
        {
            UpgradeType result = Player.GetNextUpgradeToUnlock();

            Assert.Equal(UpgradeType.CheeseModifier, result);
        }

        [Fact]
        public void ShouldReturnModifierForAllBronzeUpgradesBought()
        {
            Player.NextCheeseModifierUpgradeUnlock = Rank.Silver;
            Player.NextCriticalCheeseUpgradeUnlock = Rank.Silver;
            Player.NextQuestUpgradeUnlock = Rank.Silver;
            Player.NextWorkerProductionUpgradeUnlock = Rank.Silver;
            Player.NextStorageUpgradeUnlock = Rank.Silver;

            UpgradeType result = Player.GetNextUpgradeToUnlock();

            Assert.Equal(UpgradeType.CheeseModifier, result);
        }

        [Fact]
        public void ShouldReturnWorkerProduction()
        {
            Player.NextCheeseModifierUpgradeUnlock = Rank.Silver;
            Player.NextCriticalCheeseUpgradeUnlock = Rank.Silver;
            Player.NextQuestUpgradeUnlock = Rank.Silver;
            Player.NextWorkerProductionUpgradeUnlock = Rank.Bronze;
            Player.NextStorageUpgradeUnlock = Rank.Bronze;

            UpgradeType result = Player.GetNextUpgradeToUnlock();

            Assert.Equal(UpgradeType.WorkerProduction, result);
        }

        [Fact]
        public void ShouldReturnStorage()
        {
            Player.NextCheeseModifierUpgradeUnlock = Rank.Silver;
            Player.NextCriticalCheeseUpgradeUnlock = Rank.Silver;
            Player.NextQuestUpgradeUnlock = Rank.Silver;
            Player.NextWorkerProductionUpgradeUnlock = Rank.Silver;
            Player.NextStorageUpgradeUnlock = Rank.Bronze;

            UpgradeType result = Player.GetNextUpgradeToUnlock();

            Assert.Equal(UpgradeType.Storage, result);
        }

        [Fact]
        public void ShouldReturnNone()
        {
            Player.NextCheeseModifierUpgradeUnlock = Rank.None;
            Player.NextCriticalCheeseUpgradeUnlock = Rank.None;
            Player.NextQuestUpgradeUnlock = Rank.None;
            Player.NextWorkerProductionUpgradeUnlock = Rank.None;
            Player.NextStorageUpgradeUnlock = Rank.None;

            UpgradeType result = Player.GetNextUpgradeToUnlock();

            Assert.Equal(UpgradeType.None, result);
        }
    }
}
