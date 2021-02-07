using Chubberino.Client.Abstractions;
using Chubberino.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Chubberino.Client.Commands
{
    public sealed class Permutations : Command
    {
        public Permutations(IExtendedClient client, TextWriter console) : base(client, console)
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
                TwitchClient.SpoolMessage(permutation);
            }
        }
    }
}
