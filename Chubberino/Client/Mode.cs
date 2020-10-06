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
                    Bot.Refresh(Bot.ModeratorClientOptions);
                    Bot.IsModerator = true;
                    break;
                case "n":
                case "normal":
                    Bot.Refresh(Bot.RegularClientOptions);
                    Bot.IsModerator = false;
                    break;

            }
        }
    }
}
