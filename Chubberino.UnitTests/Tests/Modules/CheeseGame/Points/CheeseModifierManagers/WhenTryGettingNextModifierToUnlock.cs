using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.Points;
using Chubberino.Modules.CheeseGame.Rankings;
using Chubberino.Modules.CheeseGame.Repositories;
using Xunit;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Points.CheeseModifierManagers
{
    public sealed class WhenTryGettingNextModifierToUnlock : UsingCheeseModifierManager
    {
        private Player Player { get; }

        public WhenTryGettingNextModifierToUnlock()
        {
            Player = new Player();
        }
        [Fact]
        public void ShouldReturnFirstModifier()
        {
            Player.NextCheeseModifierUpgradeUnlock = Rank.Bronze;

            var result = Sut.TryGetNextToUnlock(Player, out var cheeseModifier);

            Assert.True(result);
            Assert.NotNull(cheeseModifier);
            Assert.Equal(CheeseModifierRepository.Modifiers[1], cheeseModifier);
        }

        [Fact]
        public void ShouldReturnLastModifier()
        {
            Player.NextCheeseModifierUpgradeUnlock = Rank.Legend;

            var result = Sut.TryGetNextToUnlock(Player, out var cheeseModifier);

            Assert.True(result);
            Assert.NotNull(cheeseModifier);
            Assert.Equal(CheeseModifierRepository.Modifiers[CheeseModifierRepository.Modifiers.Count - 1], cheeseModifier);
        }

        [Fact]
        public void ShouldReturnNoModifier()
        {
            Player.NextCheeseModifierUpgradeUnlock = Rank.None;

            var result = Sut.TryGetNextToUnlock(Player, out var cheeseModifier);

            Assert.False(result);
            Assert.Null(cheeseModifier);
        }

    }
}
