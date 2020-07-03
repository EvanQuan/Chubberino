using MouseBot.Implementation.Abstractions;
using System;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Interfaces;

namespace MouseBot.Implementation.Commands.Settings
{
    internal sealed class RandomColors : Setting
    {
        private ChatColorPresets CurrentColor { get; set; }

        private Random Random { get; }

        public RandomColors(ITwitchClient client, IMessageSpooler spooler)
            : base(client, spooler)
        {
            TwitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
            Random = new Random();
        }

        private void TwitchClient_OnMessageReceived(Object sender, OnMessageReceivedArgs e)
        {
            if (IsEnabled)
            {
                if (e.ChatMessage.Username.Equals(TwitchInfo.BotUsername, StringComparison.OrdinalIgnoreCase))
                {
                    SetNextColor();
                    TwitchClient.ChangeChatColor(TwitchClient.JoinedChannels[0], CurrentColor);
                }
            }
        }

        /// <summary>
        /// Gets a random color that is different than the current color.
        /// </summary>
        private void SetNextColor()
        {
            Array values = Enum.GetValues(typeof(ChatColorPresets));
            ChatColorPresets randomColor;
            do
            {
                randomColor = (ChatColorPresets)values.GetValue(Random.Next(values.Length));
            }
            while (randomColor == CurrentColor);

            CurrentColor = randomColor;
        }
    }
}
