﻿using System;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client.Models;

namespace Chubberino.Client.Commands.Strategies
{
    public sealed class StopSettingStrategy : IStopSettingStrategy
    {
        public StopSettingStrategy(IBot bot)
        {
            Bot = bot;
        }

        private static ISet<String> StopWords { get; } = new HashSet<String>()
        {
            "bot",
        };

        public ITwitchClientManager TwitchClientManager { get; }
        public IBot Bot { get; }

        public Boolean ShouldStop(ChatMessage chatMessage)
        {
            if (!chatMessage.IsModerator) { return false; }

            if (chatMessage.Username.Equals("streamelements", StringComparison.OrdinalIgnoreCase)) { return false; }

            String[] messageWords = chatMessage.Message.Split(' ');

            return messageWords.Any(word => StopWords.Contains(word.ToLower()) || word.Equals(Bot.Name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
