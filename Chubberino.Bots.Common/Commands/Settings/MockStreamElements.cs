using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Chubberino.Infrastructure.Client.TwitchClients;

namespace Chubberino.Bots.Common.Commands.Settings;

public sealed class MockStreamElements : Copy
{
    public MockStreamElements(ITwitchClientManager client, TextWriter writer)
        : base(client, writer)
    {
    }

    public override void Execute(IEnumerable<String> arguments)
    {
        base.Execute(arguments.Any()
            ? new List<String>() { "StreamElements", "mock" }.Concat(arguments)
            : new List<String>() { });
    }
}
