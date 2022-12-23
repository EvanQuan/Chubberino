using System.Linq;
using Chubberino.Bots.Channel.Modules.CheeseGame.Items.Upgrades.RecipeModifiers;
using Chubberino.Database.Models;
using LanguageExt;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Items.Upgrades.RecipeModifiers;

public sealed class When_trying_to_get_the_next_modifier_to_unlock
{
    public static readonly Option<RecipeModifier>[] Repository = RecipeModifierRepository.Modifiers;

    public sealed class Given_next_modifier_is_the_first_rank
    {
        [Fact]
        public void Then_the_first_modifier_is_returned()
        {
            var player = new Player
            {
                NextCheeseModifierUpgradeUnlock = Rank.Bronze
            };

            var result = Repository.TryGetNextToUnlock(player);

            // Skip index 0, which is no modifier
            var first = Repository[1]
                    .Some(x => x)
                    .None(default(RecipeModifier));

            result.IsSome.Should().BeTrue();
            result.IfSome(cheeseModifier =>
                cheeseModifier.Should().Be(first));
        }
    }

    public sealed class Given_next_modifier_is_the_last_rank
    {
        [Fact]
        public void Then_the_last_modifier_is_returned()
        {
            var player = new Player
            {
                NextCheeseModifierUpgradeUnlock = Rank.Legend
            };

            var result = Repository.TryGetNextToUnlock(player);

            var last = Repository
                .Last()
                .Some(x => x)
                .None(default(RecipeModifier));

            result.IsSome.Should().BeTrue();
            result.IfSome(cheeseModifier =>
                cheeseModifier.Should().Be(last));
        }
    }

    public sealed class Given_next_modifier_is_none_or_invalid
    {
        [Theory]
        [InlineData(Rank.None)]
        [InlineData((Rank)(-1))]
        public void Then_no_modifier_is_returned(Rank rank)
        {
            var player = new Player
            {
                NextCheeseModifierUpgradeUnlock = rank
            };

            var result = Repository.TryGetNextToUnlock(player);

            result.IsNone.Should().BeTrue();
        }
    }

}
