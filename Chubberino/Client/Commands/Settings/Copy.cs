using Chubberino.Client.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client.Events;

namespace Chubberino.Client.Commands.Settings
{
    public class Copy : Setting
    {
        private String UserToMirror { get; set; }

        private String MessagePrefix { get; set; }

        private enum CopyMode
        {
            Default = 0,
            Mock = 1,
            Reverse = 2
        }

        private CopyMode Mode { get; set; }

        public override String Status => IsEnabled
            ? $"{UserToMirror} Mode: {Mode} Prefix: {MessagePrefix}"
            : "disabled";

        public Copy(ITwitchClientManager client, IConsole console)
            : base(client, console)
        {
            Enable = twitchClient =>
            {
                TwitchClientManager.Client.OnMessageReceived += TwitchClient_OnMessageReceived;
            };

            Disable = twitchClient =>
            {
                TwitchClientManager.Client.OnMessageReceived -= TwitchClient_OnMessageReceived;
            };
        }

        private void TwitchClient_OnMessageReceived(Object sender, OnMessageReceivedArgs e)
        {
            if (!e.ChatMessage.Username.Equals(UserToMirror, StringComparison.OrdinalIgnoreCase)) { return; }

            String replacedUsernameMessage =
                // Modify message to mentions of bot username become the username
                // of the one copied.
                e.ChatMessage.Username.Equals("streamelements", StringComparison.OrdinalIgnoreCase)
                ? e.ChatMessage.Message
                : e.ChatMessage.Message.Replace(e.ChatMessage.BotUsername, e.ChatMessage.Username, StringComparison.OrdinalIgnoreCase);

            String modeModifiedMessage = String.Empty;

            switch (Mode)
            {
                case CopyMode.Default:
                    modeModifiedMessage = replacedUsernameMessage;
                    break;
                case CopyMode.Mock:
                    modeModifiedMessage = "" + replacedUsernameMessage.ToRandomCase();
                    break;
                case CopyMode.Reverse:
                    modeModifiedMessage = String.Join(String.Empty, replacedUsernameMessage.Reverse());
                    break;
            }

            String prefixAddedMessage = (String.IsNullOrWhiteSpace(MessagePrefix)
                ? String.Empty
                : MessagePrefix + " ")
                + modeModifiedMessage;

            // Shorten message to abide by message length limit (for non-VIPs).
            const Int32 messageCharacterLimit = 300;
            String truncatedMessage = prefixAddedMessage.Substring(0, Math.Min(prefixAddedMessage.Length, messageCharacterLimit));

            TwitchClientManager.SpoolMessage(truncatedMessage);
        }

        public override void Execute(IEnumerable<String> arguments)
        {
            if (arguments.Count() == 0)
            {
                UserToMirror = null;
                Console.WriteLine("Copy disabled");
                IsEnabled = false;
                return;
            }

            UserToMirror = arguments.FirstOrDefault();

            if (arguments.Count() > 1)
            {
                Mode = (arguments.Skip(1).FirstOrDefault()?.ToLower()) switch
                {
                    "m" => CopyMode.Mock,
                    "mock" => CopyMode.Mock,
                    "r" => CopyMode.Reverse,
                    "reverse" => CopyMode.Reverse,
                    _ => CopyMode.Default,
                };
            }
            else
            {
                Mode = CopyMode.Default;
            }

            MessagePrefix = String.Join(" ", arguments.Skip(Mode == CopyMode.Default ? 1 : 2));
            Console.WriteLine($"Copying user \"{UserToMirror}\" Mode: \"{Mode}\" Prefix: \"{MessagePrefix}\"");

            IsEnabled = true;
        }

        public override String GetHelp()
        {
            return @"
Copy another user's chat messages.

usage: copy <username> [mode] [message prefix]

    <username>  The Twitch username to copy.

    [mode]      default - Copy messages exactly.
                mock - Randomly use upper and lowercase letters.
                reverse - Reverse the message.

    [prefix]    Text to prepend to the copied messages.
";
        }
    }
}
