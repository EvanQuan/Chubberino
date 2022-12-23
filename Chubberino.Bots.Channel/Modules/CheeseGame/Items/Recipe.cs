using System.Collections.Generic;
using Chubberino.Bots.Channel.Modules.CheeseGame.Items.Recipes;
using Chubberino.Bots.Channel.Modules.CheeseGame.Points;
using Chubberino.Common.Extensions;
using Chubberino.Database.Models;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Items;

public sealed class Recipe : Item
{
    /// <summary>
    /// Failure to buy recipe due to it being gated behind the next rank message.
    /// 0 - Rank to unlock
    /// 1 - Name of recipe.
    /// </summary>
    public const String NeedToRankUpMessage = "You must rankup to {0} rank before you can buy the {1} recipe.";

    /// <summary>
    /// There are no recipes available to buy right now. All recipes have
    /// already been purchased.
    /// </summary>
    public const String NoRecipeForSaleMessage = "You have already purchased every recipe.";

    public override IEnumerable<String> Names { get; } = new String[]
    {
        "Recipe",
        "r",
        "recipes"
    };

    public IReadOnlyList<RecipeInfo> RecipeRepository { get; }

    public Recipe(IReadOnlyList<RecipeInfo> recipeRepository)
    {
        RecipeRepository = recipeRepository;
    }


    public override String GetSpecificNameForNotEnoughToBuy(Player player)
        => RecipeRepository
            .TryGetNextToUnlock(player)
            .Some(cheese => $"the {cheese.Name} recipe")
            .None(NoRecipeForSaleMessage);

    public override String GetSpecificNameForSuccessfulBuy(Player player, Int32 quantity)
    {
        var temporaryPlayer = new Player()
        {
            CheeseUnlocked = player.CheeseUnlocked - quantity
        };

        List<String> cheeseNamesPurchased = new();

        for (Int32 i = 0; i < quantity; i++, temporaryPlayer.CheeseUnlocked++)
        {
            RecipeRepository
                .TryGetNextToUnlock(temporaryPlayer)
                .IfSome(cheese => cheeseNamesPurchased.Add(cheese.Name));
        }

        return $"the {String.Join(", ", cheeseNamesPurchased)} recipe{(quantity == 1 ? String.Empty : "s")}";
    }

    public override Either<Int32, String> TryBuySingleUnit(Player player, Int32 price)
        => RecipeRepository
            .TryGetNextToUnlock(player)
            .Some(nextCheeseToUnlock =>
            {
                player.CheeseUnlocked++;
                if (nextCheeseToUnlock.UnlocksNegativeCheese)
                {
                    // Increment again so that the next cheese to unlock is not a negative one.
                    player.CheeseUnlocked++;
                }

                player.Points -= nextCheeseToUnlock.CostToUnlock;

                return Either<Int32, String>.Left(1);
            })
            .None(NoRecipeForSaleMessage);

    public override Option<String> IsForSale(Player player)
        => RecipeRepository
            .TryGetNextToUnlock(player)
            .Some(cheese =>
            {
                if (cheese.RankToUnlock > player.Rank)
                {
                    return String.Format(NeedToRankUpMessage, cheese.RankToUnlock, cheese.Name);
                }
                return Option<String>.None;
            })
            .None(NoRecipeForSaleMessage);

    public override Either<Int32, String> GetPrice(Player player)
        => RecipeRepository
            .TryGetNextToUnlock(player)
            .Some(cheese => Either<Int32, String>.Left(cheese.CostToUnlock))
            .None(NoRecipeForSaleMessage);

    public override Option<String> GetShopPrompt(Player player)
        => RecipeRepository
            .TryGetNextToUnlock(player)
            .Bind(nextCheeseToUnlock =>
            {
                var cheesePoints = player.GetModifiedPoints(nextCheeseToUnlock.Points);

                String recipePrompt = nextCheeseToUnlock.RankToUnlock > player.Rank
                    ? $"{nextCheeseToUnlock.Name} (+{cheesePoints})] unlocked at {player.Rank.Next()} rank"
                    : $"{nextCheeseToUnlock.Name} (+{cheesePoints})] for {nextCheeseToUnlock.CostToUnlock} cheese";

                return Option<String>.Some($"{GetBaseShopPrompt(player)} [{recipePrompt}");
            });
}
