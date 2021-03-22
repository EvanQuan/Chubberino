using Chubberino.Modules.CheeseGame.Points;
using Chubberino.Modules.CheeseGame.Rankings;
using Xunit;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Points.CheeseModifierManagers
{
    public sealed class WhenTryGettingNextModifierToUnlock : UsingCheeseModifierManager
    {
        [Fact]
        public void ShouldReturnFirstModifier()
        {
            var result = Sut.TryGetNextModifierToUnlock(Rank.Bronze, out var cheeseModifier);

            Assert.True(result);
            Assert.NotNull(cheeseModifier);
            Assert.Equal(CheeseModifierManager.Modifiers[1], cheeseModifier);
        }

        [Fact]
        public void ShouldReturnLastModifier()
        {
            var result = Sut.TryGetNextModifierToUnlock(Rank.Legend, out var cheeseModifier);

            Assert.True(result);
            Assert.NotNull(cheeseModifier);
            Assert.Equal(CheeseModifierManager.Modifiers[CheeseModifierManager.Modifiers.Count - 1], cheeseModifier);
        }

        [Fact]
        public void ShouldReturnNoModifier()
        {
            var result = Sut.TryGetNextModifierToUnlock(Rank.None, out var cheeseModifier);

            Assert.False(result);
            Assert.Null(cheeseModifier);
        }

    }
}
