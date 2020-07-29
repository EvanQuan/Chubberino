using Chubberino.Client.Abstractions;
using Chubberino.Client.Commands.Settings.Colors;
using System;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Client.Commands.Settings
{
    /// <summary>
    /// Sets the user color to a random <see cref="ChatColorPresets"/> after
    /// each message. Since setting the color requires sending a message, the
    /// color may not change quick enough if messages are being sent near the
    /// throttle limit.
    /// </summary>
    internal sealed class Color : Setting
    {
        private IEnumerable<IColorSelector> Selectors { get; }

        private String CurrentColor { get; set; }

        private Random Random { get; }

        private IColorSelector CurrentSelector { get; set; }

        public override String Status => base.Status
            + $"\n\tSelector: {CurrentSelector.Name}";

        public Color(ITwitchClient client, IMessageSpooler spooler)
            : base(client, spooler)
        {
            TwitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
            TwitchClient.OnMessageSent += TwitchClient_OnMessageSent;
            Random = new Random();
            Selectors = new List<IColorSelector>()
            {
                new RandomColorSelector(Random, () => CurrentColor),
                new PresetColorSelector(Random, () => CurrentColor),
                new RainbowColorSelector(),
            };
            CurrentSelector = Selectors.FirstOrDefault();
        }

        /// <summary>
        /// When bot sends a message.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TwitchClient_OnMessageSent(Object sender, OnMessageSentArgs e)
        {
            if (!IsEnabled) { return; }

            // Avoid infinite recursion.
            if (e.SentMessage.Message.StartsWith('.')) { return; }

            ChangeColor();
        }

        /// <summary>
        /// When message is sent manually.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TwitchClient_OnMessageReceived(Object sender, OnMessageReceivedArgs e)
        {
            if (!IsEnabled) { return; }

            if (e.ChatMessage.Username.Equals(TwitchInfo.BotUsername, StringComparison.OrdinalIgnoreCase))
            {
                ChangeColor();
            }
        }
        
        private void ChangeColor()
        {
            CurrentColor = CurrentSelector.GetNextColor();

            Spooler.SpoolMessage($".color {CurrentColor}");
        }

        public override Boolean Set(String property, IEnumerable<String> arguments)
        {
            switch (property.ToLower())
            {
                case "selector":
                    IColorSelector proposedSelector = Selectors
                        .Where(x => x.Name == arguments.FirstOrDefault())
                        .FirstOrDefault();

                    if (proposedSelector == null)
                    {
                        return false;
                    }

                    CurrentSelector = proposedSelector;
                    return true;
                default:
                    return false;
            }
        }
    }
}
