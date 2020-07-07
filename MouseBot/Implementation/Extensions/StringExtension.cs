using System;
using System.Collections.Generic;
using System.Linq;

namespace MouseBot.Implementation.Extensions
{
    internal static class StringExtension
    {
        private static Random Random { get; } = new Random();

        public static String ToRandomCase(this String source)
        {
            IEnumerable<Char> finalString = source
                .Select(character => Char.IsLetter(character)
                ? (Random.Next() % 2 == 0
                    ? character.ToString().ToLower().First()
                    : character.ToString().ToUpper().First())
                : character);

            return new String(finalString.ToArray());
        }
    }
}
