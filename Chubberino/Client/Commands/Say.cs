using Chubberino.Client.Abstractions;
using System;
using System.Collections.Generic;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Client.Commands
{
    public sealed class Say : Command
    {
        public Say(IExtendedClient client)
            : base(client)
        {
        }

        public override void Execute(IEnumerable<String> arguments)
        {
            TwitchClient.SpoolMessage(String.Join(" ", arguments));
        }
    }
}
