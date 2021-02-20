using System;
using System.Collections.Generic;
using System.Linq;

namespace Chubberino.Client.Commands
{
    public sealed class Jimbox : Command
    {
        public Jimbox(ITwitchClientManager client, IConsole console)
            : base(client, console)
        {
        }

        /// <summary>Dank memes are cool</summary>
        /// <example></example>
        /// <param name="arguments"></param>
        public override void Execute(IEnumerable<String> arguments)
        {
            String surroundingEmote = arguments.FirstOrDefault() ?? "yyjW";

            TwitchClientManager.SpoolMessage($"{surroundingEmote} {surroundingEmote} {surroundingEmote} {surroundingEmote}");
            TwitchClientManager.SpoolMessage($"{surroundingEmote} yyj1 yyj2 {surroundingEmote}");
            TwitchClientManager.SpoolMessage($"{surroundingEmote} yyj3 yyj4 {surroundingEmote}");
            TwitchClientManager.SpoolMessage($"{surroundingEmote} {surroundingEmote} {surroundingEmote} {surroundingEmote}");
        }
    }
}
