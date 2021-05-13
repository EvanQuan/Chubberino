using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.Quests;
using System.Collections.Generic;
using Xunit;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Quests.QuestRepositories
{
    public sealed class WhenTryGettingNextQuestToUnlock
    {
        public IReadOnlyList<Quest> Sut = QuestRepository.Quests;

        private Player Player { get; }

        public WhenTryGettingNextQuestToUnlock()
        {
            Player = new Player();
        }

        [Fact]
        public void ShouldReturnFirstQuest()
        {
            var result = Sut.TryGetNextToUnlock(Player, out var quest);

            Assert.True(result);
            Assert.NotNull(quest);
            Assert.Equal(Sut[0], quest);
        }

        [Fact]
        public void ShouldReturnLastCheese()
        {
            Player.QuestsUnlockedCount = Sut.Count - 1;
            var result = Sut.TryGetNextToUnlock(Player, out var quest);

            Assert.True(result);
            Assert.NotNull(quest);
            Assert.Equal(Sut[Sut.Count - 1], quest);
        }

        [Fact]
        public void ShouldReturnNoCheese()
        {
            Player.QuestsUnlockedCount = Sut.Count;
            var result = Sut.TryGetNextToUnlock(Player, out var cheeseType);

            Assert.False(result);
            Assert.Null(cheeseType);
        }
    }
}
