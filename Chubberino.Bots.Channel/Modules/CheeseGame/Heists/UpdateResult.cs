namespace Chubberino.Bots.Channel.Modules.CheeseGame.Heists;

public enum UpdateResult
{
    /// <summary>
    /// Not joined.
    /// </summary>
    InvalidWager,

    /// <summary>
    /// Joined the heist for the first time.
    /// </summary>
    Joined,

    /// <summary>
    /// Updated a pre-existing wager.
    /// </summary>
    Updated,

    /// <summary>
    /// Left a pre-existing wager.
    /// </summary>
    Left,
}
