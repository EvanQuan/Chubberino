using Chubberino.Modules.CheeseGame.Rankings;
using System;

namespace Chubberino.Modules.CheeseGame.Points
{
    public static class RankExtensions
    {
        /// <summary>
        /// Base chance for rare quests.
        /// </summary>
        public const Double BaseRareQuestChance = 0.01;

        /// <summary>
        /// Chance increase per rare quest reward.
        /// </summary>
        public const Double RareQuestUpgradePercent = 0.005;

        /// <summary>
        /// The additional worker point percent increase per upgrade.
        /// </summary>
        public const Double WorkerUpgradePercent = 0.02;

        /// <summary>
        /// The base point increase value workers provide.
        /// </summary>
        public const Double BaseWorkerPointPercent = 0.1;


        /// <summary>
        /// Gets the chance for a rare quest to be chosen.
        /// </summary>
        /// <param name="rank"></param>
        /// <returns></returns>
        public static Double GetRareQuestChance(this Rank rank)
        {
            return BaseRareQuestChance + (Int32)rank * RareQuestUpgradePercent;
        }

        public static Double GetWorkerPointMultiplier(this Rank rank)
        {
            return BaseWorkerPointPercent + ((Int32)rank) * WorkerUpgradePercent;
        }
    }
}
