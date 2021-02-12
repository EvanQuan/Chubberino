using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame.Points
{
    public interface IPointManager
    {
        void AddPoints(ChatMessage message);
    }
}
