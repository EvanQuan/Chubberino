﻿using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame.Heists
{
    public interface IHeistManager
    {
        void InitiateHeist(ChatMessage message);
    }
}
