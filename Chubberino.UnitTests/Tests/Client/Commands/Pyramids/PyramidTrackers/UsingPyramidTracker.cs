using Chubberino.Client.Commands.Settings.PyramidTrackers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chubberino.UnitTests.Tests.Client.Commands.Pyramids
{
    public abstract class UsingPyramidTracker
    {
        protected const String ExpectedName1 = "1";
        protected const String ExpectedName2 = "2";
        protected const String ExpectedBlock1 = "a";
        protected const String ExpectedBlock2 = "b";

        protected PyramidTracker Sut { get; }

        public UsingPyramidTracker()
        {
            Sut = new PyramidTracker();
        }
    }
}
