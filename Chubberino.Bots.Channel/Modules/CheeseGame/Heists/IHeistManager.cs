using Chubberino.Database.Contexts;
using Chubberino.Database.Models;
using TwitchLib.Client.Models;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Heists;

public interface IHeistManager
{
    void Heist(ChatMessage message);

    void LeaveAllHeists(IApplicationContext context, Player player);
}
