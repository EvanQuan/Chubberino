using System;
using System.Collections.Generic;
using System.Linq;

namespace Chubberino.Bots.Common.Commands.Settings.Pyramids
{
    public sealed class PyramidBuilder
    {
        public IEnumerable<String> GetPyramid(String block, Int32 height)
        {
            var pyramid = new List<String>();

            Int32 currentPyramidHeight = 0;

            // Build pyramid up
            while (currentPyramidHeight < height)
            {
                String message = String.Join(' ', Enumerable.Repeat(block, ++currentPyramidHeight));
                pyramid.Add(message);
            }

            // Build pyramid down
            while (currentPyramidHeight > 0)
            {
                String message = String.Join(' ', Enumerable.Repeat(block, --currentPyramidHeight));
                pyramid.Add(message);
            }

            return pyramid;
        }
    }
}
