using System.Collections.Generic;
using System.Linq;
using Chubberino.Common.ValueObjects;

namespace Chubberino.UnitTests.Tests.Client.Commands.CommandRepositories;

/// <summary>
/// When setting an invalid property on a valid command
/// </summary>
public sealed class WhenSettingProperty : UsingCommandRepository
{
    public WhenSettingProperty()
    {
        Sut.AddCommand(MockedSetting1.Object);
    }

    /// <summary>
    /// Should output a message indicating the property failed to be set.
    /// A property is not set if the value being set is invalid, or if the
    /// property cannot be found.
    /// </summary>
    /// <param name="arguments"></param>
    [Theory]
    // Invalid property
    [InlineData("invalid", "valid")]
    // Invalid property and value
    [InlineData("invalid", "invalid")]
    // Invalid value
    [InlineData("valid", "invalid")]
    public void ShouldOutputPropertyNotSetMessage(params String[] arguments)
    {
        MockedSetting1
            .Setup(x => x.Set(It.IsAny<String>(), It.IsAny<IEnumerable<String>>()))
            .Returns((String property, IEnumerable<String> args) =>
            {
                return property == "valid" && "valid" == args.FirstOrDefault();
            });

        var validCommandName = MockedSetting1.Object.Name;
        String propertyName = arguments[0];
        List<String> commandWithArguments = new() { validCommandName.Value };
        commandWithArguments.AddRange(arguments);

        Sut.Execute("set", commandWithArguments);

        MockedWriter.Verify(x => x.WriteLine($"Command \"{validCommandName.Value}\" property \"{propertyName}\" not set."), Times.Once());
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

        Sut.Execute("set", commandWithArguments);

        MockedWriter.Verify(x => x.WriteLine($"Command \"{invalidCommandName}\" not found to set."), Times.Once());
    }

    [Theory]
    [InlineData("message", "a")]
    [InlineData("message", "a b")]
    public void ShouldOutputPropertySetMessage(params String[] arguments)
    {
        MockedSetting1
            .Setup(x => x.Set(It.IsAny<String>(), It.IsAny<IEnumerable<String>>()))
            .Returns(true);

        var validCommandName = MockedSetting1.Object.Name;
        String propertyName = arguments[0];
        IEnumerable<String> propertyValue = arguments.Skip(1);
        List<String> commandWithArguments = new() { validCommandName.Value };
        commandWithArguments.AddRange(arguments);

        Sut.Execute("set", commandWithArguments);

        MockedWriter.Verify(x => x.WriteLine($"Command \"{validCommandName.Value}\" property \"{propertyName}\" set to \"{String.Join(" ", propertyValue)}\"."), Times.Once());
    }
}
