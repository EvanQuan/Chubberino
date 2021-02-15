﻿using Chubberino.Client.Abstractions;
using Chubberino.Modules.CheeseGame.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Hazards;
using Chubberino.Modules.CheeseGame.PlayerExtensions;
using System;
using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame.Points
{
    public sealed class PointManager : AbstractCommandStrategy, IPointManager
    {
        public static TimeSpan PointGainCooldown { get; set; } = TimeSpan.FromMinutes(5);

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
            var now = DateTime.Now;
            var player = GetPlayer(message);

            var timeSinceLastPointGain = now - player.LastPointsGained;

            var playerStorage = player.GetTotalStorage();

            if (timeSinceLastPointGain >= PointGainCooldown)
            {
                if (player.Points >= playerStorage)
                {
                    Spooler.SpoolMessage($"{player.GetDisplayName()}, you have {player.Points}/{playerStorage} cheese and cannot store any more. Consider buying more cheese storage with \"!cheese buy storage\".");
                }
                else
                {
                    var cheese = CheeseRepository.GetRandomType(player.CheeseUnlocked);

                    String outputMessage = player.GetDisplayName() + " ";

                    outputMessage += HazardManager.UpdateMouseInfestationStatus(player);

                    var oldPoints = player.Points;
                    player.AddPoints(cheese, !player.IsMouseInfested);

                    var newPoints = player.Points;

                    var pointsGained = newPoints - oldPoints;

                    player.LastPointsGained = now;

                    Context.SaveChanges();

                    Boolean isPositive = cheese.PointValue > 0;

                    String emote = isPositive
                        ? EmoteManager.GetRandomPositiveEmote()
                        : EmoteManager.GetRandomNegativeEmote();

                    outputMessage += $"You made {cheese.Name} cheese ({(isPositive ? "+" : String.Empty)}{pointsGained}). {emote} You now have {player.Points}/{playerStorage} cheese. StinkyCheese";

                    Spooler.SpoolMessage(outputMessage);
                }
            }
            else
            {
                var timeUntilNextValidPointGain = PointGainCooldown - timeSinceLastPointGain;

                var timeToWait = Format(timeUntilNextValidPointGain);

                Spooler.SpoolMessage($"{player.GetDisplayName()}, you must wait {timeToWait} until you can make more cheese.");
            }
        }
    }
}
