﻿using Chubberino.Database.Models;
using System.Collections.Generic;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Emotes;

public interface IEmoteManager
{
    EmoteManagerResult AddAll(IEnumerable<String> emotes, EmoteCategory category, String channel);

    EmoteManagerResult RemoveAll(IEnumerable<String> emotes, EmoteCategory category, String channel);

    IReadOnlyList<String> Get(String channel, EmoteCategory category);
}
