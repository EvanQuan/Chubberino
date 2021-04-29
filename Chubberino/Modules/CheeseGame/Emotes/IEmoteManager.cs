using Chubberino.Database.Models;
using System;

namespace Chubberino.Modules.CheeseGame.Emotes
{
    public interface IEmoteManager
    {
        String GetRandomPositiveEmote(String channelName);

        String GetRandomNegativeEmote(String channelName);

        void Refresh(String channel);

        void Add(String emote, EmoteCategory category, String channel);

        Boolean TryRemove(String emote, EmoteCategory category, String channel);
    }
}
