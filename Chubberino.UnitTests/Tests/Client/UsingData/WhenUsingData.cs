using Chubberino.Client;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.UsingData
{
    public sealed class WhenUsingData
    {
        [Fact]
        public void InvisibleCharacterIsNotASpace()
        {
            Assert.NotEqual(' ', Data.InvisibleCharacter);
        }

        [Fact]
        public void InvisibleCharacterShoulCountAsToken()
        {
            Assert.Equal(2, $"{Data.InvisibleCharacter} {Data.InvisibleCharacter}".Split(" ").Length);
            Assert.Equal(3, $"x x {Data.InvisibleCharacter}".Split(" ").Length);
        }
    }
}
