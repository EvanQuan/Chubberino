using System;
using System.Collections.Generic;
using System.Linq;
using Chubberino.Infrastructure.Commands.Settings;
using Moq;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Commands.CommandRepositories
{
    public sealed class WhenGettingProperty : UsingCommandRepository
    {
        private Mock<ISetting> MockedSetting { get; }
        public WhenGettingProperty()
        {
            MockedSetting = new Mock<ISetting>().SetupAllProperties();
            MockedSetting
                .Setup(x => x.Name)
                .Returns(Guid.NewGuid().ToString());

            Sut.AddCommand(MockedSetting.Object);
        }

        /// <summary>
        /// Should output a message indicating the command was not found.
        /// </summary>
        /// <param name="arguments"></param>
        [Theory]
        [InlineData("a", "1")]
        [InlineData("a", "b")]
        [InlineData("s", "a")]
        public void ShouldOutputCommandNotFoundMessage(params String[] arguments)
        {
            String invalidCommandName = Guid.NewGuid().ToString();
            String propertyName = arguments[0];
            List<String> commandWithArguments = new() { invalidCommandName };
            commandWithArguments.AddRange(arguments);

            Sut.Execute("get", commandWithArguments);

            MockedWriter.Verify(x => x.WriteLine($"Command \"{invalidCommandName}\" not found to get."), Times.Once());
        }

        [Theory]
        [InlineData("message", "a")]
        [InlineData("message", "a b")]
        public void ShouldOutputPropertyGetMessage(params String[] arguments)
        {
            MockedSetting
                .Setup(x => x.Get(It.IsAny<IEnumerable<String>>()))
                .Returns("valid");

            String validCommandName = MockedSetting.Object.Name;
            String propertyName = arguments[0];
            IEnumerable<String> propertyValue = arguments.Skip(1);
            List<String> commandWithArguments = new() { validCommandName };
            commandWithArguments.AddRange(arguments);

            Sut.Execute("get", commandWithArguments);

            MockedWriter.Verify(x => x.WriteLine($"Command \"{validCommandName}\" value \"{String.Join(" ", arguments)}\" is \"valid\"."), Times.Once());
        }
    }
}
