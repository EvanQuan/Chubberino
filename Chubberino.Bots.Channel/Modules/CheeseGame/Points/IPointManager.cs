using Chubberino.Bots.Channel.Modules.CheeseGame.Items.Upgrades.RecipeModifiers;
using TwitchLib.Client.Models;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Points;

public interface IPointManager
{
    void AddPoints(ChatMessage message, Option<RecipeModifier>[] modifiers);

    void AddPoints(String channel, String username, Int32 points);
}
