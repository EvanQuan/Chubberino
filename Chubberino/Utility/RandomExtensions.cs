using System;
using System.Collections.Generic;

namespace Chubberino.Utility
{
    public static class RandomExtensions
    {
        /// <summary>
        /// Get the result of a percent chance success.
        /// </summary>
        /// <param name="random">Source.</param>
        /// <param name="successChance">Percent chance of success.</param>
        /// <returns>true on success; false otherwise.</returns>
        public static Boolean TryPercentChance(this Random random, Double successChance)
        {
            return random.NextDouble() < successChance;
        }

        /// <summary>
        /// Gets a random double bounded beetween a <paramref name="minimum"/> and <paramref name="maximum"/> range.
        /// </summary>
        /// <param name="random">Source.</param>
        /// <param name="minimum">Lower bound.</param>
        /// <param name="maximum">Upper bound.</param>
        /// <returns>A random double.</returns>
        public static Double NextDouble(this Random random, Double minimum, Double maximum)
        {
            return random.NextDouble() * (maximum - minimum) + minimum;
        }

        /// <summary>
        /// Get a random element from the specified <paramref name="list"/>.
        /// </summary>
        /// <typeparam name="TElement">The type of elements in the list.</typeparam>
        /// <param name="random">Source.</param>
        /// <param name="list">List to get an element from.</param>
        /// <returns>A random element of the <paramref name="list"/>.</returns>
        public static TElement GetElement<TElement>(this Random random, IList<TElement> list)
        {
            return list[random.Next(list.Count)];
        }

        /// <summary>
        /// Remove a random element from the specified <paramref name="list"/>.
        /// </summary>
        /// <typeparam name="TElement">The type of elements in the list.</typeparam>
        /// <param name="random">Source.</param>
        /// <param name="list">List to get an element from.</param>
        /// <returns>A random element of the <paramref name="list"/>.</returns>
        public static TElement RemoveElement<TElement>(this Random random, IList<TElement> list)
        {
            Int32 index = random.Next(list.Count);
            var element = list[index];
            list.RemoveAt(index);

            return element;
        }

    }
}
