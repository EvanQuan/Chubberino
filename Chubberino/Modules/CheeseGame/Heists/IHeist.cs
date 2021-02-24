using Chubberino.Modules.CheeseGame.Models;
using System;
using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame.Heists
{
    public interface IHeist
    {
        String InitatorName { get; }

        Boolean Start(ChatMessage message, Player player);

        public Boolean TryAdd(Player player, Int32 points);
    }
}