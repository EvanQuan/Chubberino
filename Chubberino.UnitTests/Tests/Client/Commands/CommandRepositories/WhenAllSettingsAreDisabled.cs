using Chubberino.Client.Abstractions;
using System.Collections.Generic;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Commands.CommandRepositories
{
    public sealed class WhenAllSettingsAreDisabled : UsingCommandRepository
    {
        /// <summary>
        /// All the settings are already disabled, so there is nothing to do.
        /// </summary>
        [Fact]
        public void ShouldDoNothing()
        {
            IEnumerable<ISetting> settings = Sut.Settings;

            Sut.DisableAllSettings();

            Assert.All(settings, setting => Assert.False(setting.IsEnabled));
        }
    }
}
