using Chubberino.Modules.CheeseGame.Items.Upgrades.RecipeModifiers;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.Ranks;
using System.Collections.Generic;
using Xunit;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Items.Upgrades.RecipeModifiers
{
    public sealed class WhenTryGettingNextModifierToUnlock
    {
        public IReadOnlyList<RecipeModifier> Repository = RecipeModifierRepository.Modifiers;

        private Player Player { get; }

        public WhenTryGettingNextModifierToUnlock()
        {
            Player = new Player();
        }

        [Fact]
        public void ShouldReturnFirstModifier()
        {
            Player.NextCheeseModifierUpgradeUnlock = Rank.Bronze;

            var result = Repository.TryGetNextToUnlock(Player, out var cheeseModifier);

            Assert.True(result);
            Assert.NotNull(cheeseModifier);
            Assert.Equal(Repository[1], cheeseModifier);
        }

        [Fact]
        public void ShouldReturnLastModifier()
        {
            Player.NextCheeseModifierUpgradeUnlock = Rank.Legend;

            var result = Repository.TryGetNextToUnlock(Player, out var cheeseModifier);

            Assert.True(result);
            Assert.NotNull(cheeseModifier);
            Assert.Equal(Repository[Repository.Count - 1], cheeseModifier);
        }

        [Fact]
        public void ShouldReturnNoModifier()
        {
            Player.NextCheeseModifierUpgradeUnlock = Rank.None;

            var result = Repository.TryGetNextToUnlock(Player, out var cheeseModifier);

            Assert.False(result);
            Assert.Null(cheeseModifier);
        }

    }
}
