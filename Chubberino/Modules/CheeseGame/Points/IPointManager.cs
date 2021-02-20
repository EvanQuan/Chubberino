using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame.Points
{
    public interface IPointManager : ICommandStrategy
    {
        void AddPoints(ChatMessage message);
    }
}
