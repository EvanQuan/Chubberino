using Chubberino.Client.Abstractions;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Commands.CommandRepositories
{
    public sealed class WhenSomeSettingsAreEnabled : UsingCommandRepository
    {
        /// <summary>
        /// The settings are were enabled should be disabled, and the disabled
        /// commands should remain disabled.
        /// </summary>
        [Fact]
        public void ShouldDisableAllSettings()
        {
            var setting1 = new Mock<ISetting>().SetupAllProperties();
            var setting2 = new Mock<ISetting>().SetupAllProperties();
            var command = new Mock<ICommand>().SetupAllProperties();

            Sut.AddCommand(setting1.Object);
            Sut.AddCommand(command.Object);
            Sut.AddCommand(setting2.Object);

            IEnumerable<ISetting> settings = Sut.GetSettings();

            settings.First().IsEnabled = true;

            Assert.Contains(settings, x => x.IsEnabled);

            Sut.DisableAllSettings();

            Assert.All(settings, setting => Assert.False(setting.IsEnabled));
        }
    }
}
