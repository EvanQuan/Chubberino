using System;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.Copies
{
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

            MockedConsole.Verify(x => x.WriteLine($"Copying user \"{userToCopy}\" Mode: \"{mode}\" Prefix: \"{prefix}\""));
        }
    }
}
