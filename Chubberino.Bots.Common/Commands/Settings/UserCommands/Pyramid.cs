﻿using System.IO;
using Chubberino.Bots.Common.Commands.Settings.Pyramids;
using Chubberino.Client.Commands.Settings.UserCommands;
using Chubberino.Infrastructure.Client;
using Chubberino.Infrastructure.Client.TwitchClients;
using Chubberino.Infrastructure.Commands.Settings.UserCommands;

namespace Chubberino.Bots.Common.Commands.Settings.UserCommands;

public sealed class Pyramid : UserCommand
{
    public Pyramid(ITwitchClientManager client, TextWriter writer)
        : base(client, writer)
    {
    }

    public override void Execute(IEnumerable<String> arguments)
    {
        if (!arguments.Any())
        {
            base.Execute(arguments);
            return;
        }

        if (!Int32.TryParse(arguments.First(), out Int32 height))
        {
            Writer.WriteLine($"Pyramid height of \"{arguments.First()}\" must be an integer");
            return;
        }


        IEnumerable<String> pyramidBlockArguments = arguments.Skip(1);

        if (!pyramidBlockArguments.Any())
        {
            Writer.WriteLine("Pyramid block not supplied.");
            return;
        }

        String pyramidBlock = String.Join(' ', pyramidBlockArguments);

        var pyramid = PyramidBuilder.Get(pyramidBlock, height);

        foreach (var line in pyramid)
        {
            TwitchClientManager.SpoolMessage(line);
        }
    }

    public override void Invoke(Object sender, OnUserCommandReceivedArgs e)
    {
        if (e.Words.Length < 2)
        {
            TwitchClientManager.SpoolMessage(e.ChatMessage.DisplayName + " Usage: !pyramid <size> <text>", Priority.Low);
            return;
        }

        if (!Int32.TryParse(e.Words[0], out Int32 height) || height < 1)
        {
            TwitchClientManager.SpoolMessage($"{e.ChatMessage.DisplayName} Pyramid height of \"{e.Words[0]}\" must be a positive integer");
        }

        IEnumerable<String> pyramidBlockArguments = e.Words[1..];

        String pyramidBlock = String.Join(' ', pyramidBlockArguments);

        var pyramid = PyramidBuilder.Get(pyramidBlock, height);

        foreach (var line in pyramid)
        {
            TwitchClientManager.SpoolMessage(line);
        }
    }

    public override String GetHelp()
        => @"
Creates a pyramid in chat.

usage: pyramid <height> <message>

    <height> - The count of <message> repeated at the tallest point in the pyramid.

    <message> - the text that is repeated within the pyramid.
                This can be composed of multiple words.
";
}
