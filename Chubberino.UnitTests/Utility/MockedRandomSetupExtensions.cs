using Moq;
using System;

namespace Chubberino.UnitTests.Utility
{
    public static class MockedRandomSetupExtensions
    {
        public static Mock<Random> SetupReturnMinimum(this Mock<Random> random)
        {
            random.SetupThrowOutOfRangeExceptionOnNegative();
            random.Setup(x => x.Next(It.Is<Int32>(x => x >= 0), It.Is<Int32>(x => x >= 0))).Returns<Int32, Int32>((min, max) => min);
            random.Setup(x => x.Next()).Returns(0);
            random.Setup(x => x.Next(It.Is<Int32>(x => x >= 0))).Returns(0);
            random.Setup(x => x.NextDouble()).Returns(0);

            return random;
        }

        public static Mock<Random> SetupReturnMaximum(this Mock<Random> random)
        {
            random.SetupThrowOutOfRangeExceptionOnNegative();
            random.Setup(x => x.Next(It.Is<Int32>(x => x >= 0), It.Is<Int32>(x => x >= 0))).Returns<Int32, Int32>((min, max) => max);
            random.Setup(x => x.Next()).Returns(0);
            random.Setup(x => x.Next(It.Is<Int32>(x => x >= 0))).Returns<Int32>(x => x);
            random.Setup(x => x.NextDouble()).Returns(1);

            return random;
        }

        public static Mock<Random> SetupThrowOutOfRangeExceptionOnNegative(this Mock<Random> random)
        {
            random.Setup(x => x.Next(It.Is<Int32>(x => x < 0), It.IsAny<Int32>())).Throws<ArgumentOutOfRangeException>();
            random.Setup(x => x.Next(It.IsAny<Int32>(), It.Is<Int32>(x => x < 0))).Throws<ArgumentOutOfRangeException>();
            random.Setup(x => x.Next(It.Is<Int32>(x => x < 0))).Throws<ArgumentOutOfRangeException>();

            return random;
        }
    }
}
