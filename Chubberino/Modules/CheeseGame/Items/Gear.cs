﻿using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.PlayerExtensions;
using Monad;
using System;
using System.Collections.Generic;

namespace Chubberino.Modules.CheeseGame.Items
{
    public sealed class Gear : Item
    {
        public const Int32 MaximumCount = 150;

        public override IEnumerable<String> Names { get; } = new String[] { "Gear", "g", "gears" };

        public override Int32 GetPrice(Player player)
        {
            return player.GetGearPrice();
        }

        public override String GetSpecificNameForNotEnoughToBuy(Player player)
        {
            return "some gear";
        }

        public override String GetSpecificNameForSuccessfulBuy(Player player, Int32 quantity)
        {
            var newQuestSuccessChance = player.GetQuestSuccessChance();
            var oldQuestSuccessChance = newQuestSuccessChance - quantity * Constants.QuestGearSuccessPercent;
            return $"{quantity} gear [{String.Format("{0:0.0}", oldQuestSuccessChance * 100)}% -> {String.Format("{0:0.0}", newQuestSuccessChance * 100)}% quest success]";
        }

        public override Either<Int32, String> TryBuySingleUnit(Player player, Int32 price)
        {
            player.GearCount++;
            player.Points -= price;

            return () => 1;
        }


        public override Boolean IsForSale(Player player, out String reason)
        {
            if (player.GearCount < 150)
            {
                reason = default;
                return true;
            }

            reason = "You cannot buy any more gear.";
            return false;
        }

        public override String GetShopPrompt(Player player)
        {
            if (IsForSale(player, out _))
            {
                var questSuccessChance = player.GetQuestSuccessChance();
                return $"{base.GetShopPrompt(player)} [+{String.Format("{0:0.0}", questSuccessChance * 100)}% -> +{String.Format("{0:0.0}", (questSuccessChance + Constants.QuestGearSuccessPercent) * 100)}% quest success] for {GetPrice(player)} cheese";
            }

            return null;
        }
    }
}
