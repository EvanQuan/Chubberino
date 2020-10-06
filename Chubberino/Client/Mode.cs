using Chubberino.Client.Abstractions;
using Chubberino.Client.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Chubberino.Client
{
    public sealed class Mode : Command
    {
        private IBot Bot { get; }

        private BotInfo BotInfo { get; }

        public Mode(IExtendedClient client, TextWriter console, IBot bot, BotInfo botInfo)
            : base(client, console)
        {
            Bot = bot;
            BotInfo = botInfo;
        }

        public override void Execute(IEnumerable<String> arguments)
        {
            switch (arguments?.FirstOrDefault())
            {
                case "m":
                case "mod":
                case "moderator":
                    Bot.Refresh(BotInfo.ModeratorClientOptions);
                    BotInfo.IsModerator = true;
                    break;
                case "n":
                case "normal":
                    Bot.Refresh(BotInfo.RegularClientOptions);
                    BotInfo.IsModerator = false;
                    break;

            }
        }
    }
}
