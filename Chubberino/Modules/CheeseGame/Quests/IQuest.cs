using Chubberino.Modules.CheeseGame.Models;
using System;
using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame.Quests
{
    public interface IQuest
    {
        Boolean Start(ChatMessage message, Player player);
    }
}
