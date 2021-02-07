using Chubberino.Client.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubberino.Client.Commands
{
    public sealed class Permutations : Command
    {
        public Permutations(IExtendedClient client, TextWriter console) : base(client, console)
        {
        }

        public override void Execute(IEnumerable<String> arguments)
        {
            if (arguments.Count() == 0)
            {
                Console.WriteLine(GetHelp());
                return;
            }


            throw new NotImplementedException();
        }
    }
}
