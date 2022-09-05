using Chubberino.Bots.Channel.Modules.CheeseGame.Ranks;
using Chubberino.Database.Models;
using System;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Items.Recipes;

public sealed class RecipeInfo
{
    public RecipeInfo(String name, Int32 pointValue)
    {
        Name = name;
        Points = pointValue;
        CostToUnlock = 0;
        RankToUnlock = Rank.Bronze;
        UnlocksNegativeCheese = false;
    }
    public RecipeInfo(
        String name,
        Int32 pointValue,
        Rank rankToUnlock,
        Int32 costToUnlock,
        Boolean unlocksNegativeCheese = false)
    {
        Name = name;
        Points = pointValue;
        RankToUnlock = rankToUnlock;
        CostToUnlock = costToUnlock;
        UnlocksNegativeCheese = unlocksNegativeCheese;
    }

    public RecipeInfo(
        String name,
        Int32 pointValue,
        Rank rankToUnlock,
        Double rankPricePercentPrice,
        Boolean unlocksNegativeCheese = false)
    {
        Name = name;
        Points = pointValue;
        RankToUnlock = rankToUnlock;
        CostToUnlock = (Int32)(RankManager.RanksToPoints[RankToUnlock] * rankPricePercentPrice);
        UnlocksNegativeCheese = unlocksNegativeCheese;
    }

    public String Name { get; }

    public Int32 Points { get; }

    public Int32 CostToUnlock { get; }

    public Rank RankToUnlock { get; }

    public Boolean UnlocksNegativeCheese { get; }
}
