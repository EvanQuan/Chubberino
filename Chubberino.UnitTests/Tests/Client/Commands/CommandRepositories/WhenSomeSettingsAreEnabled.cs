using Chubberino.Client.Abstractions;
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
            IEnumerable<ISetting> settings = Sut.GetSettings();

            settings.First().IsEnabled = true;

            Assert.Contains(settings, x => x.IsEnabled);

            Sut.DisableAllSettings();

            Assert.All(settings, setting => Assert.False(setting.IsEnabled));
        }
    }
}
