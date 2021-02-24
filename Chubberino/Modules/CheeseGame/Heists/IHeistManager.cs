using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame.Heists
{
    public interface IHeistManager
    {
        void StartHeist(ChatMessage message);

        IHeistManager AddHeist(IHeist heist);
    }
}
