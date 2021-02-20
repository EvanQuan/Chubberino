using Chubberino.Client.Abstractions;
using Chubberino.Client.Commands.Settings.UserCommands;
using Chubberino.Client.Commands.Settings.UserCommands.Translations;
using Jering.Javascript.NodeJS;
using System;
using System.IO;
using System.Linq;
using TwitchLib.Client.Events;

namespace Chubberino.Client.Commands
{
    public sealed class Translate : UserCommand
    {
        private INodeJSService NodeService { get; }

        public Translate(ITwitchClientManager client, IConsole console, INodeJSService nodeService) : base(client, console)
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
            if (!TryValidateCommand(e, out var words)) { return; }

            String translatedText = NodeService.InvokeFromStringAsync<String>(moduleString: JavaScript.Translate, args: words.ToArray()).Result;

            if (translatedText != null)
            {
                TwitchClientManager.SpoolMessage(translatedText);
            }
        }
    }
}
