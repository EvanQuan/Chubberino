using System;
using System.Collections.Generic;
using System.Linq;

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

        public static IEnumerable<String> GetPermutations(this String source)
        {
            var permutations = new List<String>();

            if (source == String.Empty)
            {
                return permutations.Append(String.Empty);
            }

            Int32 maxRecursionDepth = source.Length - 1;

            Char[] array = source.ToCharArray();

            array.GetPermutationHelper(permutations, 0, maxRecursionDepth);

            return permutations;
        }

        private static void GetPermutationHelper(this Char[] source, IList<String> permutations, Int32 currentRecursionDepth, Int32 maxRecursionDepth)
        {
            if (currentRecursionDepth == maxRecursionDepth)
            {
                permutations.Add(String.Join(String.Empty, source));
                return;
            }

            for (Int32 i = currentRecursionDepth; i <= maxRecursionDepth; i++)
            {
                Swap(ref source[currentRecursionDepth], ref source[i]);
                source.GetPermutationHelper(permutations, currentRecursionDepth + 1, maxRecursionDepth);
                Swap(ref source[currentRecursionDepth], ref source[i]);
            }
        }

        public static void Swap(ref Char a, ref Char b)
        {
            if (a == b) return;

            var temp = a;
            a = b;
            b = temp;
        }
    }
}
