using Chubberino.Modules.CheeseGame.Rankings;
using Chubberino.Modules.CheeseGame.Upgrades;
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

        public Rank Rank { get; set; }

        public Rank LastWorkerProductionUpgradeUnlocked { get; set; }

        public Rank LastWorkerQuestHelpUnlocked { get; set; }

        public Int32 Prestige { get; set; }

        public Int32 MaximumPointStorage { get; set; }

        public Int32 WorkerCount { get; set; }

        public Int32 PopulationCount { get; set; }

        public Int32 CheeseUnlocked { get; set; }

        public Int32 MouseTrapCount { get; set; }

        public Boolean IsMouseInfested { get; set; }
    }
}
