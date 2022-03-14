using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Chubberino.Infrastructure.Client.TwitchClients;
using Chubberino.Infrastructure.Commands;

namespace Chubberino.Bots.Common.Commands
{
    public sealed class Jimbox : Command
    {
        public Jimbox(ITwitchClientManager client, TextWriter writer)
            : base(client, writer)
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
