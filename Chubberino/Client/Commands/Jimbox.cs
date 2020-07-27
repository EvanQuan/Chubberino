using Chubberino.Client.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Client.Commands
{
    public sealed class Jimbox : Command
    {
        public Jimbox(ITwitchClient client, IMessageSpooler spooler)
            : base(client, spooler)
        {
        }

        public override void Execute(IEnumerable<String> arguments)
        {
            String surroundingEmote = arguments.FirstOrDefault() ?? "yyjW";

            Spooler.SpoolMessage($"{surroundingEmote} {surroundingEmote} {surroundingEmote} {surroundingEmote}");
            Spooler.SpoolMessage($"{surroundingEmote} yyj1 yyj2 {surroundingEmote}");
            Spooler.SpoolMessage($"{surroundingEmote} yyj3 yyj4 {surroundingEmote}");
            Spooler.SpoolMessage($"{surroundingEmote} {surroundingEmote} {surroundingEmote} {surroundingEmote}");
        }
    }
}
