using Chubberino.Database.Models;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Items.Upgrades;

public static class PlayerUpgradeExtensions
{
    public static UpgradeType GetPreviousUpgradeUnlocked(this Player player)
    {
        if (player.NextWorkerProductionUpgradeUnlock > player.NextStorageUpgradeUnlock)
        {
            return UpgradeType.WorkerProduction;
        }
        else if (player.NextQuestUpgradeUnlock > player.NextWorkerProductionUpgradeUnlock)
        {
            return UpgradeType.Quest;
        }
        else if (player.NextCriticalCheeseUpgradeUnlock > player.NextQuestUpgradeUnlock)
        {
            return UpgradeType.CriticalCheese;
        }
        else if (player.NextCheeseModifierUpgradeUnlock > player.NextCriticalCheeseUpgradeUnlock)
        {
            return UpgradeType.CheeseModifier;
        }
        else if (player.NextCheeseModifierUpgradeUnlock <= Rank.Legend)
        {
            // Either no unlocks have been bought
            if (player.NextStorageUpgradeUnlock == Rank.Bronze)
            {
                return UpgradeType.None;
            }
            // or at the start of a new rank's upgrades.
            return UpgradeType.Storage;
        }
        else
        {
            return UpgradeType.Storage;
        }
    }

    public static UpgradeType GetNextUpgradeToUnlock(this Player player)
    {
        if (player.NextWorkerProductionUpgradeUnlock > player.NextStorageUpgradeUnlock)
        {
            return UpgradeType.Storage;
        }
        else if (player.NextQuestUpgradeUnlock > player.NextWorkerProductionUpgradeUnlock)
        {
            return UpgradeType.WorkerProduction;
        }
        else if (player.NextCriticalCheeseUpgradeUnlock > player.NextQuestUpgradeUnlock)
        {
            return UpgradeType.Quest;
        }
        else if (player.NextCheeseModifierUpgradeUnlock > player.NextCriticalCheeseUpgradeUnlock)
        {
            return UpgradeType.CriticalCheese;
        }
        else if (player.NextCheeseModifierUpgradeUnlock <= Rank.Legend)
        {
            return UpgradeType.CheeseModifier;
        }
        else
        {
            return UpgradeType.None;
        }
    }

    public static Option<UpgradeInfo> GetNextCheeseModifierUpgrade(this Player player)
        => player.NextCheeseModifierUpgradeUnlock.GetCheeseModifierUpgrade();

    public static Option<UpgradeInfo> GetNextCriticalCheeseUpgrade(this Player player)
        => player.NextCriticalCheeseUpgradeUnlock.GetCriticalCheeseUpgrade();

    public static Option<UpgradeInfo> GetNextQuestUpgrade(this Player player)
        => player.NextQuestUpgradeUnlock.GetQuestUpgrade();

    public static Option<UpgradeInfo> GetNextWorkerProductionUpgrade(this Player player)
        => player.NextWorkerProductionUpgradeUnlock.GetWorkerProductionUpgrade();

    public static Option<UpgradeInfo> GetNextStorageUpgrade(this Player player)
        => player.NextStorageUpgradeUnlock.GetStorageUpgrade();

    public static Option<UpgradeInfo> GetPreviousCheeseModifierUpgrade(this Player player)
        => (player.NextCheeseModifierUpgradeUnlock - 1).GetCheeseModifierUpgrade();

    public static Option<UpgradeInfo> GetPreviousCriticalCheeseUpgrade(this Player player)
        => (player.NextCriticalCheeseUpgradeUnlock - 1).GetCriticalCheeseUpgrade();

    public static Option<UpgradeInfo> GetPreviousQuestUpgrade(this Player player)
        => (player.NextQuestUpgradeUnlock - 1).GetQuestUpgrade();

    public static Option<UpgradeInfo> GetPreviousWorkerProductionUpgrade(this Player player)
        => (player.NextWorkerProductionUpgradeUnlock - 1).GetWorkerProductionUpgrade();

    public static Option<UpgradeInfo> GetPreviousStorageUpgrade(this Player player)
        => (player.NextStorageUpgradeUnlock - 1).GetStorageUpgrade();

    private static Option<Func<Player, Option<UpgradeInfo>>> TryGetTypeToNextUpgrade(UpgradeType type) => type switch
    {
        UpgradeType.CheeseModifier => Option<Func<Player, Option<UpgradeInfo>>>.Some(GetNextCheeseModifierUpgrade),
        UpgradeType.CriticalCheese => Option<Func<Player, Option<UpgradeInfo>>>.Some(GetNextCriticalCheeseUpgrade),
        UpgradeType.Quest => Option<Func<Player, Option<UpgradeInfo>>>.Some(GetNextQuestUpgrade),
        UpgradeType.WorkerProduction => Option<Func<Player, Option<UpgradeInfo>>>.Some(GetNextWorkerProductionUpgrade),
        UpgradeType.Storage => Option<Func<Player, Option<UpgradeInfo>>>.Some(GetNextStorageUpgrade),
        _ => Option<Func<Player, Option<UpgradeInfo>>>.None,
    };

    private static Option<Func<Player, Option<UpgradeInfo>>> TryGetTypeToPreviousUpgrade(UpgradeType type) => type switch
    {
        UpgradeType.CheeseModifier => Option<Func<Player, Option<UpgradeInfo>>>.Some(GetPreviousCheeseModifierUpgrade),
        UpgradeType.CriticalCheese => Option<Func<Player, Option<UpgradeInfo>>>.Some(GetPreviousCriticalCheeseUpgrade),
        UpgradeType.Quest => Option<Func<Player, Option<UpgradeInfo>>>.Some(GetPreviousQuestUpgrade),
        UpgradeType.WorkerProduction => Option<Func<Player, Option<UpgradeInfo>>>.Some(GetPreviousWorkerProductionUpgrade),
        UpgradeType.Storage => Option<Func<Player, Option<UpgradeInfo>>>.Some(GetPreviousStorageUpgrade),
        _ => Option<Func<Player, Option<UpgradeInfo>>>.None
    };

    public static Option<UpgradeInfo> TryPreviousUpgradeUnlocked(this Player player)
        => TryGetTypeToPreviousUpgrade(player.GetPreviousUpgradeUnlocked())
            .Some(applyUpgrade => applyUpgrade(player))
            .None(Option<UpgradeInfo>.None);


    public static Option<UpgradeInfo> TryGetNextUpgradeToUnlock(this Player player)
        => TryGetTypeToNextUpgrade(player.GetNextUpgradeToUnlock())
            .Some(applyUpgrade => applyUpgrade(player))
            .None(Option<UpgradeInfo>.None);

}
