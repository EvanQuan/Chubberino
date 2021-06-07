using System;
using System.Collections.Generic;
using System.Text;

namespace Chubberino.Utility
{
    public static class StringExtensions
    {
        public static String Format(this String source, params Object[] args)
        {
            return String.Format(source, args);
        }

        public static String StripStart(this String source, String prefix)
        {
            if (source.StartsWith(prefix))
            {
                return source[prefix.Length..];
            }

            return source;
        }

        public static String StripStart(this String source, Char prefix)
        {
            if (source.StartsWith(prefix))
            {
                return source[1..];
            }

            return source;
        }

        /// <summary>
        /// Gets the first word of the string.
        /// </summary>
        /// <param name="source">Source string to get the first word from.</param>
        /// <param name="word">The first word found.</param>
        /// <returns>The remaining string without the leading space, or <see cref="String.Empty"/> if there is nothing remaining.</returns>
        public static String GetNextWord(this String source, out String word)
        {
            var trimmedSource = source.Trim();
            var firstSpaceIndex = trimmedSource.IndexOf(' ');

            if (firstSpaceIndex < 0)
            {
                word = trimmedSource;
                return String.Empty;
            }

            word = trimmedSource.Substring(0, firstSpaceIndex);

            return trimmedSource[(firstSpaceIndex + 1)..];
        }

        public static Boolean TryParseEnum<TEnum>(this String source, out TEnum value)
            where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum || String.IsNullOrWhiteSpace(source))
            {
                value = default;
                return false;
            }

            var values = (TEnum[])Enum.GetValues(typeof(TEnum));

            foreach (TEnum v in values)
            {
                if (v.ToString().StartsWith(source, StringComparison.OrdinalIgnoreCase))
                {
                    value = v;
                    return true;
                }
            }

            value = default;
            return false;
        }

        /// <summary>
        /// Split the specified <paramref name="source"/> into segments of at
        /// most length of <paramref name="segmentLength"/>, by word.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="segmentLength"></param>
        /// <returns></returns>
        public static IEnumerable<String> SplitByLengthOnWord(this String source, Int32 segmentLength)
        {
            if (source.Length <= segmentLength)
            {
                return new String[] { source };
            }

            StringBuilder justifiedLine = new();
            String[] words = source.Split(' ');

            List<String> segments = new();

            for (Int32 i = 0; i < words.Length; i++)
            {
                justifiedLine.Append(words[i]).Append(' ');
                if (i + 1 == words.Length || justifiedLine.Length + words[i + 1].Length > segmentLength)
                {
                    justifiedLine.Remove(justifiedLine.Length - 1, 1);
                    segments.Add(justifiedLine.ToString());
                    justifiedLine = new StringBuilder();
                }
            }
            return segments;
        }
    }
}
