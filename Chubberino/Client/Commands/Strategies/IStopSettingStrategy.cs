using System;
using TwitchLib.Client.Models;

namespace Chubberino.Client.Commands.Strategies
{
    public interface IStopSettingStrategy
    {
        Boolean ShouldStop(ChatMessage chatMessage);
    }
}
