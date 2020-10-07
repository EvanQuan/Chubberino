using Chubberino.Client.Commands.Settings.Pyramids;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chubberino.UnitTests.Tests.Client.Commands.Pyramids
{
    public abstract class UsingPyramidTracker
    {
        protected PyramidTracker Sut { get; }

        public UsingPyramidTracker()
        {
            Sut = new PyramidTracker();
        }
    }
}
