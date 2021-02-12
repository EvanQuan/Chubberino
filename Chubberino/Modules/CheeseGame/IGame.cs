using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame
{
    public interface IGame
    {
        void AddPoints(ChatMessage message);
    }
}
