using System;
using Chubberino.Infrastructure.Commands.Settings;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.Copies
{
    /// <summary>
    /// When calling <see cref="Copy.Execute(IEnumerable{String})"/>
    /// </summary>
    public sealed class WhenExecuting : UsingCopy
    {
        /// <summary>
        /// Should output the setting properties.
        /// </summary>
        [Theory]
        [InlineData("a", "Mock", "1")]
        [InlineData("b", "Reverse", "2")]
        public void ShouldOutputProperties(params String[] arguments)
        {
            String userToCopy = arguments[0];
            String mode = arguments[1];
            String prefix = arguments[2];

            Sut.Execute(arguments);

            MockedWriter.Verify(x => x.WriteLine($"Copying user \"{userToCopy}\" Mode: \"{mode}\" Prefix: \"{prefix}\""));
        }

        /// <summary>
        /// Should disable setting.
        /// </summary>
        [Fact]
        public void ShouldDisable()
        {
            Assert.Raises<OnSettingStateChangeArgs>(
                x => Sut.OnSettingStateChange += x,
                x => Sut.OnSettingStateChange -= x,
                () =>
                {
                    Sut.Execute(Array.Empty<String>());
                });

            MockedWriter.Verify(x => x.WriteLine("Copy disabled"));
        }
    }
}
