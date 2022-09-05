using Chubberino.Database.Models;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Ranks;

public static class PlayerRankExtensions
{
    public static Player ResetRank(this Player player)
    {
        player.MaximumPointStorage = 50;
        player.Points = 0;
        player.PopulationCount = 0;
        player.WorkerCount = 0;
        player.Rank = Rank.Bronze;
        player.CheeseUnlocked = 0;
        player.NextWorkerProductionUpgradeUnlock = 0;
        player.NextQuestUpgradeUnlock = 0;
        player.NextCheeseModifierUpgradeUnlock = 0;
        player.NextStorageUpgradeUnlock = 0;
        player.NextCriticalCheeseUpgradeUnlock = 0;
        player.MouseTrapCount = 0;
        player.RatCount = 0;
        player.CatCount = 0;
        player.ContributedDamageToBoss = 0;
        player.QuestsUnlockedCount = 0;
        player.GearCount = 0;

        return player;
    }
}
