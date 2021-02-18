using Chubberino.Client.Abstractions;
using Chubberino.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Hazards;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.PlayerExtensions;
using System;
using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame.Points
{
    public sealed class PointManager : AbstractCommandStrategy, IPointManager
    {
        public static TimeSpan PointGainCooldown { get; set; } = TimeSpan.FromMinutes(30);

        public ICheeseRepository CheeseRepository { get; }

        public IHazardManager HazardManager { get; }

        public PointManager(
            ApplicationContext context,
            IMessageSpooler spooler,
            ICheeseRepository cheeseRepository,
            Random random,
            IEmoteManager emoteManager,
            IHazardManager hazardManager)
            : base(context, spooler, random, emoteManager)
        {
            CheeseRepository = cheeseRepository;
            HazardManager = hazardManager;
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
                    Spooler.SpoolMessage(message.Channel, $"{player.GetDisplayName()}, you have {player.Points}/{playerStorage} cheese and cannot store any more. Consider buying more cheese storage with \"!cheese buy storage\".");
                }
                else
                {
                    CheeseType cheese = CheeseRepository.GetRandomType(player.CheeseUnlocked);

                    String outputMessage = HazardManager.UpdateMouseInfestationStatus(player);

                    Int32 oldPoints = player.Points;
                    player.AddPoints(cheese, !player.IsMouseInfested);

                    Int32 newPoints = player.Points;

                    Int32 pointsGained = newPoints - oldPoints;

                    player.LastPointsGained = now;

                    Context.SaveChanges();

                    Boolean isPositive = cheese.PointValue > 0;

                    Boolean useChannelEmotes = message.Channel.Equals("ChubbehMouse", StringComparison.OrdinalIgnoreCase);

                    String emote = isPositive
                        ? EmoteManager.GetRandomPositiveEmote(useChannelEmotes)
                        : EmoteManager.GetRandomNegativeEmote(useChannelEmotes);

                    outputMessage += $"You made {cheese.Name} cheese ({(isPositive ? "+" : String.Empty)}{pointsGained}). {emote} You now have {player.Points}/{playerStorage} cheese. StinkyCheese";
                    outputMessage = player.GetDisplayName() + " " + outputMessage;

                    Spooler.SpoolMessage(message.Channel, outputMessage);
                }
            }
            else
            {
                TimeSpan timeUntilNextValidPointGain = PointGainCooldown - timeSinceLastPointGain;

                String timeToWait = Format(timeUntilNextValidPointGain);

                Spooler.SpoolMessage(message.Channel, $"{player.GetDisplayName()}, you must wait {timeToWait} until you can make more cheese.");
            }
        }
    }
}
