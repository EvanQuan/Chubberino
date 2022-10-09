using TwitchLib.Client.Enums;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.ColorSelectors.PresetColorSelectors;

public sealed class WhenGettingNextColor : UsingPresetColorSelector
{
    [Theory]
    [InlineData(0, ChatColorPresets.Blue)]
    [InlineData(1, ChatColorPresets.Coral)]
    [InlineData(2, ChatColorPresets.DodgerBlue)]
    [InlineData(3, ChatColorPresets.SpringGreen)]
    [InlineData(4, ChatColorPresets.YellowGreen)]
    [InlineData(5, ChatColorPresets.Green)]
    [InlineData(6, ChatColorPresets.OrangeRed)]
    [InlineData(7, ChatColorPresets.Red)]
    [InlineData(8, ChatColorPresets.GoldenRod)]
    [InlineData(9, ChatColorPresets.HotPink)]
    [InlineData(10, ChatColorPresets.CadetBlue)]
    [InlineData(11, ChatColorPresets.SeaGreen)]
    [InlineData(12, ChatColorPresets.Chocolate)]
    [InlineData(13, ChatColorPresets.BlueViolet)]
    [InlineData(14, ChatColorPresets.Firebrick)]
    public void ShouldReturnCorrectColor(Int32 index, ChatColorPresets expectedColor)
    {
        MockedRandom.Setup(x => x
            .Next(Enum.GetValues(typeof(ChatColorPresets)).Length))
            .Returns(index);

        String actualColor = Sut.GetNextColor();

        Assert.Equal(expectedColor.ToString(), actualColor);
    }
}
