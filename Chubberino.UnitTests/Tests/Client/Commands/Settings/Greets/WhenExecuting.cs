﻿namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.Greets;

/// <summary>
/// When calling <see cref="Greet.Execute(IEnumerable{String})"/>
/// </summary>
public sealed class WhenExecuting : UsingGreet
{
    /// <summary>
    /// Should set the greeting to the <paramref name="arguments"/>.
    /// </summary>
    /// <param name="arguments"></param>
    [Theory]
    [InlineData("a")]
    [InlineData("a", "b")]
    public void ShouldSetGreeting(params String[] arguments)
    {
        Sut.Execute(arguments);

        String greeting = String.Join(" ", arguments);

        MockedWriter.Verify(x => x.WriteLine($"Greeting message is \"{greeting}\"."), Times.Once());
    }

    /// <summary>
    /// Should disable the setting.
    /// </summary>
    [Fact]
    public void Disable()
    {
        Sut.Execute(Array.Empty<String>());

        MockedWriter.Verify(x => x.WriteLine(It.IsAny<String>()), Times.Never());

        Assert.False(Sut.IsEnabled);
    }
}
