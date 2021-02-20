using Chubberino.Client.Abstractions;
using Chubberino.Client.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chubberino.Client
{
    public sealed class Mode : Command
    {
        private IBot Bot { get; }
        public IModeratorClientOptions ModeratorClientOptions { get; }
        public IRegularClientOptions RegularClientOptions { get; }

        public Mode(
            ITwitchClientManager client,
            IConsole console,
            IBot bot,
            IModeratorClientOptions moderatorClientOptions,
            IRegularClientOptions regularClientOptions)
            : base(client, console)
        {
            Bot = bot;
            ModeratorClientOptions = moderatorClientOptions;
            RegularClientOptions = regularClientOptions;
        }

        public override void Execute(IEnumerable<String> arguments)
        {
            switch (arguments?.FirstOrDefault())
            {
                case "m":
                case "mod":
                case "moderator":
                    Bot.Refresh(ModeratorClientOptions);
                    Bot.IsModerator = true;
                    break;
                case "n":
                case "normal":
                    Bot.Refresh(RegularClientOptions);
                    Bot.IsModerator = false;
                    break;

            }
        }
    }
}
