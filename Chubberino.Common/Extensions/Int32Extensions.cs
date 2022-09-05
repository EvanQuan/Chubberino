using System;

namespace Chubberino.Common.Extensions;

public static class Int32Extensions
{
    /// <summary>
    /// Repeats the specified <paramref name="action"/> <paramref name="source"/> number of times.
    /// </summary>
    /// <param name="source">Number of times to repeat <paramref name="action"/>.</param>
    /// <param name="action">Action to repeat.</param>
    public static void Repeat(this Int32 source, Action action)
    {
        for (Int32 i = 0; i < source; i++)
        {
            action();
        }
    }

    /// <summary>
    /// Repeats the specified <paramref name="action"/> <paramref name="source"/> number of times, with the index of the iteration.
    /// </summary>
    /// <param name="source">Number of times to repeat <paramref name="action"/>.</param>
    /// <param name="action">Action to repeat.</param>
    public static void Repeat(this Int32 source, Action<Int32> action)
    {
        for (Int32 i = 0; i < source; i++)
        {
            action(i);
        }
    }

    public static Int32 Max(this Int32 source, Int32 other)
    {
        return Math.Max(source, other);
    }

    public static Int32 Min(this Int32 source, Int32 other)
    {
        return Math.Min(source, other);
    }
}
