using TwitchLib.Client.Models;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Quests;

public interface IQuestManager
{
    void TryStartQuest(ChatMessage message);
}
