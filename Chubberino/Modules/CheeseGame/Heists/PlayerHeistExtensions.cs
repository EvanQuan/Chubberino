using Chubberino.Modules.CheeseGame.Models;
using System;

namespace Chubberino.Modules.CheeseGame.Heists
{
    public static class PlayerHeistExtensions
    {
        public static Boolean TryGetWager(this Player player, String proposedWager, out Int32 wager)
        {
            if (Int32.TryParse(proposedWager, out wager))
            {
                return true;
            }
            else if (proposedWager.Equals("all", StringComparison.OrdinalIgnoreCase) || proposedWager.Equals("a", StringComparison.OrdinalIgnoreCase))
            {
                wager = player.Points;
                return true;
            }
            else if (proposedWager.EndsWith("k", StringComparison.OrdinalIgnoreCase) && Double.TryParse(proposedWager.Substring(0, proposedWager.Length - 1), out Double wagerDouble))
            {
                wager = (Int32)Math.Ceiling(wagerDouble * 1000);
                return true;
            }
            else if (proposedWager.EndsWith('%') && Double.TryParse(proposedWager.Substring(0, proposedWager.Length - 1), out Double wagerPercent))
            {
                wager = (Int32)Math.Ceiling(wagerPercent / 100.0 * player.Points);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
