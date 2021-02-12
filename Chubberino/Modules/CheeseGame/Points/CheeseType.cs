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
        }
        public CheeseType(String name, Int32 pointValue, Rank rankToUnlock, Int32 costToUnlock)
        {
            Name = name;
            PointValue = pointValue;
            RankToUnlock = rankToUnlock;
            CostToUnlock = costToUnlock;
        }

        public String Name { get; }

        public Int32 PointValue { get; }

        public Int32 CostToUnlock { get; }

        public Rank RankToUnlock { get; }
    }
}
