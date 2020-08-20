using Chubberino.Client.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chubberino.Client.Commands.Settings
{
    internal sealed class MockStreamElements : Copy
    {
        public MockStreamElements(IExtendedClient client)
            : base(client)
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
