using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Commands.CommandRepositories
{
    public sealed class WhenGettingSettings : UsingCommandRepository
    {
        [Fact]
        public void ShouldReturnNothing()
        {
            Assert.Empty(Sut.Settings.Disabled);
            Assert.Empty(Sut.Settings.Enabled);
        }

        [Fact]
        public void ShouldReturnSetting()
        {
            Sut.AddCommand(MockedSetting1.Object);
            Sut.AddCommand(MockedCommand.Object);

            Assert.Contains(MockedSetting1.Object, Sut.Settings.Disabled);
            Assert.Single(Sut.Settings.Disabled);
        }
    }
}
