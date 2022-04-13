using System;
using System.Collections.Generic;
using System.Linq;

namespace Chubberino.Bots.Common.Commands.Settings.Pyramids;

public static class PyramidBuilder
{
    public static IEnumerable<String> Get(String block, Int32 height)
    {
        Int32 currentPyramidHeight = 0;

        // Build pyramid up
        while (currentPyramidHeight < height)
        {
            yield return String.Join(' ', Enumerable.Repeat(block, ++currentPyramidHeight));
        }

        // Build pyramid down
        while (currentPyramidHeight > 0)
        {
            yield return String.Join(' ', Enumerable.Repeat(block, --currentPyramidHeight));
        }
    }
}
