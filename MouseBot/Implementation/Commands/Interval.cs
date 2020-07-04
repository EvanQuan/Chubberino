using MouseBot.Implementation.Abstractions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client.Interfaces;

namespace MouseBot.Implementation.Commands
{
    public sealed class Interval : Command
    {
        public Interval(ITwitchClient client, IMessageSpooler spooler)
            : base(client, spooler)
        {
        }

        public override void Execute(IEnumerable<String> arguments)
        {
            if (arguments.Count() == 0)
            {
                Console.WriteLine($"Message interval is {Spooler.Interval.TotalSeconds} seconds.");
                return;
            }

            if (Double.TryParse(arguments.FirstOrDefault(), out Double result))
            {
                Spooler.Interval = TimeSpan.FromSeconds(result);
                Console.WriteLine($"Message interval set to {result} seconds.");
            }
        }
    }
}
