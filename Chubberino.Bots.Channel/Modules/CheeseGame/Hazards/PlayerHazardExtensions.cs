using Chubberino.Database.Models;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Hazards;

public static class PlayerHazardExtensions
{
    /// <summary>
    /// Gets if the player is currently infested.
    /// </summary>
    /// <param name="player">Player to check if they are infested.</param>
    /// <returns>true if the player is currently infested; false otherwise.</returns>
    public static Boolean IsInfested(this Player player)
        => player.RatCount > 0;
}
