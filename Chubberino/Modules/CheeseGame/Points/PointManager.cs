using Chubberino.Client.Abstractions;
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
        public static TimeSpan PointGainCooldown { get; set; } = TimeSpan.FromMinutes(1);

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

            if (timeSinceLastPointGain >= PointGainCooldown)
            {
                if (player.Points >= player.MaximumPointStorage)
                {
                    Spooler.SpoolMessage($"{player.GetDisplayName()}, you have {player.Points}/{player.MaximumPointStorage} cheese and cannot store any more. Consider buying more cheese storage with \"!cheese buy storage\".");
                }
                else
                {
                    var cheese = CheeseRepository.GetRandomType(player.CheeseUnlocked);

                    // Cannot reach negative points.
                    // Cannot go above the point storage.
                    // Prestige bonus is only applied to base cheese gained.
                    // Workers will collectively add at least 1.
                    Int32 workerPoints = Math.Max(cheese.PointValue * (player.WorkerCount * (((Int32)player.LastWorkerProductionUpgradeUnlocked + 1) * 10) / 100), 1);

                    var newPoints = (Int32)Math.Min(Math.Max(player.Points + (cheese.PointValue * (1 + Constants.PrestigeBonus * player.Prestige)) + workerPoints, 0), player.MaximumPointStorage);

                    player.Points = newPoints;
                    player.LastPointsGained = DateTime.Now;

                    Context.SaveChanges();


                    Boolean isPositive = cheese.PointValue > 0;

                    var workerMessage = player.WorkerCount == 0
                        ? String.Empty
                        : (isPositive ? "+" : "-") + workerPoints;

                    String emote = isPositive
                        ? EmoteManager.GetRandomPositiveEmote()
                        : EmoteManager.GetRandomNegativeEmote();

                    Spooler.SpoolMessage($"{player.GetDisplayName()}, you made {cheese.Name} cheese ({(isPositive ? "+" : String.Empty)}{cheese.PointValue}{workerMessage}). {emote} You now have {player.Points}/{player.MaximumPointStorage} cheese. StinkyCheese");
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
