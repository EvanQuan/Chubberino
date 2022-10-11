using Chubberino.Infrastructure.Client;

namespace Chubberino.UnitTests.Tests.Client.UsingData;

public sealed class WhenUsingData
{
    [Fact]
    public void InvisibleCharacterIsNotASpace()
        => Data.InvisibleCharacter.Should().NotBe(' ');

    [Fact]
    public void InvisibleCharacterShoulCountAsToken()
    {
        Assert.Equal(2, $"{Data.InvisibleCharacter} {Data.InvisibleCharacter}".Split(" ").Length);
        Assert.Equal(3, $"x x {Data.InvisibleCharacter}".Split(" ").Length);
    }
}
