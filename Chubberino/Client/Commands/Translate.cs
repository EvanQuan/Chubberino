using Chubberino.Client.Abstractions;
using Chubberino.Client.Commands.Settings.UserCommands;
using Chubberino.Client.Commands.Settings.UserCommands.Translations;
using Jering.Javascript.NodeJS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TwitchLib.Client.Events;

namespace Chubberino.Client.Commands
{
    public sealed class Translate : UserCommand
    {
        private INodeJSService NodeService { get; }

        public Translate(IExtendedClient client, TextWriter console, INodeJSService nodeService) : base(client, console)
        {
            NodeService = nodeService;

            Enable = twitchClient =>
            {
                twitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
            };
            Disable = twitchClient =>
            {
                twitchClient.OnMessageReceived -= TwitchClient_OnMessageReceived;
            };
        }

        private void TwitchClient_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            String message = e.ChatMessage.Message;
            if (!message.StartsWith("!translate")) { return; }

            IEnumerable<String> words = message.Split(' ').Skip(1);

            String translatedText = NodeService.InvokeFromStringAsync<String>(moduleString: JavaScript.Translate, args: words.ToArray()).Result;

            if (translatedText != null)
            {
                TwitchClient.SpoolMessage(translatedText);
            }
        }
    }
}
