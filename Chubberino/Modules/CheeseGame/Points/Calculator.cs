using Chubberino.Modules.CheeseGame.Rankings;
using System;

namespace Chubberino.Modules.CheeseGame.Points
{
    public sealed class Calculator : ICalculator
    {

        public const Double RewardRankMultiplier = 0.5;

        public const Double RewardRankExponent = 2;

        public Double GetQuestRewardMultiplier(Rank rank)
        {
            return Math.Pow((1 + (Int32)rank * RewardRankMultiplier), RewardRankExponent);
        }
    }
}
