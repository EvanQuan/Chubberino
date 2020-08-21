using Chubberino.Client.Abstractions;
using Chubberino.Client.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Client
{
    public sealed class Mode : Command
    {
        public Mode(IExtendedClient client)
            : base(client)
        {
            
        }

        public override void Execute(IEnumerable<String> arguments)
        {
            switch (arguments?.FirstOrDefault())
            {
                case "m":
                case "mod":
                case "moderator":
                    Bot.Instance.Refresh(BotInfo.Instance.ModeratorClientOptions);
                    BotInfo.Instance.IsModerator = true;
                    break;
                case "n":
                case "normal":
                    Bot.Instance.Refresh(BotInfo.Instance.RegularClientOptions);
                    BotInfo.Instance.IsModerator = false;
                    break;

            }
        }
    }
}
