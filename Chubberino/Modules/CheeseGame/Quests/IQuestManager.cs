using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame.Quests
{
    public interface IQuestManager
    {
        void TryStartQuest(ChatMessage message);
    }
}
