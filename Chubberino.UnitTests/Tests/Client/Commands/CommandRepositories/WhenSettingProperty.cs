using Chubberino.Client.Commands.Settings;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Commands.CommandRepositories
{
    /// <summary>
    /// When setting an invalid property on a valid command
    /// </summary>
    public sealed class WhenSettingProperty : UsingCommandRepository
    {
        /// <summary>
        /// A property is not set if the value being set is invalid, or if the
        /// property cannot be found.
        /// </summary>
        /// <param name="arguments"></param>
        [Theory]
        // Invalid property
        [InlineData("a", "1")]
        // Invalid property and value
        [InlineData("a", "b")]
        // Invalid value
        [InlineData("s", "a")]
        public void ShouldOutputPropertyNotSetMessage(params String[] arguments)
        {
            String validCommandName = new AutoChat(MockedClient.Object, MockedConsole.Object).Name;
            String propertyName = arguments[0];
            List<String> commandWithArguments = new List<String>() { validCommandName };
            commandWithArguments.AddRange(arguments);

            Sut.Execute("set", commandWithArguments);

            MockedConsole.Verify(x => x.WriteLine($"Command \"{validCommandName}\" property \"{propertyName}\" not set."), Times.Once());
        }
    }
}
