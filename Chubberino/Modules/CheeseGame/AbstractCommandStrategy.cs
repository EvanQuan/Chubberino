using Chubberino.Client.Abstractions;
using Chubberino.Modules.CheeseGame.Database.Contexts;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.PlayerExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame
{
    public abstract class AbstractCommandStrategy : ICommandStrategy
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
        protected ApplicationContext Context { get; }

        public IMessageSpooler Spooler { get; set; }

        public AbstractCommandStrategy(ApplicationContext context, IMessageSpooler spooler, Random random)
        {
            Context = context;
            Spooler = spooler;
            Random = random;
        }

        public Random Random { get; }

        protected Player GetPlayer(ChatMessage message)
        {
            var player = Context.Players.FirstOrDefault(x => x.TwitchUserID == message.UserId);

            if (player == null)
            {
                player = new Player()
                {
                    TwitchUserID = message.UserId,
                    Name = message.DisplayName
                }
                .ResetRank();

                Context.Add(player);

                Context.SaveChanges();

                Spooler.SpoolMessage($"!!! NEW CHEESE FACTORY !!! {GetPlayerDisplayName(player, message)} You have just begun building your own cheese factory in the land of Mookanda, where {player.ID - 1} other cheese factories already reside here. Begin producing cheese with \"!cheese\". You can get help with \"!cheese help\". Good luck!");
            }

            return player;
        }

        protected static String GetPlayerDisplayName(Player player, ChatMessage message)
        {
            return $"{message.DisplayName} [P{player.Prestige} {player.Rank}, {player.Points}/{player.MaximumPointStorage} cheese, {player.WorkerCount}/{player.PopulationCount} workers]";
        }

        protected String GetRandomPositiveEmote()
        {
            return PositiveEmotes[Random.Next(PositiveEmotes.Count)];
        }

        protected String GetRandomNegativeEmote()
        {
            return NegativeEmotes[Random.Next(NegativeEmotes.Count)];
        }
    }
}
