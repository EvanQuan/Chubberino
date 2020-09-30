using Chubberino.Client.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Client.Commands
{
    public sealed class Say : Command
    {
        public Say(IExtendedClient client, TextWriter console)
            : base(client, console)
        {
        }

        public override void Execute(IEnumerable<String> arguments)
        {
            TwitchClient.SpoolMessage(String.Join(" ", arguments));
        }
    }
}
