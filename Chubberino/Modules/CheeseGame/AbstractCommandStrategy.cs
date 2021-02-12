using Chubberino.Client.Abstractions;
using Chubberino.Modules.CheeseGame.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
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
        protected ApplicationContext Context { get; }

        public IMessageSpooler Spooler { get; set; }

        public AbstractCommandStrategy(ApplicationContext context, IMessageSpooler spooler, Random random, IEmoteManager emoteManager)
        {
            Context = context;
            Spooler = spooler;
            Random = random;
            EmoteManager = emoteManager;
        }

        public Random Random { get; }

        public IEmoteManager EmoteManager { get; }

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

                Spooler.SpoolMessage($"!!! NEW CHEESE FACTORY !!! {player.GetDisplayName()} You have just begun building your own cheese factory in the land of Mookanda, where {player.ID - 1} other cheese factories already reside here. Begin producing cheese with \"!cheese\". You can get help with \"!cheese help\". Good luck!");
            }

            return player;
        }


        protected static String Format(TimeSpan timespan)
        {
            return (timespan.TotalMinutes > 1
                ? (Math.Floor(timespan.TotalMinutes) + " minutes and ")
                : String.Empty)
                + timespan.Seconds + " seconds";
        }
    }
}
