using Chubberino.Client.Abstractions;
using Chubberino.Modules.CheeseGame.Database.Contexts;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.PlayerExtensions;
using System.Linq;
using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame
{
    public abstract class AbstractCommandStrategy
    {
        protected ApplicationContext Context { get; }

        public IMessageSpooler Spooler { get; }

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
            }

            return player;
        }
    }
}
