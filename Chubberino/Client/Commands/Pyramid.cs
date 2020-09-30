using Chubberino.Client.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Chubberino.Client.Commands
{
    public sealed class Pyramid : Command
    {
        private Int32 MaximumPyramidHeight { get; set; }

        private Int32 CurrentPyramidHeight { get; set; }

        public Pyramid(IExtendedClient client, TextWriter console)
            : base(client, console)
        {
        }

        public override void Execute(IEnumerable<String> arguments)
        {
            if (arguments.Count() == 0)
            {
                Console.WriteLine(GetHelp());
                return;
            }

            if (!Int32.TryParse(arguments.First(), out Int32 result))
            {
                Console.WriteLine($"Pyramid height of \"{arguments.First()}\" must be an integer");
                return;
            }


            IEnumerable<String> pyramidBlockArguments = arguments.Skip(1);

            if (pyramidBlockArguments.Count() == 0)
            {
                Console.WriteLine("Pyramid block not supplied.");
                return;
            }

            String pyramidBlock = String.Join(' ', pyramidBlockArguments);

            MaximumPyramidHeight = result;
            CurrentPyramidHeight = 0;

            // Build pyramid up
            while (CurrentPyramidHeight < MaximumPyramidHeight)
            {
                String message = String.Join(' ', Enumerable.Repeat(pyramidBlock, ++CurrentPyramidHeight));
                TwitchClient.SpoolMessage(message);
            }

            // Build pyramid down
            while (CurrentPyramidHeight > 0)
            {
                String message = String.Join(' ', Enumerable.Repeat(pyramidBlock, --CurrentPyramidHeight));
                TwitchClient.SpoolMessage(message);
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
