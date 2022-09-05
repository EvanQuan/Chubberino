using System;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Heists;

public struct Wager
{
    public String PlayerTwitchID { get; init; }

    public Int32 WageredPoints { get; set; }
}
