namespace Chubberino.Common.Extensions;

public static class DoubleExtensions
{
    public static Double Max(this in Double source, in Double other)
    {
        return Math.Max(source, other);
    }

    public static Double Min(this in Double source, in Double other)
    {
        return Math.Min(source, other);
    }
}
