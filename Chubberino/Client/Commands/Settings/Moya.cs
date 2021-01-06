using Chubberino.Client.Abstractions;
using Chubberino.Client.Commands.Settings.UserCommands.Translations;
using Jering.Javascript.NodeJS;
using System;
using System.IO;
using System.Linq;
using TwitchLib.Client.Events;

namespace Chubberino.Client.Commands.Settings
{
    public sealed class Moya : Setting
    {
        private const String ListenChannel = "jinnytty";

        private const String ListenUsername = "moya0806";

        private INodeJSService NodeService { get; }

        public Moya(IExtendedClient client, TextWriter console, INodeJSService nodeService)
            : base(client, console)
        {
            Enable = twitchClient =>
            {
                twitchClient.JoinChannel(ListenChannel);
                twitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
            };

            Disable = twitchClient =>
            {
                twitchClient.LeaveChannel(ListenChannel);
                twitchClient.OnMessageReceived -= TwitchClient_OnMessageReceived;
            };

            NodeService = nodeService;
        }


        private void TwitchClient_OnMessageReceived(Object sender, OnMessageReceivedArgs e)
        {
            if (e.ChatMessage.Channel != ListenChannel) { return; }
            if (e.ChatMessage.Username != ListenUsername) { return; }

            String translatedText = NodeService.InvokeFromStringAsync<String>(
                moduleString: JavaScript.Translate,
                args: e.ChatMessage.Message.Split(' ').ToArray()).Result;

            if (translatedText != null)
            {
                if (translatedText == e.ChatMessage.Message)
                {
                    TwitchClient.SpoolMessage($"TearChub New {ListenUsername} message in {ListenChannel}'s chat!");
                    TwitchClient.SpoolMessage("Message: " + e.ChatMessage.Message);
                }
                else
                {
                    TwitchClient.SpoolMessage($"TearChub New non-english {ListenUsername} message in {ListenChannel}'s chat!");
                    TwitchClient.SpoolMessage("Original message: " + e.ChatMessage.Message);
                    TwitchClient.SpoolMessage("Translated message: " + translatedText);
                    String replacedMessage = e.ChatMessage.Message.Replace(" ", "%20");
                    TwitchClient.SpoolMessage("View on google translate: https://translate.google.com/?um=1&ie=UTF-8&hl=en&client=tw-ob#view=home&op=translate&sl=zh-CN&tl=en&text=" + replacedMessage);
                }
            }
        }
    }
}
