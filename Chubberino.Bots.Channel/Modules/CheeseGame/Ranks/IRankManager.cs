using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame.Ranks
{
    public interface IRankManager
    {
        void ShowRank(ChatMessage message);

        void RankUp(ChatMessage message);

    }
}
