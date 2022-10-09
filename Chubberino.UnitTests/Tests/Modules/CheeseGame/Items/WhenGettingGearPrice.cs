using Chubberino.Bots.Channel.Modules.CheeseGame.Items;
using Chubberino.Database.Models;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Items;

public sealed class WhenGettingGearPrice
{
    public Player Player { get; }

    public WhenGettingGearPrice()
    {
        Player = new Player();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(19)]
    public void ShouldReturn1(Int32 gearCount)
    {
        Player.GearCount = gearCount;

        var price = Player.GetGearPrice();

        Assert.Equal(1, price);
    }

    [Theory]
    [InlineData(20, 1 + 1 * 1)]
    [InlineData(21, 1 + 1 * 2)]
    [InlineData(22, 1 + 1 * 3)]
    [InlineData(29, 1 + 1 * 10)]
    [InlineData(30, 1 + 1 * 11)]
    [InlineData(39, 1 + 1 * 20)]
    public void ShouldReturn1ExtraPerAdditionalGear(Int32 gearCount, Int32 expectedPrice)
    {
        Player.GearCount = gearCount;

        var price = Player.GetGearPrice();

        Assert.Equal(expectedPrice, price);
    }

    [Theory]
    [InlineData(40, 1 + 1 * 20 + 2 * 1)]
    [InlineData(41, 1 + 1 * 20 + 2 * 2)]
    public void ShouldReturn2ExtraPerAdditionalGear(Int32 gearCount, Int32 expectedPrice)
    {
        Player.GearCount = gearCount;

        var price = Player.GetGearPrice();

        Assert.Equal(expectedPrice, price);
    }
}
