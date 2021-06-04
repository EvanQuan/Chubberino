using Chubberino.Database.Contexts;
using Chubberino.Modules.CheeseGame.Models;
using System;
using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame.Heists
{
    public interface IHeistManager
    {
        void InitiateHeist(ChatMessage message);

        void LeaveAllHeists(IApplicationContext context, Player player);
    }
}
