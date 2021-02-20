using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.Rankings;
using System;

namespace Chubberino.Modules.CheeseGame.Upgrades
{
    public class Upgrade
    {
        public Upgrade(String description, Rank rankToUnlock, Int32 price, Action<Player> updatePlayer)
        {
            Description = description;
            RankToUnlock = rankToUnlock;
            Price = price;
            UpdatePlayer = updatePlayer;
        }

        public String Description { get; }


        public Rank RankToUnlock { get; }

        public Int32 Price { get; }

        public Action<Player> UpdatePlayer { get; }
    }
}
