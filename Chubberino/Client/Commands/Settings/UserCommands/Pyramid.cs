using Chubberino.Client.Commands.Pyramids;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chubberino.Client.Commands.Settings.UserCommands
{
    public sealed class Pyramid : UserCommand
    {
        private PyramidBuilder Builder { get; }

        public Pyramid(ITwitchClientManager client, IConsole console, PyramidBuilder builder)
            : base(client, console)
        {
            Builder = builder;
            Enable = twitchClient =>
            {
                twitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
            };
            Disable = twitchClient =>
            {
                twitchClient.OnMessageReceived -= TwitchClient_OnMessageReceived;
            };
        }

        private void TwitchClient_OnMessageReceived(Object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
        {
            if (!TryValidateCommand(e, out var words)) { return; }

            if (words.Count() < 2)
            {
                TwitchClientManager.SpoolMessage(e.ChatMessage.DisplayName + " Usage: !pyramid <size> <text>");
                return;
            }

            if (!Int32.TryParse(words.First(), out Int32 height) || height < 1)
            {
                TwitchClientManager.SpoolMessage($"{e.ChatMessage.DisplayName} Pyramid height of \"{words.First()}\" must be a positive integer");
            }

            IEnumerable<String> pyramidBlockArguments = words.Skip(1);

            String pyramidBlock = String.Join(' ', pyramidBlockArguments);

            var pyramid = Builder.GetPyramid(pyramidBlock, height);

            foreach (var line in pyramid)
            {
                TwitchClientManager.SpoolMessage(line);
            }
        }
    }
}
