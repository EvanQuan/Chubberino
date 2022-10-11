using System.Collections.Generic;
using Chubberino.Bots.Channel.Modules.CheeseGame.Items.Recipes;
using Chubberino.Database.Models;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Items.Recipes;

public sealed class When_trying_to_get_the_next_cheese_type_to_unlock
{
    public static readonly IReadOnlyList<RecipeInfo> Recipes = RecipeRepository.Recipes;

    public sealed class Given_a_new_player
    {
        [Fact]
        public void Then_second_cheese_is_returned()
        {
            var player = new Player();
            var result = Recipes.TryGetNextToUnlock(player);
            var secondCheese = Recipes[1];

            result.IsSome.Should().BeTrue();
            result.IfSome(cheese =>
                cheese.Should().Be(secondCheese,
                    "because the first second is unlocked by default"));
        }
    }

    public sealed class Given_next_cheese_is_last
    {
        [Fact]
        public void Then_last_cheese_is_returned()
        {
            var player = new Player
            {
                CheeseUnlocked = Recipes.Count - 2
            };
            var lastCheese = Recipes[Recipes.Count - 1];

            var result = Recipes.TryGetNextToUnlock(player);

            result.IsSome.Should().BeTrue();
            result.IfSome(cheese =>
                cheese.Should().Be(lastCheese));
        }
    }

    public sealed class Given_next_cheese_is_out_of_range
    {
        [Fact]
        public void Then_no_cheese_is_returned()
        {
            var player = new Player
            {
                CheeseUnlocked = Recipes.Count
            };

            var result = Recipes.TryGetNextToUnlock(player);

            result.IsNone.Should().BeTrue();
        }
    }
}
