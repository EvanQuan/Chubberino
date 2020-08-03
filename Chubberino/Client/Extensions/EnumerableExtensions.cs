using System;
using System.Collections.Generic;

namespace Chubberino.Client.Extensions
{
    public static class EnumerableExtensions
    {
        public static String ToLineDelimitedString<T>(this IEnumerable<T> source, Int32 tabCount)
        {
            String delimiter = '\n' + new String('\t', tabCount);
            return String.Join(delimiter, source);
        }
    }
}
