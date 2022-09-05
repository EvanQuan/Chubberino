using System;
using TwitchLib.Client.Models;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Helping;

public interface IHelpManager
{
    void GetHelp(ChatMessage chatMessage);
}
