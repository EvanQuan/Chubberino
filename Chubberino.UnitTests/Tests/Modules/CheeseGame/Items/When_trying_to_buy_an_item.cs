using Chubberino.Bots.Channel.Modules.CheeseGame.Items;
using Chubberino.Database.Models;
using LanguageExt;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Items;

public sealed class When_trying_to_buy_an_item
{
    public sealed class Given_the_item_does_not_have_a_price
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Then_an_error_message_is_returned(Int32 quantity)
        {
            var player = new Player
            {
                Points = 10
            };
            var item = new ErrorItem();

            Either<BuyResult, String> result = item.TryBuy(quantity, player);

            result.IsRight.Should().BeTrue();
            result.IfRight(message =>
                message.Should().Be(ErrorItem.ErrorMessage));
            player.Points.Should().Be(10);
        }
    }

    public sealed class Given_the_item_has_a_static_price
    {
        public sealed class Given_the_player_has_fewer_points_than_the_item_price
        {
            [Theory]
            [InlineData(1, 0, 2)]
            [InlineData(1, 1, 1)]
            [InlineData(2, 0, 2)]
            [InlineData(2, 1, 1)]
            [InlineData(3, 0, 2)]
            [InlineData(3, 1, 1)]
            public void Then_not_enough_points_error_message_is_returned(
                Int32 quantity,
                Int32 initialPoints,
                Int32 expectedPointsNeeded)
            {
                var item = new StaticPriceItem();
                var player = new Player() { Points = initialPoints };
                var expectedMessage = String.Format(Item.NotEnoughPointsErrorMessage, expectedPointsNeeded, item.GetSpecificNameForNotEnoughToBuy(player));

                Either<BuyResult, String> result = item.TryBuy(quantity, player);

                Assert.True(result.IsRight);
                result.IfRight(message =>
                    message.Should().Be(expectedMessage));
            }
        }

        public sealed class Given_the_player_has_points_that_equal_or_exceed_the_item_price
        {
            [Theory]
            [InlineData(1)]
            [InlineData(2)]
            [InlineData(3)]
            public void Then_the_buy_result_is_returned(Int32 expectedQuantity)
            {
                var item = new StaticPriceItem();
                var expectedPointsSpent = expectedQuantity * StaticPriceItem.StaticPrice;
                var player = new Player() { Points = expectedPointsSpent };

                Either<BuyResult, String> result = item.TryBuy(expectedQuantity, player);

                result.IsLeft.Should().BeTrue();
                result.IfLeft(buyResult =>
                {
                    buyResult.QuantityPurchased.Should().Be(expectedQuantity);
                    buyResult.PointsSpent.Should().Be(expectedPointsSpent);
                });
            }
        }
    }

    public sealed class Given_the_item_has_an_incrementing_price
    {
        public sealed class Given_the_player_has_fewer_points_than_the_item_price
        {
            [Theory]
            [InlineData(1, 0, 2)]
            [InlineData(1, 1, 1)]
            [InlineData(2, 0, 2)]
            [InlineData(2, 1, 1)]
            [InlineData(3, 0, 2)]
            [InlineData(3, 1, 1)]
            public void Then_not_enough_points_error_message_is_returned(
                Int32 quantity,
                Int32 initialPoints,
                Int32 expectedPointsNeeded)
            {
                var item = new IncrementingPriceItem();
                var player = new Player() { Points = initialPoints };
                var expectedMessage = String.Format(Item.NotEnoughPointsErrorMessage, expectedPointsNeeded, item.GetSpecificNameForNotEnoughToBuy(player));

                Either<BuyResult, String> result = item.TryBuy(quantity, player);


                Assert.True(result.IsRight);
                result.IsRight.Should().BeTrue();
                result.IfRight(message => 
                    message.Should().Be(expectedMessage));
            }
        }

        public sealed class Given_the_player_has_points_that_equal_or_exceed_the_item_price
        {
            [Theory]
            [InlineData(1, IncrementingPriceItem.InitialPrice, 1, IncrementingPriceItem.InitialPrice)]
            [InlineData(2, IncrementingPriceItem.InitialPrice * 2 + 1, 2, IncrementingPriceItem.InitialPrice * 2 + 1)]
            [InlineData(2, IncrementingPriceItem.InitialPrice, 1, IncrementingPriceItem.InitialPrice)]
            [InlineData(3, IncrementingPriceItem.InitialPrice * 3 + 3, 3, IncrementingPriceItem.InitialPrice * 3 + 3)]
            [InlineData(3, IncrementingPriceItem.InitialPrice * 2 + 2, 2, IncrementingPriceItem.InitialPrice * 2 + 1)]
            public void Then_the_buy_result_is_returned(
                Int32 quantity,
                Int32 points,
                Int32 expectedQuantity,
                Int32 expectedPointsSpent)
            {
                var item = new IncrementingPriceItem();
                var player = new Player() { Points = points };

                Either<BuyResult, String> result = item.TryBuy(quantity, player);

                result.IsLeft.Should().BeTrue();
                result.IfLeft(buyResult =>
                {
                    buyResult.QuantityPurchased.Should().Be(expectedQuantity);
                    buyResult.PointsSpent.Should().Be(expectedPointsSpent);
                });
            }
        }
    }

    public sealed class Given_the_item_has_an_incrementing_quantity
    {
        [Theory]
        [InlineData(1, IncrementingQuantityItem.StaticPrice, IncrementingQuantityItem.InitialQuantity, IncrementingQuantityItem.StaticPrice * 1)]
        [InlineData(2, IncrementingQuantityItem.StaticPrice * 2, IncrementingQuantityItem.InitialQuantity * 2 + 1, IncrementingQuantityItem.StaticPrice * 2)]
        [InlineData(3, IncrementingQuantityItem.StaticPrice * 3, IncrementingQuantityItem.InitialQuantity * 3 + 3, IncrementingQuantityItem.StaticPrice * 3)]
        public void Then_the_buy_result_is_returned(
            Int32 quantity,
            Int32 points,
            Int32 expectedQuantity,
            Int32 expectedPointsSpent)
        {
            var item = new IncrementingQuantityItem();
            var player = new Player() { Points = points };

            Either<BuyResult, String> result = item.TryBuy(quantity, player);

            result.IsLeft.Should().BeTrue();
            result.IfLeft(buyResult =>
            {
                buyResult.QuantityPurchased.Should().Be(expectedQuantity);
                buyResult.PointsSpent.Should().Be(expectedPointsSpent);
            });
        }
    }
}
