using TwitchLib.Client.Models;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Points;

public interface IPointManager
{
    void AddPoints(ChatMessage message);

    void AddPoints(String channel, String username, Int32 points);
}
