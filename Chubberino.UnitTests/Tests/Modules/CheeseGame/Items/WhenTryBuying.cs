﻿using Chubberino.Modules.CheeseGame.Items;
using Chubberino.Modules.CheeseGame.Models;
using Monad;
using System;
using Xunit;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Items
{
    public sealed class WhenTryBuying
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void ShouldReturnErrorMessage(Int32 quantity)
        {
            var item = new ErrorItem();

            Either<BuyResult, String> result = item.TryBuy(quantity, new Player());

            Assert.True(result.IsRight());
            Assert.Equal(ErrorItem.ErrorMessage, result.Right());
        }

        [Theory]
        [InlineData(1, 0, 2)]
        [InlineData(1, 1, 1)]
        [InlineData(2, 0, 2)]
        [InlineData(2, 1, 1)]
        [InlineData(3, 0, 2)]
        [InlineData(3, 1, 1)]
        public void ShouldReturnNotEnoughPointsErrorMessageForStaticPriceItem(Int32 quantity, Int32 initialPoints, Int32 expectedPointsNeeded)
        {
            var item = new StaticPriceItem();

            var player = new Player() { Points = initialPoints };

            Either<BuyResult, String> result = item.TryBuy(quantity, player);

            Assert.True(result.IsRight());
            Assert.Equal(String.Format(Item.NotEnoughPointsErrorMessage, expectedPointsNeeded, item.GetSpecificNameForNotEnoughToBuy(player)), result.Right());
        }

        [Theory]
        [InlineData(1, 0, 2)]
        [InlineData(1, 1, 1)]
        [InlineData(2, 0, 2)]
        [InlineData(2, 1, 1)]
        [InlineData(3, 0, 2)]
        [InlineData(3, 1, 1)]
        public void ShouldReturnNotEnoughPointsErrorMessageForIncrementingPriceItem(Int32 quantity, Int32 initialPoints, Int32 expectedPointsNeeded)
        {
            var item = new IncrementingPriceItem();

            var player = new Player() { Points = initialPoints };

            Either<BuyResult, String> result = item.TryBuy(quantity, player);

            Assert.True(result.IsRight());
            Assert.Equal(String.Format(Item.NotEnoughPointsErrorMessage, expectedPointsNeeded, item.GetSpecificNameForNotEnoughToBuy(player)), result.Right());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void ShouldReturnStaticPrice(Int32 quantity)
        {
            var item = new StaticPriceItem();

            var expectedPointsSpent = quantity * StaticPriceItem.StaticPrice;

            var player = new Player() { Points = expectedPointsSpent };

            Either<BuyResult, String> result = item.TryBuy(quantity, player);

            Assert.True(result.IsLeft());
            Assert.Equal(quantity, result.Left().QuantityPurchased);
            Assert.Equal(expectedPointsSpent, result.Left().PointsSpent);
        }

        [Theory]
        [InlineData(1, IncrementingPriceItem.InitialPrice, 1, IncrementingPriceItem.InitialPrice)]
        [InlineData(2, IncrementingPriceItem.InitialPrice * 2 + 1, 2, IncrementingPriceItem.InitialPrice * 2 + 1)]
        [InlineData(2, IncrementingPriceItem.InitialPrice, 1, IncrementingPriceItem.InitialPrice)]
        [InlineData(3, IncrementingPriceItem.InitialPrice * 3 + 3, 3, IncrementingPriceItem.InitialPrice * 3 + 3)]
        [InlineData(3, IncrementingPriceItem.InitialPrice * 2 + 2, 2, IncrementingPriceItem.InitialPrice * 2 + 1)]
        public void ShouldReturnIncrementingPrice(Int32 quantity, Int32 points, Int32 expectedQuantity, Int32 expectedPointsSpent)
        {
            var item = new IncrementingPriceItem();

            var player = new Player() { Points = points };

            Either<BuyResult, String> result = item.TryBuy(quantity, player);

            Assert.True(result.IsLeft());
            Assert.Equal(expectedQuantity, result.Left().QuantityPurchased);
            Assert.Equal(expectedPointsSpent, result.Left().PointsSpent);
        }

        [Theory]
        [InlineData(1, IncrementingQuantityItem.StaticPrice, IncrementingQuantityItem.InitialQuantity, IncrementingQuantityItem.StaticPrice * 1)]
        [InlineData(2, IncrementingQuantityItem.StaticPrice * 2, IncrementingQuantityItem.InitialQuantity * 2 + 1, IncrementingQuantityItem.StaticPrice * 2)]
        [InlineData(3, IncrementingQuantityItem.StaticPrice * 3, IncrementingQuantityItem.InitialQuantity * 3 + 3, IncrementingQuantityItem.StaticPrice * 3)]
        public void ShouldReturnIncrementingQuantity(Int32 quantity, Int32 points, Int32 expectedQuantity, Int32 expectedPointsSpent)
        {
            var item = new IncrementingQuantityItem();

            var player = new Player() { Points = points };

            Either<BuyResult, String> result = item.TryBuy(quantity, player);

            Assert.True(result.IsLeft());
            Assert.Equal(expectedQuantity, result.Left().QuantityPurchased);
            Assert.Equal(expectedPointsSpent, result.Left().PointsSpent);
        }
    }
}
