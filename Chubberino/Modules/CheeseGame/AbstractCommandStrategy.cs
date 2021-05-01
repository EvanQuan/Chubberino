using Chubberino.Client;
using Chubberino.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Models;
using System;
using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame
{
    public abstract class AbstractCommandStrategy : ICommandStrategy
    {
        protected IApplicationContext Context { get; }

        public ITwitchClientManager TwitchClientManager { get; set; }

        public AbstractCommandStrategy(
            IApplicationContext context,
            ITwitchClientManager client,
            Random random,
            IEmoteManager emoteManager)
        {
            Context = context;
            TwitchClientManager = client;
            Random = random;
            EmoteManager = emoteManager;
        }

        public Random Random { get; }

        public IEmoteManager EmoteManager { get; }

        protected Player GetPlayer(ChatMessage message)
        {
            return Context.GetPlayer(TwitchClientManager, message);
        }
    }
}
