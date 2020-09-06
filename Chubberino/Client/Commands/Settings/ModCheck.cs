using Chubberino.Client.Abstractions;
using Chubberino.Client.Commands.Strategies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TwitchLib.Client.Events;

namespace Chubberino.Client.Commands.Settings
{
    public sealed class ModCheck : Setting
    {
        private TextWriter TextWriter { get; }

        public ICommandRepository Commands { get; }

        private IStopSettingStrategy StopSettingStrategy { get; }

        public ModCheck(IExtendedClient client, TextWriter textWriter, ICommandRepository commands, IStopSettingStrategy stopSettingStrategy)
            : base(client)
        {
            Enable = twitchClient =>
            {
                twitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
            };

            Disable = twitchClient =>
            {
                twitchClient.OnMessageReceived -= TwitchClient_OnMessageReceived;
            };
            TextWriter = textWriter;
            Commands = commands;
            StopSettingStrategy = stopSettingStrategy;
        }

        private void TwitchClient_OnMessageReceived(Object sender, OnMessageReceivedArgs e)
        {
            if (StopSettingStrategy.ShouldStop(e.ChatMessage))
            {
                Commands.DisableAllSettings();
                TextWriter.WriteLine("! ! ! DISABLED ALL SETTINGS ! ! !");
                TextWriter.WriteLine($"Moderator {e.ChatMessage.DisplayName} said: \"{e.ChatMessage.Message}\"");
            }
        }
    }
}
