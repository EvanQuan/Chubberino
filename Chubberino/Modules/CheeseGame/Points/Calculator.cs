using Chubberino.Modules.CheeseGame.Rankings;
using System;

namespace Chubberino.Modules.CheeseGame.Points
{
    public sealed class Calculator : ICalculator
    {

        public const Double QuestRewardRankMultiplier = 0.75;

        public const Double QuestRewardRankExponent = 2;

        /// <summary>
        /// The additional worker point percent increase per upgrade.
        /// </summary>
        public const Double WorkerUpgradePercent = 0.02;

        /// <summary>
        /// The base point increase value workers provide.
        /// </summary>
        public const Double BaseWorkerPointPercent = 0.1;


        public Double GetQuestRewardMultiplier(Rank rank)
        {
            return Math.Pow((1 + (Int32)rank * QuestRewardRankMultiplier), QuestRewardRankExponent);
        }

        public Double GetWorkerPointMultiplier(Rank rank)
        {
            return BaseWorkerPointPercent + ((Int32)rank) * WorkerUpgradePercent;
        }
    }
}
