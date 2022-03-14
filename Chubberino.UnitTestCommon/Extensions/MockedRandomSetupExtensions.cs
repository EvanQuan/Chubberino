using System;
using Moq;

namespace Chubberino.UnitTestQualityTools.Extensions
{
    public static class MockedRandomSetupExtensions
    {
        public static Mock<Random> SetupReturnMinimum(this Mock<Random> random)
        {
            random.SetupThrowExceptionOnNegative();
            random.Setup(x => x.Next(It.Is<Int32>(x => x >= 0), It.Is<Int32>(x => x >= 0))).Returns<Int32, Int32>((min, max) => min);
            random.Setup(x => x.Next()).Returns(0);
            random.Setup(x => x.Next(It.Is<Int32>(x => x >= 0))).Returns(0);
            random.Setup(x => x.NextDouble()).Returns(0);

            return random;
        }

        public static Mock<Random> SetupReturnMaximum(this Mock<Random> random)
        {
            random.SetupThrowExceptionOnNegative();
            random.Setup(x => x.Next(It.Is<Int32>(x => x >= 0), It.Is<Int32>(x => x >= 0))).Returns<Int32, Int32>((min, max) => min == max ? min : max - 1);
            random.Setup(x => x.Next()).Returns(0);
            random.Setup(x => x.Next(It.Is<Int32>(x => x >= 0))).Returns<Int32>(x => x == 0 ? x : x - 1);
            random.Setup(x => x.NextDouble()).Returns(1);

            return random;
        }

        public static Mock<Random> SetupThrowExceptionOnNegative(this Mock<Random> random)
        {
            random.Setup(x => x.Next(It.Is<Int32>(x => x < 0), It.IsAny<Int32>())).Throws<ArgumentOutOfRangeException>();
            random.Setup(x => x.Next(It.IsAny<Int32>(), It.Is<Int32>(x => x < 0))).Throws<ArgumentOutOfRangeException>();
            random.Setup(x => x.Next(It.Is<Int32>(x => x < 0))).Throws<ArgumentOutOfRangeException>();

            return random;
        }
    }
}
