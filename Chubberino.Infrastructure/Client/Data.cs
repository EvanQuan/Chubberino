using System;

namespace Chubberino.Infrastructure.Client;

public static class Data
{
    /// <summary>
    /// Empty character to evade identical message detection.
    /// </summary>
    public const Char InvisibleCharacter = '⠀';

    /// <summary>
    /// <see cref="InvisibleCharacter"/> prepended with a space.
    /// </summary>
    public static String SpaceWithInvisibleCharacter { get; } = " " + InvisibleCharacter;
}
