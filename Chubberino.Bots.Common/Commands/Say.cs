using System;
using System.Collections.Generic;
using System.IO;
using Chubberino.Infrastructure.Client.TwitchClients;
using Chubberino.Infrastructure.Commands;

namespace Chubberino.Bots.Common.Commands;

public sealed class Say : Command
{
    public Say(ITwitchClientManager client, TextWriter writer)
        : base(client, writer)
    {
    }

    public override void Execute(IEnumerable<String> arguments)
    {
        TwitchClientManager.SpoolMessage(String.Join(' ', arguments));
    }
}
