using Chubberino.Client.Abstractions;
using Chubberino.Modules.CheeseGame.Database.Contexts;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.PlayerExtensions;
using System;
using System.Linq;
using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame
{
    public abstract class AbstractCommandStrategy : ICommandStrategy
    {
        protected ApplicationContext Context { get; }

        public IMessageSpooler Spooler { get; set; }

        public AbstractCommandStrategy(ApplicationContext context, IMessageSpooler spooler)
        {
            Context = context;
            Spooler = spooler;
        }

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
    }
}
