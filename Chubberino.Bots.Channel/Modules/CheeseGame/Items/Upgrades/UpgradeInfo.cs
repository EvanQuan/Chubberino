using Chubberino.Bots.Channel.Modules.CheeseGame.Ranks;
using Chubberino.Database.Models;
using System;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Items.Upgrades;

public sealed class UpgradeInfo
{
    public UpgradeInfo(String description, Rank rankToUnlock, Double rankPricePercentPrice, Action<Player> updatePlayer)
    {
        Description = description;
        RankToUnlock = rankToUnlock;
        Price = (Int32)(RankManager.RanksToPoints[RankToUnlock] * rankPricePercentPrice);
        UpdatePlayer = updatePlayer;
    }

    public String Description { get; }


    public Rank RankToUnlock { get; }

    public Int32 Price { get; }

    public Action<Player> UpdatePlayer { get; }
}
