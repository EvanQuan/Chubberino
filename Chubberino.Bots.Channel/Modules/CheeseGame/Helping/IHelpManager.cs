using System;
using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame.Helping
{
    public interface IHelpManager
    {
        void GetHelp(ChatMessage chatMessage);
    }
}
