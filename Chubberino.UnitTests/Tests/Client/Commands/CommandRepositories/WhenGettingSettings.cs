using Chubberino.Client.Abstractions;
using System.Collections.Generic;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Commands.CommandRepositories
{
    public sealed class WhenGettingSettings : UsingCommandRepository
    {

        [Fact]
        public void ShouldReturnNothing()
        {
            IEnumerable<ISetting> settings = Sut.GetSettings();

            Assert.Empty(settings);
        }
    }
}
