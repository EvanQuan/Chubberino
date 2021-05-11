using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.Points;
using Chubberino.Modules.CheeseGame.Quests;
using System;

namespace Chubberino.Modules.CheeseGame.Repositories
{
    public static class IRepositoryExtensions
    {
        public static Boolean TryGetNextToUnlock(this IRepository<CheeseModifier> repository, Player player, out CheeseModifier cheeseModifier)
        {
            // The default modifier is null, the 0th element of the repository,
            // which is always a possibility.
            // We add 1 to ensure this can be picked.
            return repository.TryGetNextToUnlock((Int32)(player.NextCheeseModifierUpgradeUnlock + 1), out cheeseModifier);
        }

        public static Boolean TryGetNextToUnlock(this IRepository<CheeseType> repository, Player player, out CheeseType cheeseType)
        {
            // Since the player starts off with 1 cheese unlocked by default,
            // we always can return the 0th element of the repository.
            // We add 1 to ensure this.
            return repository.TryGetNextToUnlock(player.CheeseUnlocked + 1, out cheeseType);
        }

        public static Boolean TryGetNextToUnlock(this IRepository<Quest> repository, Player player, out Quest quest)
        {
            return repository.TryGetNextToUnlock(player.QuestsUnlockedCount, out quest);
        }
    }
}
