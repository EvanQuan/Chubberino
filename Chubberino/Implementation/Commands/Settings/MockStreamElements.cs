using Chubberino.Implementation.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Implementation.Commands.Settings
{
    internal sealed class MockStreamElements : Copy
    {
        public MockStreamElements(ITwitchClient client, IMessageSpooler spooler)
            : base(client, spooler)
        {
            IsEnabled = false;
        }

        public override void Execute(IEnumerable<String> arguments)
        {
            base.Execute(arguments.Any()
                ? new List<String>() { "StreamElements", "mock" }.Concat(arguments)
                : new List<String>() { });
        }
    }
}
