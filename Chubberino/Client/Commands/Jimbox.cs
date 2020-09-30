using Chubberino.Client.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Chubberino.Client.Commands
{
    public sealed class Jimbox : Command
    {
        public Jimbox(IExtendedClient client, TextWriter console)
            : base(client, console)
        {
        }

        /// <summary>Dank memes are cool</summary>
        /// <example></example>
        /// <param name="arguments"></param>
        public override void Execute(IEnumerable<String> arguments)
        {
            String surroundingEmote = arguments.FirstOrDefault() ?? "yyjW";

            TwitchClient.SpoolMessage($"{surroundingEmote} {surroundingEmote} {surroundingEmote} {surroundingEmote}");
            TwitchClient.SpoolMessage($"{surroundingEmote} yyj1 yyj2 {surroundingEmote}");
            TwitchClient.SpoolMessage($"{surroundingEmote} yyj3 yyj4 {surroundingEmote}");
            TwitchClient.SpoolMessage($"{surroundingEmote} {surroundingEmote} {surroundingEmote} {surroundingEmote}");
        }
    }
}
