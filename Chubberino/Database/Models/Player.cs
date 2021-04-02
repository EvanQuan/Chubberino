using Chubberino.Modules.CheeseGame.Rankings;
using System;

namespace Chubberino.Modules.CheeseGame.Models
{
    public class Player
    {
        public Int32 ID { get; set; } 

        public String TwitchUserID { get; set; }

        public String Name { get; set; }

        public Int32 Points { get; set; }

        public DateTime LastPointsGained { get; set; }

        public DateTime LastQuestVentured { get; set; }

        public DateTime LastHeistInitiated { get; set; }

        public Rank Rank { get; set; }

        public Rank NextWorkerProductionUpgradeUnlock { get; set; }

        public Rank NextQuestRewardUpgradeUnlock { get; set; }

        public Rank NextCheeseModifierUpgradeUnlock { get; set; }

        public Int32 Prestige { get; set; }

        public Int32 MaximumPointStorage { get; set; }

        public Int32 WorkerCount { get; set; }

        public Int32 PopulationCount { get; set; }

        public Int32 CheeseUnlocked { get; set; }

        public Int32 MouseTrapCount { get; set; }

        /// <summary>
        /// Number of mice present in the factory.
        /// </summary>
        public Int32 MouseCount { get; set; }

        public Rank NextStorageUpgradeUnlock { get; set; }

        public Rank NextCriticalCheeseUpgradeUnlock { get; set; }

        public Int32 CatCount { get; set; }

        public Int32 ContributedDamageToBoss { get; set; }
        
        /// <summary>
        /// Number of quests unlocked.
        /// </summary>
        public Int32 QuestsUnlockedCount { get; set; }
    }
}
