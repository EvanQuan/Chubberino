using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame.Rankings
{
    public interface IRankManager : ICommandStrategy
    {
        void ShowRank(ChatMessage message);

        void RankUp(ChatMessage message);

    }
}
