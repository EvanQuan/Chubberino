using Chubberino.Client;
using Chubberino.Client.Services;
using Chubberino.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Hazards;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.PlayerExtensions;
using Chubberino.Utility;
using System;
using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame.Points
{
    public sealed class PointManager : AbstractCommandStrategy, IPointManager
    {
        public static TimeSpan PointGainCooldown { get; set; } = TimeSpan.FromMinutes(15);

        public ICheeseRepository CheeseRepository { get; }

        public IHazardManager HazardManager { get; }
        public IDateTimeService DateTime { get; }

        public PointManager(
            IApplicationContext context,
            ITwitchClientManager client,
            ICheeseRepository cheeseRepository,
            Random random,
            IEmoteManager emoteManager,
            IHazardManager hazardManager,
            IDateTimeService dateTime)
            : base(context, client, random, emoteManager)
        {
            CheeseRepository = cheeseRepository;
            HazardManager = hazardManager;
            DateTime = dateTime;
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
                    CheeseType cheese = CheeseRepository.GetRandomType(player.CheeseUnlocked);

                    String outputMessage = HazardManager.UpdateMouseInfestationStatus(player);

                    Boolean useChannelEmotes = message.Channel.Equals("ChubbehMouse", StringComparison.OrdinalIgnoreCase);

                    Int32 oldPoints = player.Points;

                    Boolean isCritical = Random.TryPercentChance((Int32)player.NextCriticalCheeseUpgradeUnlock * Constants.CriticalCheeseUpgradePercent);

                    if (isCritical)
                    {
                        if (cheese.PointValue > 0)
                        {
                            outputMessage += $"{EmoteManager.GetRandomPositiveEmote(useChannelEmotes)} CRITICAL CHEESE!!! {EmoteManager.GetRandomPositiveEmote(useChannelEmotes)} ";
                        }
                        else
                        {
                            outputMessage += $"{EmoteManager.GetRandomNegativeEmote(useChannelEmotes)} CRITICAL NEGATIVE CHEESE!!! {EmoteManager.GetRandomNegativeEmote(useChannelEmotes)} ";
                        }
                    }

                    player.AddPoints(cheese, !player.IsMouseInfested(), isCritical);

                    Int32 newPoints = player.Points;

                    Int32 pointsGained = newPoints - oldPoints;

                    player.LastPointsGained = now;

                    Context.SaveChanges();

                    Boolean isPositive = cheese.PointValue > 0;


                    String emote = isPositive
                        ? EmoteManager.GetRandomPositiveEmote(useChannelEmotes)
                        : EmoteManager.GetRandomNegativeEmote(useChannelEmotes);


                    outputMessage += $"You made {cheese.Name} cheese. {emote} ({(isPositive ? "+" : String.Empty)}{pointsGained})";

                    TwitchClientManager.SpoolMessageAsMe(message.Channel, player, outputMessage);
                }
            }
            else
            {
                TimeSpan timeUntilNextValidPointGain = PointGainCooldown - timeSinceLastPointGain;

                String timeToWait = timeUntilNextValidPointGain.Format();

                TwitchClientManager.SpoolMessageAsMe(message.Channel, player, $"You must wait {timeToWait} until you can make more cheese.");
            }
        }
    }
}
