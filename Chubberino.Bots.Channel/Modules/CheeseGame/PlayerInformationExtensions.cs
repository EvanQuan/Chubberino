using Chubberino.Bots.Channel.Modules.CheeseGame.Items.Storages;
using Chubberino.Database.Models;
using System;

namespace Chubberino.Bots.Channel.Modules.CheeseGame;

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
