using Chubberino.Client;
using Chubberino.Client.Services;
using Chubberino.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Hazards;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.PlayerExtensions;
using Chubberino.Utility;
using System;
using System.Linq;
using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame.Points
{
    public sealed class PointManager : AbstractCommandStrategy, IPointManager
    {
        public static TimeSpan PointGainCooldown { get; set; } = TimeSpan.FromMinutes(15);

        public IRepository<CheeseType> CheeseRepository { get; }
        public ICheeseModifierManager CheeseModifierManager { get; }
        public IHazardManager HazardManager { get; }
        public IDateTimeService DateTime { get; }
        public IConsole Console { get; }
        public ICalculator Calculator { get; }

        public PointManager(
            IApplicationContext context,
            ITwitchClientManager client,
            IRepository<CheeseType> cheeseRepository,
            ICheeseModifierManager cheeseModifierManager,
            Random random,
            IEmoteManager emoteManager,
            IHazardManager hazardManager,
            IDateTimeService dateTime,
            IConsole console,
            ICalculator calculator)
            : base(context, client, random, emoteManager)
        {
            CheeseRepository = cheeseRepository;
            CheeseModifierManager = cheeseModifierManager;
            HazardManager = hazardManager;
            DateTime = dateTime;
            Console = console;
            Calculator = calculator;
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
                    CheeseType initialCheese = CheeseRepository.GetRandom(player.CheeseUnlocked);

                    CheeseModifier modifier = CheeseModifierManager.GetRandomModifier(player.NextCheeseModifierUpgradeUnlock);

                    CheeseType cheese = modifier.Modify(initialCheese);

                    String outputMessage = HazardManager.UpdateMouseInfestationStatus(player);

                    Int32 oldPoints = player.Points;

                    Boolean isCritical = Random.TryPercentChance((Int32)player.NextCriticalCheeseUpgradeUnlock * Constants.CriticalCheeseUpgradePercent);

                    if (isCritical)
                    {
                        if (cheese.PointValue > 0)
                        {
                            outputMessage += $"{EmoteManager.GetRandomPositiveEmote(message.Channel)} CRITICAL CHEESE!!! {EmoteManager.GetRandomPositiveEmote(message.Channel)} ";
                        }
                        else
                        {
                            outputMessage += $"{EmoteManager.GetRandomNegativeEmote(message.Channel)} CRITICAL NEGATIVE CHEESE!!! {EmoteManager.GetRandomNegativeEmote(message.Channel)} ";
                        }
                    }

                    player.AddPoints(cheese, Calculator, !player.IsMouseInfested(), isCritical);

                    Int32 newPoints = player.Points;

                    Int32 pointsGained = newPoints - oldPoints;

                    player.LastPointsGained = now;

                    Context.SaveChanges();

                    Boolean isPositive = cheese.PointValue > 0;


                    String emote = isPositive
                        ? EmoteManager.GetRandomPositiveEmote(message.Channel)
                        : EmoteManager.GetRandomNegativeEmote(message.Channel);


                    outputMessage += $"You made some {cheese.Name}. {emote} ({(isPositive ? "+" : String.Empty)}{pointsGained} cheese)";

                    TwitchClientManager.SpoolMessageAsMe(message.Channel, player, outputMessage);
                }
            }
            else
            {
                TimeSpan timeUntilNextValidPointGain = PointGainCooldown - timeSinceLastPointGain;

                String timeToWait = timeUntilNextValidPointGain.Format();

                TwitchClientManager.SpoolMessageAsMe(message.Channel, player,
                    $"You must wait {timeToWait} until you can make more cheese.",
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
                TwitchClientManager.SpoolMessageAsMe(channel, player, $"The Mischievious Cheese Demon sneaks up on you and pickpockets some cheese. ({pointGain} cheese)");
            }
        }
    }
}
