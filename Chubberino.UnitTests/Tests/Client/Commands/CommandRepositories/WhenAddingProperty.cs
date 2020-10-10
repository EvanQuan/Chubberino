using Chubberino.Client.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Commands.CommandRepositories
{
    public sealed class WhenAddingProperty : UsingCommandRepository
    {
         private Mock<ISetting> MockedSetting { get; }

        public WhenAddingProperty()
        {
            MockedSetting = new Mock<ISetting>().SetupAllProperties();

            MockedSetting
                .Setup(x => x.Name)
                .Returns(Guid.NewGuid().ToString());

            Sut.AddCommand(MockedSetting.Object);
        }

        [Theory]
        // Invalid property
        [InlineData("invalid", "valid")]
        // Invalid property and value
        [InlineData("invalid", "invalid")]
        // Invalid value
        [InlineData("valid", "invalid")]
        public void ShouldOutputPropertyNotSetMessage(params String[] arguments)
        {
            MockedSetting
                .Setup(x => x.Add(It.IsAny<String>(), It.IsAny<IEnumerable<String>>()))
                .Returns((String property, IEnumerable<String> args) =>
                {
                    return property == "valid" && "valid" == args.FirstOrDefault();
                });

            String validCommandName = MockedSetting.Object.Name;
            String propertyName = arguments[0];
            List<String> commandWithArguments = new List<String>() { validCommandName };
            commandWithArguments.AddRange(arguments);

            Sut.Execute("add", commandWithArguments);

            MockedConsole.Verify(x => x.WriteLine($"Command \"{validCommandName}\" property \"{propertyName}\" not added to."), Times.Once());
        }

        [Theory]
        [InlineData("a", "1")]
        [InlineData("a", "b")]
        [InlineData("s", "a")]
        public void ShouldOutputCommandNotFoundMessage(params String[] arguments)
        {
            String invalidCommandName = Guid.NewGuid().ToString();
            String propertyName = arguments[0];
            List<String> commandWithArguments = new List<String>() { invalidCommandName };
            commandWithArguments.AddRange(arguments);

            Sut.Execute("add", commandWithArguments);

            MockedConsole.Verify(x => x.WriteLine($"Command \"{invalidCommandName}\" not found to add to."), Times.Once());
        }

        [Theory]
        [InlineData("message", "a")]
        [InlineData("message", "a b")]
        public void ShouldOutputPropertyRemovedMessage(params String[] arguments)
        {
            MockedSetting
                .Setup(x => x.Add(It.IsAny<String>(), It.IsAny<IEnumerable<String>>()))
                .Returns(true);

            String validCommandName = MockedSetting.Object.Name;
            String propertyName = arguments[0];
            IEnumerable<String> propertyValue = arguments.Skip(1);
            List<String> commandWithArguments = new List<String>() { validCommandName };
            commandWithArguments.AddRange(arguments);

            Sut.Execute("add", commandWithArguments);

            MockedConsole.Verify(x => x.WriteLine($"Command \"{validCommandName}\" property \"{propertyName}\" added \"{String.Join(" ", propertyValue)}\"."), Times.Once());
        }
    }
}
