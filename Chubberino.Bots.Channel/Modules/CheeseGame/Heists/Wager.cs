using System;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Heists;

public sealed class Wager
{
    public String PlayerTwitchID { get; init; }

    public Int32 WageredPoints { get; set; }
}
