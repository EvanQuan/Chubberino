using System;

namespace Chubberino.Utility
{
    public static class DoubleExtensions
    {
        public static Double Max(this Double source, Double other)
        {
            return Math.Max(source, other);
        }

        public static Double Min(this Double source, Double other)
        {
            return Math.Min(source, other);
        }
    }
}
