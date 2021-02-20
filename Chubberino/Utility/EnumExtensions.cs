using System;

namespace Chubberino.Utility
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Get the next enum value.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TEnum Next<TEnum>(this TEnum source)
            where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum) { throw new ArgumentException($"Argument {typeof(TEnum).FullName} is not an Enum"); }

            TEnum[] array = (TEnum[])Enum.GetValues(source.GetType());

            int index = Array.IndexOf(array, source) + 1;

            return (array.Length == index) ? array[0] : array[index];
        }

    }
}
