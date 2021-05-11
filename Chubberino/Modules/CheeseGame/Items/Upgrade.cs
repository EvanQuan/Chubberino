using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.Upgrades;
using Monad;
using System;
using System.Collections.Generic;

namespace Chubberino.Modules.CheeseGame.Items
{
    public sealed class Upgrade : Item
    {
        public override IEnumerable<String> Names => new String[] { "Upgrade", "u", "up", "upgrades" };

        public override Int32 GetPrice(Player player)
        {
            if (player.TryGetNextUpgradeToUnlock(out var upgrade))
            {
                return upgrade.Price;
            }

            return Int32.MaxValue;
        }

        public override String GetSpecificNameForNotEnoughToBuy(Player player)
        {
            if (player.TryGetNextUpgradeToUnlock(out var upgrade))
            {
                return $"the {upgrade.Description}";
            }

            return UnexpectedErrorMessage;
        }

        public override String GetSpecificNameForSuccessfulBuy(Player player, Int32 quantity)
        {
            if (player.TryPreviousUpgradeUnlocked(out var upgrade))
            {
                return $"the {upgrade.Description} upgrade{(quantity == 1 ? String.Empty : $" and {quantity - 1} other{(quantity - 1 == 1 ? String.Empty : "s")}")}";
            }

            return UnexpectedErrorMessage;
        }

        public override Either<Int32, String> TryBuySingleUnit(Player player, Int32 price)
        {
            if (!player.TryGetNextUpgradeToUnlock(out var nextQuestToUnlock))
            {
                return () => UnexpectedErrorMessage;
            }

            player.QuestsUnlockedCount++;

            player.Points -= nextQuestToUnlock.Price;

            return () => 1;
        }
    }
}
