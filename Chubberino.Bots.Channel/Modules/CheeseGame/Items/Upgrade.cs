using System.Collections.Generic;
using Chubberino.Bots.Channel.Modules.CheeseGame.Items.Upgrades;
using Chubberino.Database.Models;
using LanguageExt.Common;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Items;

public sealed class Upgrade : Item
{
    private const String NoUpgradesForSaleMessage = "You have already purchased every upgrade.";

    public override IEnumerable<String> Names { get; } = new String[]
    {
        "Upgrade",
        "u",
        "up",
        "upgrades"
    };

    public override Either<Int32, String> GetPrice(Player player)
        => player
            .TryGetNextUpgradeToUnlock()
            .Some(upgrade => Either<Int32, String>.Left(upgrade.Price))
            .None(NoUpgradesForSaleMessage);

    public override String GetSpecificNameForNotEnoughToBuy(Player player)
        => player
            .TryGetNextUpgradeToUnlock()
            .Some(upgrade => $"the {upgrade.Description} upgrade")
            .None(NoUpgradesForSaleMessage);

    public override String GetSpecificNameForSuccessfulBuy(Player player, Int32 quantity)
        => player
            .TryPreviousUpgradeUnlocked()
            .Some(upgrade =>
                $"the {upgrade.Description} upgrade{(quantity == 1 ? String.Empty : $" and {quantity - 1} other{(quantity - 1 == 1 ? String.Empty : "s")}")}")
            .None(NoUpgradesForSaleMessage);

    public override Either<Int32, String> TryBuySingleUnit(Player player, Int32 price)
        => player
            .TryGetNextUpgradeToUnlock()
            .Some(nextUpgradeToLock =>
            {
                nextUpgradeToLock.UpdatePlayer(player);

                player.Points -= nextUpgradeToLock.Price;

                return Either<Int32, String>.Left(1);
            })
            .None(NoUpgradesForSaleMessage);

    public override Option<String> GetShopPrompt(Player player)
        => player
            .TryGetNextUpgradeToUnlock()
            .Bind(upgrade =>
            {
                String upgradePrompt = upgrade.RankToUnlock > player.Rank
                    ? $"{upgrade.Description}] unlocked at {upgrade.RankToUnlock} rank"
                    : $"{upgrade.Description}] for {upgrade.Price} cheese";

                return Option<String>.Some($"{base.GetShopPrompt(player)} [{upgradePrompt}");
            });
}
