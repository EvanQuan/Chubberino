using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame.Quests
{
    public interface IQuestManager
    {
        void StartQuest(ChatMessage message);

        IQuestManager AddQuest(IQuest quest);
    }
}
