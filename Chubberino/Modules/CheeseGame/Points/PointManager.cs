﻿using Chubberino.Client;
using Chubberino.Client.Services;
using Chubberino.Database.Contexts;
using Chubberino.Database.Models;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Hazards;
using Chubberino.Modules.CheeseGame.Items;
using Chubberino.Modules.CheeseGame.Items.Recipes;
using Chubberino.Modules.CheeseGame.Items.Upgrades;
using Chubberino.Modules.CheeseGame.Items.Upgrades.RecipeModifiers;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame.Points
{
    public sealed class PointManager : IPointManager
    {
        public static TimeSpan PointGainCooldown { get; set; } = TimeSpan.FromMinutes(15);
        public IApplicationContextFactory ContextFactory { get; }
        public ITwitchClientManager Client { get; }
        public IReadOnlyList<RecipeInfo> RecipeRepository { get; }
        public IReadOnlyList<RecipeModifier> RecipeModifierManager { get; }
        public Random Random { get; }
        public IEmoteManager EmoteManager { get; }
        public IHazardManager HazardManager { get; }
        public IDateTimeService DateTime { get; }
        public IConsole Console { get; }

        public PointManager(
            IApplicationContextFactory contextFactory,
            ITwitchClientManager client,
            IReadOnlyList<RecipeInfo> recipeRepository,
            IReadOnlyList<RecipeModifier> recipeModifierRepository,
            Random random,
            IEmoteManager emoteManager,
            IHazardManager hazardManager,
            IDateTimeService dateTime,
            IConsole console)
        {
            ContextFactory = contextFactory;
            Client = client;
            RecipeRepository = recipeRepository;
            RecipeModifierManager = recipeModifierRepository;
            Random = random;
            EmoteManager = emoteManager;
            HazardManager = hazardManager;
            DateTime = dateTime;
            Console = console;
        }

        public async void AddPoints(ChatMessage message)
        {
            DateTime now = DateTime.Now;

            using var context = ContextFactory.GetContext();

            Player player = context.GetPlayer(Client, message);

            TimeSpan timeSinceLastPointGain = now - player.LastPointsGained;

            Int32 playerStorage = player.GetTotalStorage();

            if (timeSinceLastPointGain >= PointGainCooldown)
            {
                if (player.Points >= playerStorage)
                {
                    Client.SpoolMessageAsMe(message.Channel, player, $" You have {player.Points}/{playerStorage} cheese and cannot store any more. Consider buying more cheese storage with \"!cheese buy storage\".");
                    return;
                }

                RecipeInfo initialCheese = Random.NextElement(RecipeRepository, player.CheeseUnlocked);

                RecipeModifier modifier = Random.NextElement(RecipeModifierManager, (Int32)player.NextCheeseModifierUpgradeUnlock);

                RecipeInfo cheese = modifier.Modify(initialCheese);

                StringBuilder outputMessage = new();

                String infestationStatus = HazardManager.UpdateInfestationStatus(player);

                var infestationTask = context.SaveChangesAsync();

                if (!String.IsNullOrWhiteSpace(infestationStatus))
                {
                    outputMessage
                        .Append(infestationStatus)
                        .Append(Random.NextElement(EmoteManager.Get(message.Channel, EmoteCategory.Rat)))
                        .Append(' ');
                }

                Boolean isCritical = Random.TryPercentChance((Int32)player.NextCriticalCheeseUpgradeUnlock * RankUpgradeExtensions.CriticalCheeseUpgradePercent);

                await infestationTask;

                var modifiedPoints = player.GetModifiedPoints(cheese.Points, isCritical);

                player.AddPoints(modifiedPoints);

                player.LastPointsGained = now;

                var addPointsTask = context.SaveChangesAsync();

                Boolean isPositive = cheese.Points > 0;

                var emoteCategory = isPositive ? EmoteCategory.Positive : EmoteCategory.Negative;

                var emoteList = EmoteManager.Get(message.Channel, emoteCategory);

                if (isCritical)
                {
                    outputMessage.Append(Random.NextElement(emoteList));

                    if (!isPositive)
                    {
                        outputMessage.Append(" NEGATIVE ");
                    }

                    outputMessage
                        .Append(" CRITICAL CHEESE!!! ")
                        .Append(Random.NextElement(emoteList))
                        .Append(' ');
                }

                outputMessage.Append($"You made some {cheese.Name}. {Random.NextElement(emoteList)} ({(isPositive ? "+" : String.Empty)}{modifiedPoints} cheese)");

                Client.SpoolMessageAsMe(message.Channel, player, outputMessage.ToString());

                await addPointsTask;
                return;
            }

            TimeSpan timeUntilNextValidPointGain = PointGainCooldown - timeSinceLastPointGain;

            String timeToWait = timeUntilNextValidPointGain.Format();

            Client.SpoolMessageAsMe(message.Channel, player,
                $"You must wait {timeToWait} until you can make more cheese. {Random.NextElement(EmoteManager.Get(message.Channel, EmoteCategory.Waiting))}",
                Priority.Low);
        }

        public async void AddPoints(String channel, String username, Int32 points)
        {
            using var context = ContextFactory.GetContext();

            var player = context.Players.FirstOrDefault(x => x.Name.Equals(username));

            if (player is null)
            {
                Console.WriteLine($"Cannot find player {username} to add points.");
                return;
            }

            Int32 oldPoints = player.Points;

            player.AddPoints(points);
            var task = context.SaveChangesAsync();

            Int32 newPoints = player.Points;

            Int32 pointGain = newPoints - oldPoints;

            if (pointGain > 0)
            {
                Client.SpoolMessageAsMe(channel, player, $"The Magical Cheese Fairy descends from the heavens and hands you a gift. (+{pointGain} cheese)");
            }
            else if (pointGain < 0)
            {
                Client.SpoolMessageAsMe(channel, player, $"The Mischievious Cheese Goblin sneaks up on you and pickpockets some cheese. ({pointGain} cheese)");
            }

            await task;
        }
    }
}
