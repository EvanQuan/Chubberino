using Chubberino.Database.Contexts;
using Moq;

namespace Chubberino.UnitTests.Utility
{
    public static class MockedIApplicationContextFactoryExtensions
    {
        public static Mock<IApplicationContext> SetupContext(this Mock<IApplicationContextFactory> source)
        {
            Mock<IApplicationContext> context = new();

            source.Setup(x => x.GetContext()).Returns(context.Object);

            return context;
        }
    }
}
