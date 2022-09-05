using System;
using Chubberino.Bots.Common.Commands.Settings.Pyramids;

namespace Chubberino.UnitTests.Tests.Client.Commands.Pyramids.PyramidTrackers;

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
