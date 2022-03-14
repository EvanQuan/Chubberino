using System;
using System.Collections.Generic;
using System.Linq;

namespace Chubberino.Common.Extensions
{
    public static class IEnumerableExtensions
    {
        public static Boolean TryGetFirst<TElement>(this IEnumerable<TElement> source, Func<TElement, Boolean> predicate, out TElement element)
        {
            element = source.FirstOrDefault(predicate);

            return !Equals(default(TElement), element);
        }

        public static Boolean TryGetFirst<TElement>(this IEnumerable<TElement> source, out TElement element)
        {
            element = source.FirstOrDefault();

            return !Equals(default(TElement), element);
        }

        public static Boolean TryGetFirst<TElement>(this IEnumerable<TElement> source, out TElement element, out IEnumerable<TElement> next)
        {
            element = source.FirstOrDefault();

            next = source.Skip(1);

            return !Equals(default(TElement), element);
        }

        public static IEnumerable<TType> ForEach<TType>(this IEnumerable<TType> source, Action<TType> action)
        {
            foreach (var element in source)
            {
                action(element);
            }

            return source;
        }

        public static String ToLineDelimitedString<TType>(this IEnumerable<TType> source, Int32 tabCount)
        {
            String delimiter = Environment.NewLine + new String('\t', tabCount);
            return String.Join(delimiter, source);
        }
    }
}
