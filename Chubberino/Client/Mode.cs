using Chubberino.Client.Abstractions;
using Chubberino.Client.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Client
{
    public sealed class Mode : Command
    {
        private IBot Bot { get; }
        public Mode(IExtendedClient client, TextWriter console, IBot bot)
            : base(client, console)
        {
            Bot = bot;
        }

        public override void Execute(IEnumerable<String> arguments)
        {
            switch (arguments?.FirstOrDefault())
            {
                case "m":
                case "mod":
                case "moderator":
                    Bot.Refresh(BotInfo.Instance.ModeratorClientOptions);
                    BotInfo.Instance.IsModerator = true;
                    break;
                case "n":
                case "normal":
                    Bot.Refresh(BotInfo.Instance.RegularClientOptions);
                    BotInfo.Instance.IsModerator = false;
                    break;

            }
        }
    }
}
