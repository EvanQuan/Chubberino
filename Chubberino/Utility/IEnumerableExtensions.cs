using System;
using System.Collections.Generic;

namespace Chubberino.Utility
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<TType> ForEach<TType>(this IEnumerable<TType> source, Action<TType> action)
        {
            foreach (var element in source)
            {
                action(element);
            }

            return source;
        }
    }
}
