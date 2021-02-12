using Chubberino.Client.Abstractions;
using Chubberino.Modules.CheeseGame.Database.Contexts;
using System;
using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame
{
    public sealed class PointManager : AbstractCommandStrategy, IPointManager
    {
        public TimeSpan PointGainCooldown { get; set; } = TimeSpan.FromSeconds(60);

        public ICheeseRepository CheeseRepository { get; }

        public PointManager(ApplicationContext context, IMessageSpooler spooler, ICheeseRepository cheeseRepository)
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
                    Spooler.SpoolMessage($"{GetPlayerDisplayName(player, message)}, you have {player.Points}/{player.MaximumPointStorage} cheese. Consider buying more cheese storage with \"!cheese shop\".");
                }
                else
                {
                    var cheese = CheeseRepository.GetRandomType(player.CheeseUnlocked);

                    // Cannot reach negative points.
                    // Cannot go above the point storage.
                    player.Points = Math.Min(Math.Max(player.Points + cheese.PointValue + player.WorkerCount, 0), player.MaximumPointStorage);
                    player.LastPointsGained = DateTime.Now;

                    Context.SaveChanges();


                    var workerMessage = player.WorkerCount == 0
                        ? String.Empty
                        : $"Your workers made {player.WorkerCount} cheese. ";

                    Spooler.SpoolMessage($"{GetPlayerDisplayName(player, message)}, you made {cheese.Name} ({cheese.PointValue}). {workerMessage}You now have {player.Points}/{player.MaximumPointStorage} cheese.");
                }
            }
            else
            {
                var timeUntilNextValidPointGain = PointGainCooldown - timeSinceLastPointGain;

                var timeToWait = timeUntilNextValidPointGain.TotalMinutes > 1
                    ? Math.Ceiling(timeUntilNextValidPointGain.TotalMinutes) + " minutes"
                    : Math.Ceiling(timeUntilNextValidPointGain.TotalSeconds) + " seconds";

                Spooler.SpoolMessage($"{GetPlayerDisplayName(player, message)}, wait {timeToWait} for your next cheese.");
            }
        }
    }
}
