using System;
using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame.Points
{
    public interface IPointManager
    {
        void AddPoints(ChatMessage message);

        void AddPoints(String channel, String username, Int32 points);
    }
}
