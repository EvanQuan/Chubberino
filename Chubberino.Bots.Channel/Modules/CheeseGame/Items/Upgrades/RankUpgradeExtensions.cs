using Chubberino.Bots.Channel.Modules.CheeseGame.Items.Upgrades.RecipeModifiers;
using Chubberino.Bots.Channel.Modules.CheeseGame.Items.Workers;
using Chubberino.Bots.Channel.Modules.CheeseGame.Quests;
using Chubberino.Common.Extensions;
using Chubberino.Database.Models;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Items.Upgrades;

public static class RankUpgradeExtensions
{
    /// <summary>
    /// Additive percent bonus for each storage upgrade.
    /// </summary>
    public const Double StorageUpgradePercent = 0.5;

    /// <summary>
    /// Additive percent bonus for critical cheese occurring for each
    /// critical cheese upgrade.
    /// </summary>
    public const Double CriticalCheeseUpgradePercent = 0.005;

    private const String ModifierDescription = "Chance to make {0} cheese (+{1} to base cheese value)";

    private const String CriticalCheeseDescription = "{0}% -> {1}% critical cheese chance";

    private const String ProductionDescription = "+{0}% -> +{1}% cheese per worker";

    private const String StorageDescription = "+{0}% -> +{1}% storage increase";

    private const String QuestDescription = "{0}% -> {1}% rare quest chance";

    public static Option<UpgradeInfo> GetCheeseModifierUpgrade(this Rank rank)
        => RecipeModifierRepository
            .Modifiers
            .TryGet((Int32)(rank + 1))
            .Bind(cheeseModifier =>
            {
                return Option<UpgradeInfo>.Some(new UpgradeInfo(
                    String.Format(ModifierDescription, cheeseModifier.Name, cheeseModifier.Points),
                    rank,
                    0.25,
                    x => x.NextCheeseModifierUpgradeUnlock++));
            });

    public static Option<UpgradeInfo> GetCriticalCheeseUpgrade(this Rank rank)
    {
        Double currentUpgradePercent = (Int32)rank * CriticalCheeseUpgradePercent * 100;
        Double nextUpgradePercent = (Int32)(rank + 1) * CriticalCheeseUpgradePercent * 100;
        return new UpgradeInfo(
            String.Format(CriticalCheeseDescription, String.Format("{0:0.0}", currentUpgradePercent), String.Format("{0:0.0}", nextUpgradePercent)),
            rank,
            0.40,
            x => x.NextCriticalCheeseUpgradeUnlock++);
    }

    public static Option<UpgradeInfo> GetQuestUpgrade(this Rank rank)
    {
        String currentUpgradePercent = String.Format("{0:0.0}", rank.GetRareQuestChance() * 100);
        String nextUpgradePercent = String.Format("{0:0.0}", rank.Next().GetRareQuestChance() * 100);
        return new UpgradeInfo(
            String.Format(QuestDescription, currentUpgradePercent, nextUpgradePercent),
            rank,
            0.55,
            x => x.NextQuestUpgradeUnlock++);
    }

    public static Option<UpgradeInfo> GetWorkerProductionUpgrade(this Rank rank)
    {
        Int32 currentUpgradePercent = (Int32)(rank.GetWorkerPointMultiplier() * 100);
        Int32 nextUpgradePercent = (Int32)(rank.Next().GetWorkerPointMultiplier() * 100);
        return new UpgradeInfo(
            String.Format(ProductionDescription, currentUpgradePercent, nextUpgradePercent),
            rank,
            0.70,
            x => x.NextWorkerProductionUpgradeUnlock++);
    }

    public static Option<UpgradeInfo> GetStorageUpgrade(this Rank rank)
    {
        Double currentUpgradePercent = (Int32)rank * StorageUpgradePercent * 100;
        Double nextUpgradePercent = (Int32)(rank + 1) * StorageUpgradePercent * 100;
        return new UpgradeInfo(
            String.Format(StorageDescription, currentUpgradePercent, nextUpgradePercent),
            rank,
            0.85,
            x => x.NextStorageUpgradeUnlock++);
    }
}
