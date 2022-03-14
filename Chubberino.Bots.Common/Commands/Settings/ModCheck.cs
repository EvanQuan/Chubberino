using System;
using System.IO;
using Chubberino.Bots.Common.Commands.Settings.Strategies;
using Chubberino.Infrastructure.Client.TwitchClients;
using Chubberino.Infrastructure.Commands;
using Chubberino.Infrastructure.Commands.Settings;
using TwitchLib.Client.Events;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Bots.Common.Commands.Settings
{
    public sealed class ModCheck : Setting
    {
        public ICommandRepository Commands { get; }

        private IStopSettingStrategy StopSettingStrategy { get; }

        public ModCheck(
            ITwitchClientManager client,
            TextWriter writer,
            ICommandRepository commands,
            IStopSettingStrategy stopSettingStrategy)
            : base(client, writer)
        {
            Commands = commands;
            StopSettingStrategy = stopSettingStrategy;
        }

        public override void Register(ITwitchClient client)
        {
            client.OnMessageReceived += Client_OnMessageReceived;
        }

        public override void Unregister(ITwitchClient client)
        {
            client.OnMessageReceived -= Client_OnMessageReceived;
        }

        public void Client_OnMessageReceived(Object sender, OnMessageReceivedArgs e)
        {
            if (StopSettingStrategy.ShouldStop(e.ChatMessage))
            {
                Commands.DisableAllSettings();
                Writer.WriteLine("! ! ! DISABLED ALL SETTINGS ! ! !");
                Writer.WriteLine($"Moderator {e.ChatMessage.DisplayName} said: \"{e.ChatMessage.Message}\"");
            }
        }
    }
}
