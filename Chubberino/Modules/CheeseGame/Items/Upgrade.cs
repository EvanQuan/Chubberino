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

        public IUpgradeManager UpgradeManager { get; }

        public Upgrade(IUpgradeManager upgradeManager)
        {
            UpgradeManager = upgradeManager;
        }

        public override Int32 GetPrice(Player player)
        {
            if (UpgradeManager.TryGetNextUpgradeToUnlock(player, out var upgrade))
            {
                return upgrade.Price;
            }

            return Int32.MaxValue;
        }

        public override String GetSpecificNameForNotEnoughToBuy(Player player)
        {
            if (UpgradeManager.TryGetNextUpgradeToUnlock(player, out var upgrade))
            {
                return $"the {upgrade.Description}";
            }

            return UnexpectedErrorMessage;
        }

        public override String GetSpecificNameForSuccessfulBuy(Player player, Int32 quantity)
        {
            var temporaryPlayer = new Player()
            {
                QuestsUnlockedCount = player.QuestsUnlockedCount - quantity
            };

            List<String> upgradeDescriptionsPurchased = new();

            for (Int32 i = 0; i < quantity; i++, temporaryPlayer.QuestsUnlockedCount++)
            {
                UpgradeManager.TryGetNextUpgradeToUnlock(temporaryPlayer, out var upgrade);

                upgradeDescriptionsPurchased.Add(upgrade.Description);
            }

            return $"the {String.Join(", ", upgradeDescriptionsPurchased)} upgrade{(quantity == 1 ? String.Empty : "s")}";
        }

        public override Either<Int32, String> TryBuySingleUnit(Player player, Int32 price)
        {
            if (!UpgradeManager.TryGetNextUpgradeToUnlock(player, out var nextQuestToUnlock))
            {
                return () => UnexpectedErrorMessage;
            }

            player.QuestsUnlockedCount++;

            player.Points -= nextQuestToUnlock.Price;

            return () => 1;
        }
    }
}
