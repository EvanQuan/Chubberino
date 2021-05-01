using Chubberino.Client.Commands;
using Chubberino.Client.Commands.Settings;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Commands.CommandRepositories
{
    public sealed class WhenGettingSettings : UsingCommandRepository
    {
        [Fact]
        public void ShouldReturnNothing()
        {
            IEnumerable<ISetting> settings = Sut.Settings;

            Assert.Empty(settings);
        }

        [Fact]
        public void ShouldReturnSetting()
        {
            var mockedSetting = new Mock<ISetting>().SetupAllProperties();
            var mockedCommand = new Mock<ICommand>().SetupAllProperties();

            Sut.AddCommand(mockedSetting.Object);
            Sut.AddCommand(mockedCommand.Object);

            IEnumerable<ISetting> settings = Sut.Settings;

            Assert.Contains(mockedSetting.Object, settings);
            Assert.Single(settings);
        }
    }
}
