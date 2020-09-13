using Chubberino.Client.Commands.Settings;
using System;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.Greets
{
    public sealed class WhenSetting : UsingGreet
    {
        [Theory]
        [InlineData("m", new String[] { "default" }, Greet.Mode.Default)]
        [InlineData("M", new String[] { "DeFaUlt" }, Greet.Mode.Default)]
        [InlineData("m", new String[] { "" }, Greet.Mode.Default)]
        [InlineData("M", new String[] { }, Greet.Mode.Default)]
        [InlineData("m", new String[] { "wholesome" }, Greet.Mode.Wholesome)]
        [InlineData("M", new String[] { "WhOlEsOmE" }, Greet.Mode.Wholesome)]
        [InlineData("m", new String[] { "w" }, Greet.Mode.Wholesome)]
        [InlineData("M", new String[] { "W" }, Greet.Mode.Wholesome)]
        [InlineData("mode", new String[] { "default" }, Greet.Mode.Default)]
        [InlineData("mOdE", new String[] { "DeFaUlt" }, Greet.Mode.Default)]
        [InlineData("mode", new String[] { "" }, Greet.Mode.Default)]
        [InlineData("mode", new String[] { }, Greet.Mode.Default)]
        [InlineData("mode", new String[] { "wholesome" }, Greet.Mode.Wholesome)]
        [InlineData("MoDe", new String[] { "WhOlEsOmE" }, Greet.Mode.Wholesome)]
        [InlineData("mode", new String[] { "w" }, Greet.Mode.Wholesome)]
        [InlineData("mOdE", new String[] { "W" }, Greet.Mode.Wholesome)]
        public void ShouldSetMode(String property, String[] arguments, Greet.Mode expectedMode)
        {
            Sut.Set(property, arguments);

            Assert.Equal(expectedMode, Sut.CurrentMode);
        }
    }
}
