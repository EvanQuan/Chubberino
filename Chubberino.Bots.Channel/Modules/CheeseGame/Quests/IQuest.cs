using Chubberino.Database.Models;
using System;
using TwitchLib.Client.Models;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Quests;

public interface IQuest
{
    Boolean Start(ChatMessage message, Player player);
}
