using Chubberino.Client;
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
    public sealed class PointManager : AbstractCommandStrategy, IPointManager
    {
        public static TimeSpan PointGainCooldown { get; set; } = TimeSpan.FromMinutes(15);

        public IReadOnlyList<RecipeInfo> RecipeRepository { get; }
        public IReadOnlyList<RecipeModifier> RecipeModifierManager { get; }
        public IHazardManager HazardManager { get; }
        public IDateTimeService DateTime { get; }
        public IConsole Console { get; }

        public PointManager(
            IApplicationContext context,
            ITwitchClientManager client,
            IReadOnlyList<RecipeInfo> recipeRepository,
            IReadOnlyList<RecipeModifier> recipeModifierRepository,
            Random random,
            IEmoteManager emoteManager,
            IHazardManager hazardManager,
            IDateTimeService dateTime,
            IConsole console)
            : base(context, client, random, emoteManager)
        {
            RecipeRepository = recipeRepository;
            RecipeModifierManager = recipeModifierRepository;
            HazardManager = hazardManager;
            DateTime = dateTime;
            Console = console;
        }

        public void AddPoints(ChatMessage message)
        {
            DateTime now = DateTime.Now;
            Player player = GetPlayer(message);

            TimeSpan timeSinceLastPointGain = now - player.LastPointsGained;

            Int32 playerStorage = player.GetTotalStorage();

            if (timeSinceLastPointGain >= PointGainCooldown)
            {
                if (player.Points >= playerStorage)
                {
                    TwitchClientManager.SpoolMessageAsMe(message.Channel, player, $" You have {player.Points}/{playerStorage} cheese and cannot store any more. Consider buying more cheese storage with \"!cheese buy storage\".");
                }
                else
                {
                    RecipeInfo initialCheese = Random.NextElement(RecipeRepository, player.CheeseUnlocked);

                    RecipeModifier modifier = Random.NextElement(RecipeModifierManager, (Int32)player.NextCheeseModifierUpgradeUnlock);

                    RecipeInfo cheese = modifier.Modify(initialCheese);

                    StringBuilder outputMessage = new();

                    String infestationStatus = HazardManager.UpdateInfestationStatus(player);

                    if (!String.IsNullOrWhiteSpace(infestationStatus))
                    {
                        outputMessage
                            .Append(infestationStatus)
                            .Append(Random.NextElement(EmoteManager.Get(message.Channel, EmoteCategory.Rat)))
                            .Append(' ');
                    }

                    Boolean isCritical = Random.TryPercentChance((Int32)player.NextCriticalCheeseUpgradeUnlock * RankUpgradeExtensions.CriticalCheeseUpgradePercent);

                    var modifiedPoints = player.GetModifiedPoints(cheese.Points, isCritical);

                    player.AddPoints(modifiedPoints);

                    player.LastPointsGained = now;

                    Context.SaveChanges();

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

                    TwitchClientManager.SpoolMessageAsMe(message.Channel, player, outputMessage.ToString());
                }
            }
            else
            {
                TimeSpan timeUntilNextValidPointGain = PointGainCooldown - timeSinceLastPointGain;

                String timeToWait = timeUntilNextValidPointGain.Format();

                TwitchClientManager.SpoolMessageAsMe(message.Channel, player,
                    $"You must wait {timeToWait} until you can make more cheese. {Random.NextElement(EmoteManager.Get(message.Channel, EmoteCategory.Waiting))}",
                    Priority.Low);
            }
        }

        public void AddPoints(String channel, String username, Int32 points)
        {
            var player = Context.Players.FirstOrDefault(x => x.Name.Equals(username));

            if (player == default)
            {
                Console.WriteLine($"Cannot find player {username} to add points.");
                return;
            }

            Int32 oldPoints = player.Points;

            player.AddPoints(points);
            Context.SaveChanges();

            Int32 newPoints = player.Points;

            Int32 pointGain = newPoints - oldPoints;

            if (pointGain > 0)
            {
                TwitchClientManager.SpoolMessageAsMe(channel, player, $"The Magical Cheese Fairy descends from the heavens and hands you a gift. (+{pointGain} cheese)");
            }
            else if (pointGain < 0)
            {
                TwitchClientManager.SpoolMessageAsMe(channel, player, $"The Mischievious Cheese Goblin sneaks up on you and pickpockets some cheese. ({pointGain} cheese)");
            }
        }
    }
}
