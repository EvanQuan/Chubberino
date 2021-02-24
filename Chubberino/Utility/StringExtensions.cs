using System;

namespace Chubberino.Utility
{
    public static class StringExtensions
    {
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

            return trimmedSource.Substring(firstSpaceIndex + 1);
        }
    }
}
