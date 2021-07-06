using System;
using TwitchLib.Client.Models;

namespace Chubberino.Bots.Common.Commands.Settings.Strategies
{
    public interface IStopSettingStrategy
    {
        Boolean ShouldStop(ChatMessage chatMessage);
    }
}
