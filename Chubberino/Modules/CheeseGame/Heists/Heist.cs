using Chubberino.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Models;
using System;
using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame.Heists
{
    public abstract class Heist : IHeist
    {
        public Heist(
            IApplicationContext context,
            Random random,
            ITwitchClientManager client,
            IEmoteManager emoteManager)
        {
            Context = context;
            Random = random;
            Client = client;
            EmoteManager = emoteManager;
        }

        public IApplicationContext Context { get; }
        public Random Random { get; }
        public ITwitchClientManager Client { get; }
        public IEmoteManager EmoteManager { get; }

        public String InitatorName => throw new NotImplementedException();

        public Boolean Start(ChatMessage message, Player player)
        {
            throw new NotImplementedException();
        }

        public Boolean TryAdd(Player player, Int32 points)
        {
            throw new NotImplementedException();
        }
    }
}
