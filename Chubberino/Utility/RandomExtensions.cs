using System;
using System.Collections.Generic;

namespace Chubberino.Utility
{
    public static class RandomExtensions
    {
        /// <summary>
        /// Get the result of a percent chance success.
        /// </summary>
        /// <param name="random"></param>
        /// <param name="successChance"></param>
        /// <returns></returns>
        public static Boolean TryPercentChance(this Random random, Double successChance)
        {
            return random.NextDouble() < successChance;
        }

        public static TElement GetElement<TElement>(this Random random, IList<TElement> list)
        {
            return list[random.Next(list.Count)];
        }
    }
}
