using System;

namespace Chubberino.Modules.CheeseGame.Heists
{
    public class Wager
    {
        public Wager(String playerTwitchID, Int32 wageredPoints)
        {
            PlayerTwitchID = playerTwitchID;
            WageredPoints = wageredPoints;
        }

        public String PlayerTwitchID { get; }

        public Int32 WageredPoints { get; set; }
    }
}
