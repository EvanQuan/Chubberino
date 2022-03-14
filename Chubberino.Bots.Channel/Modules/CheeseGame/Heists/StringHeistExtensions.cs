using Chubberino.Modules.CheeseGame.Models;
using System;

namespace Chubberino.Modules.CheeseGame.Heists
{
    public static class StringHeistExtensions
    {
        public static Boolean TryGetWager(this String proposedWager, out Func<Player, Int32> wager)
        {
            if (Int32.TryParse(proposedWager, out var wagerInt))
            {
                wager = p => wagerInt;
                return true;
            }

            if (proposedWager.Equals("all", StringComparison.OrdinalIgnoreCase) || proposedWager.Equals("a", StringComparison.OrdinalIgnoreCase))
            {
                wager = p => p.Points;
                return true;
            }

            if (proposedWager.Equals("leave", StringComparison.OrdinalIgnoreCase) || proposedWager.Equals("l", StringComparison.OrdinalIgnoreCase))
            {
                wager = p => 0;
                return true;
            }

            if (proposedWager.Equals("none", StringComparison.OrdinalIgnoreCase) || proposedWager.Equals("n", StringComparison.OrdinalIgnoreCase))
            {
                wager = p => 0;
                return true;
            }

            if (proposedWager.EndsWith("k", StringComparison.OrdinalIgnoreCase) && Double.TryParse(proposedWager[0..^1], out Double wagerDouble))
            {
                wager = p => (Int32)Math.Ceiling(wagerDouble * 1000);
                return true;
            }

            if (proposedWager.EndsWith('%') && Double.TryParse(proposedWager[0..^1], out Double wagerPercent))
            {
                wager = p => (Int32)Math.Ceiling(wagerPercent / 100.0 * p.Points);
                return true;
            }

            wager = default;
            return false;
        }
    }
}
