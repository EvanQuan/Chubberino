using System;
using System.Collections.Generic;
using Chubberino.Common.Extensions;
using Chubberino.Modules.CheeseGame.Items.Recipes;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.Points;
using Monad;

namespace Chubberino.Modules.CheeseGame.Items
{
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
        public const String NoRecipeForSaleMessage = "There is no recipe for sale right now.";

        public override IEnumerable<String> Names => new String[] { "Recipe", "r", "recipes" };

        public IReadOnlyList<RecipeInfo> RecipeRepository { get; }

        public Recipe(IReadOnlyList<RecipeInfo> recipeRepository)
        {
            RecipeRepository = recipeRepository;
        }


        public override String GetSpecificNameForNotEnoughToBuy(Player player)
        {
            if (RecipeRepository.TryGetNextToUnlock(player, out var cheese))
            {
                return $"the {cheese.Name} recipe";
            }

            return UnexpectedErrorMessage;
        }

        public override String GetSpecificNameForSuccessfulBuy(Player player, Int32 quantity)
        {
            var temporaryPlayer = new Player()
            {
                CheeseUnlocked = player.CheeseUnlocked - quantity
            };

            List<String> cheeseNamesPurchased = new();

            for (Int32 i = 0; i < quantity; i++, temporaryPlayer.CheeseUnlocked++)
            {
                RecipeRepository.TryGetNextToUnlock(temporaryPlayer, out var cheese);

                cheeseNamesPurchased.Add(cheese.Name);
            }

            return $"the {String.Join(", ", cheeseNamesPurchased)} recipe{(quantity == 1 ? String.Empty : "s")}";
        }

        public override Either<Int32, String> TryBuySingleUnit(Player player, Int32 price)
        {
            if (!RecipeRepository.TryGetNextToUnlock(player, out var nextCheeseToUnlock))
            {
                return () => UnexpectedErrorMessage;
            }

            player.CheeseUnlocked++;
            if (nextCheeseToUnlock.UnlocksNegativeCheese)
            {
                // Increment again so that the next cheese to unlock is not a negative one.
                player.CheeseUnlocked++;
            }

            player.Points -= nextCheeseToUnlock.CostToUnlock;

            return () => 1;
        }

        public override Boolean IsForSale(Player player, out String reason)
        {
            if (RecipeRepository.TryGetNextToUnlock(player, out var cheese))
            {
                if (cheese.RankToUnlock > player.Rank)
                {
                    reason = String.Format(NeedToRankUpMessage, cheese.RankToUnlock, cheese.Name);
                    return false;
                }

                reason = default;
                return true;
            }

            reason = NoRecipeForSaleMessage;
            return false;
        }

        public override Int32 GetPrice(Player player)
        {
            if (RecipeRepository.TryGetNextToUnlock(player, out var cheese))
            {
                return cheese.CostToUnlock;
            }

            return Int32.MaxValue;
        }

        public override String GetShopPrompt(Player player)
        {
            String recipePrompt;
            if (RecipeRepository.TryGetNextToUnlock(player, out RecipeInfo nextCheeseToUnlock))
            {
                var cheesePoints = player.GetModifiedPoints(nextCheeseToUnlock.Points);

                if (nextCheeseToUnlock.RankToUnlock > player.Rank)
                {
                    recipePrompt = $"{nextCheeseToUnlock.Name} (+{cheesePoints})] unlocked at {player.Rank.Next()} rank"; 
                }
                else
                {
                    recipePrompt = $"{nextCheeseToUnlock.Name} (+{cheesePoints})] for {nextCheeseToUnlock.CostToUnlock} cheese"; 
                }

                return $"{base.GetShopPrompt(player)} [{recipePrompt}";
            }

            return null;
        }
    }
}
