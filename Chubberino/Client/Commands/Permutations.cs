using Chubberino.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chubberino.Client.Commands
{
    public sealed class Permutations : Command
    {
        public Permutations(ITwitchClientManager client, IConsole console)
            : base(client, console)
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
