using Chubberino.Modules.CheeseGame.Rankings;
using System;

namespace Chubberino.Modules.CheeseGame.Points
{
    public sealed class CheeseType
    {
        public CheeseType(String name, Int32 pointValue)
        {
            Name = name;
            PointValue = pointValue;
            CostToUnlock = 0;
            RankToUnlock = Rank.Bronze;
            UnlocksNegativeCheese = false;
        }
        public CheeseType(String name, Int32 pointValue, Rank rankToUnlock, Int32 costToUnlock, Boolean unlocksNegativeCheese = false)
        {
            Name = name;
            PointValue = pointValue;
            RankToUnlock = rankToUnlock;
            CostToUnlock = costToUnlock;
            UnlocksNegativeCheese = unlocksNegativeCheese;
        }

        public String Name { get; }

        public Int32 PointValue { get; }

        public Int32 CostToUnlock { get; }

        public Rank RankToUnlock { get; }

        public Boolean UnlocksNegativeCheese { get; }
    }
}
