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

        public Rank Rank { get; set; }

        public Int32 Prestige { get; set; }

        public Int32 MaximumPointStorage { get; set; }

        public Int32 WorkerCount { get; set; }

        public Int32 PopulationCount { get; set; }

        public Int32 CheeseUnlocked { get; set; }
    }
}
