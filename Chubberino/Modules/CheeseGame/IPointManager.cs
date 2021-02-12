using Chubberino.Modules.CheeseGame.Models;
using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame
{
    public interface IPointManager
    {
        void AddPoints(ChatMessage message);
    }
}
