using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Chubberino.Common.Extensions;
using Chubberino.Infrastructure.Client.TwitchClients;
using Chubberino.Infrastructure.Commands;

namespace Chubberino.Client.Commands
{
    public sealed class Permutations : Command
    {
        public Permutations(ITwitchClientManager client, TextWriter writer)
            : base(client, writer)
        {
        }

        public override void Execute(IEnumerable<String> arguments)
        {
            if (!arguments.Any())
            {
                Console.WriteLine(GetHelp());
                return;
            }

            var permutations = arguments.First().GetPermutations();

            foreach (var permutation in permutations)
            {
                TwitchClientManager.SpoolMessage(permutation);
            }
        }
    }
}
