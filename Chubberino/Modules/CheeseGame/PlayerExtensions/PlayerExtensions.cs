using Chubberino.Modules.CheeseGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubberino.Modules.CheeseGame.PlayerExtensions
{
    public static class PlayerExtensions
    {
        public static Player ResetRank(this Player player)
        {
            player.MaximumPointStorage = 50;
            player.Points = 0;
            player.PopulationCount = 0;
            player.WorkerCount = 0;
            player.Rank = Rankings.Rank.Bronze;
            player.CheeseUnlocked = 0;

            return player;
        }
    }
}
