﻿namespace Chubberino.Common.Extensions;

public static class RandomExtensions
{
    /// <summary>
    /// Get the result of a percent chance success.
    /// </summary>
    /// <param name="random">Source.</param>
    /// <param name="successChance">Percent chance of success.</param>
    /// <returns>true on success; false otherwise.</returns>
    public static Boolean TryPercentChance(this Random random, Double successChance)
        => random.NextDouble() < successChance;

    /// <summary>
    /// Gets a random double bounded beetween a <paramref name="minimum"/> and <paramref name="maximum"/> range.
    /// </summary>
    /// <param name="random">Source.</param>
    /// <param name="minimum">Lower bound.</param>
    /// <param name="maximum">Upper bound.</param>
    /// <returns>A random double.</returns>
    public static Double NextDouble(this Random random, Double minimum, Double maximum)
        => random.NextDouble() * (maximum - minimum) + minimum;

    /// <summary>
    /// Get a random element from the specified <paramref name="list"/>.
    /// </summary>
    /// <typeparam name="TElement">The type of elements in the list.</typeparam>
    /// <param name="random">Source.</param>
    /// <param name="list">List to get an element from.</param>
    /// <returns>A random element of the <paramref name="list"/>.</returns>
    public static TElement NextElement<TElement>(this Random random, TElement[] list)
        => list[random.Next(list.Length)];

    /// <summary>
    /// Get a random element from the specified <paramref name="list"/>.
    /// </summary>
    /// <typeparam name="TElement">The type of elements in the list.</typeparam>
    /// <param name="random">Source.</param>
    /// <param name="list">List to get an element from.</param>
    /// <returns>A random element of the <paramref name="list"/>.</returns>
    public static TElement NextElement<TElement>(this Random random, IList<TElement> list)
        => list[random.Next(list.Count)];

    /// <summary>
    /// Get a random element from the specified <paramref name="list"/>.
    /// </summary>
    /// <typeparam name="TElement">The type of elements in the list.</typeparam>
    /// <param name="random">Source.</param>
    /// <param name="list">List to get an element from.</param>
    /// <returns>A random element of the <paramref name="list"/>.</returns>
    public static TElement NextElement<TElement>(this Random random, IReadOnlyList<TElement> list)
        => list[random.Next(list.Count)];

    /// <summary>
    /// Get a random element from the specified <paramref name="list"/>,
    /// with an inclusive upper bound on the <paramref name="maximumIndex"/>
    /// of that list.
    /// </summary>
    /// <typeparam name="TElement">The type of elements in the list.</typeparam>
    /// <param name="random">Source.</param>
    /// <param name="list">List to get an element from.</param>
    /// <param name="maximumIndex">Maximum index of the list, inclusive.</param>
    /// <returns>A random element of the <paramref name="list"/>.</returns>
    public static TElement NextElement<TElement>(this Random random, TElement[] list, Int32 maximumIndex)
    {
        var exclusiveMax = (maximumIndex + 1).Max(0).Min(list.Length);
        var finalIndex = random.Next(0, exclusiveMax);
        return list[finalIndex];
    }

    /// <summary>
    /// Get a random element from the specified <paramref name="list"/>,
    /// with an inclusive upper bound on the <paramref name="maximumIndex"/>
    /// of that list.
    /// </summary>
    /// <typeparam name="TElement">The type of elements in the list.</typeparam>
    /// <param name="random">Source.</param>
    /// <param name="list">List to get an element from.</param>
    /// <param name="maximumIndex">Maximum index of the list, inclusive.</param>
    /// <returns>A random element of the <paramref name="list"/>.</returns>
    public static TElement NextElement<TElement>(this Random random, IList<TElement> list, Int32 maximumIndex)
    {
        var exclusiveMax = (maximumIndex + 1).Max(0).Min(list.Count);
        var finalIndex = random.Next(0, exclusiveMax);
        return list[finalIndex];
    }

    /// <summary>
    /// Get a random element from the specified <paramref name="list"/>,
    /// with an inclusive upper bound on the <paramref name="maximumIndex"/>
    /// of that list.
    /// </summary>
    /// <typeparam name="TElement">The type of elements in the list.</typeparam>
    /// <param name="random">Source.</param>
    /// <param name="list">List to get an element from.</param>
    /// <param name="maximumIndex">Maximum index of the list, inclusive.</param>
    /// <returns>A random element of the <paramref name="list"/>.</returns>
    public static TElement NextElement<TElement>(this Random random, IReadOnlyList<TElement> list, Int32 maximumIndex)
    {
        var exclusiveMax = (maximumIndex + 1).Max(0).Min(list.Count);
        var finalIndex = random.Next(0, exclusiveMax);
        return list[finalIndex];
    }

    /// <summary>
    /// Remove a random element from the specified <paramref name="list"/>.
    /// </summary>
    /// <typeparam name="TElement">The type of elements in the list.</typeparam>
    /// <param name="random">Source.</param>
    /// <param name="list">List to get an element from.</param>
    /// <returns>A random element of the <paramref name="list"/>.</returns>
    public static Option<TElement> RemoveElement<TElement>(this Random random, IList<TElement> list)
    {
        if (list.Count == 0)
        {
            return Option<TElement>.None;
        }

        Int32 index = random.Next(list.Count);
        var element = list[index];
        list.RemoveAt(index);
        return element;
    }
}
