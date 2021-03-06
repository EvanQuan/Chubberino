﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Chubberino.Client.Commands.Settings
{
    internal sealed class MockStreamElements : Copy
    {
        public MockStreamElements(ITwitchClientManager client, IConsole console)
            : base(client, console)
        {
        }

        public override void Execute(IEnumerable<String> arguments)
        {
            base.Execute(arguments.Any()
                ? new List<String>() { "StreamElements", "mock" }.Concat(arguments)
                : new List<String>() { });
        }
    }
}
