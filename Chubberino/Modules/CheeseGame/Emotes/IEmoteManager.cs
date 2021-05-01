using Chubberino.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chubberino.Modules.CheeseGame.Emotes
{
    public interface IEmoteManager
    {
        String GetRandomPositiveEmote(String channelName);

        String GetRandomNegativeEmote(String channelName);

        void Refresh(String channel);

        EmoteManagerResult AddAll(IEnumerable<String> emotes, EmoteCategory category, String channel);

        EmoteManagerResult RemoveAll(IEnumerable<String> emotes, EmoteCategory category, String channel);

        IQueryable<String> Get(String channel, EmoteCategory category);
    }
}
