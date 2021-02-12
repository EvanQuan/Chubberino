using Chubberino.Client.Abstractions;
using Chubberino.Modules.CheeseGame.Database.Contexts;
using System;
using System.Collections.Generic;
using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame.Points
{
    public sealed class PointManager : AbstractCommandStrategy, IPointManager
    {
        private static IReadOnlyList<String> PositiveEmotes { get; } = new List<String>()
        {
            "CHUBBIES",
            "COGGERS",
            "Chubbehtusa",
            "Chubbies",
            "NODDERS",
            "OkayChamp",
            "POGCRAZY",
            "POGGERS",
            "Pepetusa",
            "PogU",
            "SUPERPOG",
            "WOODY",
            "berryA",
            "chubDuane",
            "elmoHype",
            "iron95Pls",
            "peepoClap",
            "pepoBoogie",
            "sanaSnuggle",
            "sheCrazy",
            "vibePls",
            "yyjSUPERPOG",
            "yyjTasty",
        };

        private static IReadOnlyList<String> NegativeEmotes { get; } = new List<String>()
        {
            "4Weirder",
            "ChubOest",
            "DansChamp",
            "Jelleh",
            "KEKBye",
            "KEKW",
            "Karen",
            "LULW",
            "NOIDONTTHINKSO",
            "NOP",
            "NOPERS",
            "OMEGALUL",
            "PainChamp",
            "PepeLaugh",
            "PepeSpit",
            "PogO",
            "Sadge",
            "TearChub",
            "WeirdChamp",
            "WideRage",
            "chubLeave",
            "chubOff",
            "reeferSad",
            "widepeepoLuL",
            "widepeepoSad",
            "yyjOMEGALULDANCE",
            "ZULUL",
        };

        public TimeSpan PointGainCooldown { get; set; } = TimeSpan.FromMinutes(1);

        public ICheeseRepository CheeseRepository { get; }
        public Random Random { get; }

        public PointManager(ApplicationContext context, IMessageSpooler spooler, ICheeseRepository cheeseRepository, Random random)
            : base(context, spooler)
        {
            CheeseRepository = cheeseRepository;
            Random = random;
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
                    Spooler.SpoolMessage($"{GetPlayerDisplayName(player, message)}, you have {player.Points}/{player.MaximumPointStorage} cheese and cannot store any more. Consider buying more cheese storage with \"!cheese buy storage\".");
                }
                else
                {
                    var cheese = CheeseRepository.GetRandomType(player.CheeseUnlocked);

                    // Cannot reach negative points.
                    // Cannot go above the point storage.
                    // Prestige bonus is only applied to base cheese gained.
                    player.Points = (Int32)Math.Min(Math.Max(player.Points + (cheese.PointValue  * (1 + Constants.PrestigeBonus * player.Prestige)) + player.WorkerCount, 0), player.MaximumPointStorage);
                    player.LastPointsGained = DateTime.Now;

                    Context.SaveChanges();


                    var workerMessage = player.WorkerCount == 0
                        ? String.Empty
                        : $"Your worker{(player.WorkerCount == 1 ? String.Empty : "s")} made +{player.WorkerCount} cheese. {PositiveEmotes[Random.Next(PositiveEmotes.Count)]} ";

                    Boolean isPositive = cheese.PointValue > 0;
                    String emote = isPositive
                        ? PositiveEmotes[Random.Next(PositiveEmotes.Count)]
                        : NegativeEmotes[Random.Next(NegativeEmotes.Count)];

                    Spooler.SpoolMessage($"{GetPlayerDisplayName(player, message)}, you made {cheese.Name} cheese ({(isPositive ? "+" : String.Empty)}{cheese.PointValue}). {emote} {workerMessage}You now have {player.Points}/{player.MaximumPointStorage} cheese. StinkyCheese");
                }
            }
            else
            {
                var timeUntilNextValidPointGain = PointGainCooldown - timeSinceLastPointGain;

                var timeToWait = (timeUntilNextValidPointGain.TotalMinutes > 1
                    ? (Math.Ceiling(timeUntilNextValidPointGain.TotalMinutes) + " minutes and ")
                    : String.Empty)
                    + timeUntilNextValidPointGain.Seconds + " seconds";

                Spooler.SpoolMessage($"{GetPlayerDisplayName(player, message)}, wait {timeToWait} for your next cheese.");
            }
        }
    }
}
