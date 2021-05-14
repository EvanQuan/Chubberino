using Chubberino.Modules.CheeseGame.Rankings;
using System;

namespace Chubberino.Modules.CheeseGame.Points
{
    public static class RankExtensions
    {
        public const Double RareQuestRankMultiplier = 0.5;

        /// <summary>
        /// The additional worker point percent increase per upgrade.
        /// </summary>
        public const Double WorkerUpgradePercent = 0.02;

        /// <summary>
        /// The base point increase value workers provide.
        /// </summary>
        public const Double BaseWorkerPointPercent = 0.1;


        public static Double GetRareQuestChanceMultiplier(this Rank rank)
        {
            return (Int32)rank * RareQuestRankMultiplier;
        }

        public static Double GetWorkerPointMultiplier(this Rank rank)
        {
            return BaseWorkerPointPercent + ((Int32)rank) * WorkerUpgradePercent;
        }
    }
}
