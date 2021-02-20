using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
