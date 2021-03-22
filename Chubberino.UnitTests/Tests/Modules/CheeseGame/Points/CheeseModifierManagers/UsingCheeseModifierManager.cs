using Chubberino.Modules.CheeseGame.Points;
using Chubberino.UnitTests.Utility;
using Moq;
using System;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Points.CheeseModifierManagers
{
    public abstract class UsingCheeseModifierManager
    {
        protected CheeseModifierManager Sut { get; }

        protected Mock<Random> MockedRandom { get; }

        protected UsingCheeseModifierManager()
        {
            MockedRandom = new Mock<Random>().SetupThrowExceptionOnNegative();

            Sut = new CheeseModifierManager(MockedRandom.Object);
        }
    }
}
