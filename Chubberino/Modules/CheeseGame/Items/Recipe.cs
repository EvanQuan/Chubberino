using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.Points;
using Monad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// There are no recipes available to buy right now. All recipes have already been purchased.
        /// </summary>
        public const String NoRecipeForSaleMessage = "There is no recipe for sale right now.";

        public const String UnexpectedErrorMessage = "This error message should never show up.";

        public override IEnumerable<String> Names => new String[] { "Recipe", "r", "recipes" };

        public IRepository<CheeseType> CheeseRepository { get; }

        public Recipe(IRepository<CheeseType> cheeseRepository)
        {
            CheeseRepository = cheeseRepository;
        }


        public override String GetSpecificNameForNotEnoughToBuy(Player player)
        {
            throw new NotImplementedException();
        }

        public override String GetSpecificNameForSuccessfulBuy(Player player, Int32 quantity)
        {
            throw new NotImplementedException();
        }

        public override Either<Int32, String> TryBuySingleUnit(Player player, Int32 price)
        {
            if (!CheeseRepository.TryGetNextToUnlock(player, out var nextCheeseToUnlock))
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
            if (CheeseRepository.TryGetNextToUnlock(player, out var cheese))
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
            if (CheeseRepository.TryGetNextToUnlock(player, out var cheese))
            {
                return cheese.CostToUnlock;
            }

            return Int32.MaxValue;
        }
    }
}
