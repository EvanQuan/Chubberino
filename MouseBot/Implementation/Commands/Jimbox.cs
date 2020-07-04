using MouseBot.Implementation.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchLib.Client.Interfaces;

namespace MouseBot.Implementation.Commands
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

            Spooler.SpoolMessage($"{surroundingEmote} {surroundingEmote} {surroundingEmote} {surroundingEmote}", Priority.High);
            Spooler.SpoolMessage($"{surroundingEmote} yyj1 yyj2 {surroundingEmote}", Priority.High);
            Spooler.SpoolMessage($"{surroundingEmote} yyj3 yyj4 {surroundingEmote}", Priority.High);
            Spooler.SpoolMessage($"{surroundingEmote} {surroundingEmote} {surroundingEmote} {surroundingEmote}", Priority.High);
        }
    }
}
