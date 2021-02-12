using Chubberino.Modules.CheeseGame.Models;
using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame
{
    public interface IAddPointStrategy
    {
        void AddPoints(ChatMessage message);
    }
}
