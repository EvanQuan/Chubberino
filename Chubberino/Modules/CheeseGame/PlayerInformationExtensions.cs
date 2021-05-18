using Chubberino.Modules.CheeseGame.Items;
using Chubberino.Modules.CheeseGame.Models;
using System;

namespace Chubberino.Modules.CheeseGame
{
    public static class PlayerInformationExtensions
    {
        public static String GetDisplayName(this Player player)
        {
            String prestige = player.Prestige > 0 ? "P" + player.Prestige + " " : String.Empty;
            String cheese = $"{player.Points}/{player.GetTotalStorage()} cheese";
            String workers = $"{player.WorkerCount}/{player.PopulationCount} workers";
            String mousetraps = $"{player.MouseTrapCount} mousetrap{(player.MouseTrapCount != 1 ? "s" : String.Empty)}";
            return $"{player.Name} [{prestige}{player.Rank}, {cheese}, {workers}, {mousetraps}]";
        }
    }
}
