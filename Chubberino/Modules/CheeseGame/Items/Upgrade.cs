﻿using Chubberino.Modules.CheeseGame.Items.Upgrades;
using Chubberino.Modules.CheeseGame.Models;
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
                return $"the {upgrade.Description} upgrade";
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
            if (!player.TryGetNextUpgradeToUnlock(out var nextUpgradeToLock))
            {
                return () => UnexpectedErrorMessage;
            }

            nextUpgradeToLock.UpdatePlayer(player);

            player.Points -= nextUpgradeToLock.Price;

            return () => 1;
        }

        public override String GetShopPrompt(Player player)
        {
            String upgradePrompt;
            if (player.TryGetNextUpgradeToUnlock(out var upgrade))
            {
                if (upgrade.RankToUnlock > player.Rank)
                {
                    upgradePrompt = $"{upgrade.Description}] unlocked at {upgrade.RankToUnlock} rank";
                }
                else
                {
                    upgradePrompt = $"{upgrade.Description}] for {upgrade.Price} cheese";
                }

                return $"{base.GetShopPrompt(player)} [{upgradePrompt}";
            }

            return null;
        }
    }
}
