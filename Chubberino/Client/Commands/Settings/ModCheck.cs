using Chubberino.Client.Abstractions;
using Chubberino.Client.Commands.Strategies;
using System;
using TwitchLib.Client.Events;

namespace Chubberino.Client.Commands.Settings
{
    public sealed class ModCheck : Setting
    {
        public ICommandRepository Commands { get; }

        private IStopSettingStrategy StopSettingStrategy { get; }

        public ModCheck(ITwitchClientManager client, IConsole console, ICommandRepository commands, IStopSettingStrategy stopSettingStrategy)
            : base(client, console)
        {
            Enable = twitchClient =>
            {
                twitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
            };

            Disable = twitchClient =>
            {
                twitchClient.OnMessageReceived -= TwitchClient_OnMessageReceived;
            };
            Commands = commands;
            StopSettingStrategy = stopSettingStrategy;
        }

        public void TwitchClient_OnMessageReceived(Object sender, OnMessageReceivedArgs e)
        {
            if (StopSettingStrategy.ShouldStop(e.ChatMessage))
            {
                Commands.DisableAllSettings();
                Console.WriteLine("! ! ! DISABLED ALL SETTINGS ! ! !");
                Console.WriteLine($"Moderator {e.ChatMessage.DisplayName} said: \"{e.ChatMessage.Message}\"");
            }
        }
    }
}
