using Chubberino.Client.Commands.Pyramids;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chubberino.Client.Commands
{
    public sealed class PyramidBuild : Command
    {
        private PyramidBuilder Builder { get; set; }

        public PyramidBuild(ITwitchClientManager client, IConsole console, PyramidBuilder builder)
            : base(client, console)
        {
            Builder = builder;
        }

        public override void Execute(IEnumerable<String> arguments)
        {
            if (!arguments.Any())
            {
                Console.WriteLine(GetHelp());
                return;
            }

            if (!Int32.TryParse(arguments.First(), out Int32 height))
            {
                Console.WriteLine($"Pyramid height of \"{arguments.First()}\" must be an integer");
                return;
            }


            IEnumerable<String> pyramidBlockArguments = arguments.Skip(1);

            if (!pyramidBlockArguments.Any())
            {
                Console.WriteLine("Pyramid block not supplied.");
                return;
            }

            String pyramidBlock = String.Join(' ', pyramidBlockArguments);

            var pyramid = Builder.GetPyramid(pyramidBlock, height);

            foreach (var line in pyramid)
            {
                TwitchClientManager.SpoolMessage(line);
            }
        }

        public override String GetHelp()
        {
            return @"
Creates a pyramid in chat.

usage: pyramid <height> <message>

    <height> - The count of <message> repeated at the tallest point in the pyramid.

    <message> - the text that is repeated within the pyramid.
                This can be composed of multiple words.
";
        }

    }
}
