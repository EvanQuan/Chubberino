﻿using Chubberino.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.PlayerExtensions;
using Chubberino.Modules.CheeseGame.Points;
using System;

namespace Chubberino.Modules.CheeseGame.Quests.GainWorkers
{
    public sealed class PanelaPlainsQuest : Quest
    {
        public PanelaPlainsQuest(
            IApplicationContext context,
            Random random,
            ITwitchClientManager client,
            IEmoteManager emoteManager,
            ICalculator calculator)
            : base(context, random, client, emoteManager, calculator)
        {
        }

        protected override String OnFailure(Player player)
        {
            return $"You search around and don't find anything interesting. You return to your cheese factory empty-handed.";
        }

        protected override String OnIntroduction(Player player)
        {
            return $"{GetPlayerWithWorkers(player)} venture out onto the roads of the Panela Plains.";
        }

        protected override String OnSuccess(Player player)
        {
            if (player.WorkerCount + 1 > player.PopulationCount)
            {
                Int32 rewardPoints = (Int32)(50 * Calculator.GetQuestRewardMultiplier(player.Rank));
                player.AddPoints(rewardPoints);
                Context.SaveChanges();

                return $"You come across a lonely traveller looking for work. Unfortunately, you do not have enough population slots for him to join your efforts, and so gives you some cheese instead. (+{rewardPoints} cheese)";
            }
            else
            {
                player.WorkerCount++;
                Context.SaveChanges();
                return "You come across a lonely traveller looking for work. He decides to join your efforts. (+1 worker)";
            }

        }
    }
}