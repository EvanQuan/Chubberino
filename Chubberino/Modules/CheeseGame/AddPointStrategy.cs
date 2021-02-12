using Chubberino.Client.Abstractions;
using Chubberino.Modules.CheeseGame.Database.Contexts;
using System;
using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame
{
    public sealed class AddPointStrategy : AbstractCommandStrategy, IAddPointStrategy
    {
        public TimeSpan PointGainCooldown { get; set; } = TimeSpan.FromSeconds(10);

        public ICheeseRepository CheeseRepository { get; }

        public AddPointStrategy(ApplicationContext context, IMessageSpooler spooler, ICheeseRepository cheeseRepository)
            : base(context, spooler)
        {
            CheeseRepository = cheeseRepository;
        }

        public void AddPoints(ChatMessage message)
        {
            var player = GetPlayer(message);
            var now = DateTime.Now;

            var timeSinceLastPointGain = now - player.LastPointsGained;


            if (timeSinceLastPointGain >= PointGainCooldown)
            {
                if (player.Points >= player.MaximumPointStorage)
                {
                    Spooler.SpoolMessage($"{message.DisplayName}, you have {player.Points} cheese, which is the maximum amount of cheese you can hold. Consider buying more cheese storage with \"!cheese shop\".");
                }
                else
                {
                    var variant = CheeseRepository.GetRandomVariant();
                    var type = CheeseRepository.GetRandomType(player.CheeseUnlocked);

                    var cheese = variant.Name + " " + type.Name;
                    var points = variant.PointValue + type.PointValue;

                    // Cannot reach negative points.
                    // Cannot go above the point storage.
                    player.Points = Math.Min(Math.Max(player.Points + points, 0), player.MaximumPointStorage);
                    player.LastPointsGained = DateTime.Now;

                    Context.SaveChanges();
                    Spooler.SpoolMessage($"{message.DisplayName}, you found {cheese} ({points}), and now have {player.Points} cheese.");
                }
            }
            else
            {
                var timeUntilNextValidPointGain = PointGainCooldown - timeSinceLastPointGain;

                var timeToWait = timeUntilNextValidPointGain.TotalMinutes > 1
                    ? Math.Ceiling(timeUntilNextValidPointGain.TotalMinutes) + " minutes"
                    : Math.Ceiling(timeUntilNextValidPointGain.TotalSeconds) + " seconds";

                Spooler.SpoolMessage($"{message.DisplayName}, wait {timeToWait} for your next cheese.");
            }
        }
    }
}
