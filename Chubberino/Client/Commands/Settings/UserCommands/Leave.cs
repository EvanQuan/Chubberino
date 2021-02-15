using Chubberino.Client.Abstractions;
using Chubberino.Client.Commands.Settings.UserCommands;
using Chubberino.Database.Contexts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TwitchLib.Client.Events;

namespace Chubberino.Client.Commands
{
    public sealed class Leave : UserCommand
    {
        public ApplicationContext Context { get; }

        private IBot Bot { get; }

        public Leave(ApplicationContext context, IExtendedClient client, TextWriter console, IBot bot) : base(client, console)
        {
            TwitchClient.OnLeftChannel += TwitchClient_OnLeftChannel;
            Context = context;
            Bot = bot;

            Enable = twitchClient => twitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
            Disable = TwitchClient => TwitchClient.OnMessageReceived -= TwitchClient_OnMessageReceived;

            IsEnabled = true;
        }

        private void TwitchClient_OnMessageReceived(Object sender, OnMessageReceivedArgs e)
        {
            if (!TryValidateCommand(e, out var words)) { return; }

            if (!words.Any() || !words.First().Equals(e.ChatMessage.Username, StringComparison.OrdinalIgnoreCase))
            {
                TwitchClient.SpoolMessage(e.ChatMessage.Channel, $"{e.ChatMessage.DisplayName} I cannot leave other users' channels for you.");
                return;
            }

            // Users can only leave their own channel.
            var channelFound = Context.StartupChannels.FirstOrDefault(x => x.UserID == e.ChatMessage.UserId);

            if (channelFound != null)
            {
                Context.StartupChannels.Remove(channelFound);
                Context.SaveChanges();
            }

            TwitchClient.LeaveChannel(e.ChatMessage.Username);

            TwitchClient.SpoolMessage(e.ChatMessage.Channel, $"{e.ChatMessage.DisplayName} I have left your channel");
        }

        private void TwitchClient_OnLeftChannel(Object sender, OnLeftChannelArgs e)
        {
            Console.WriteLine($"Left channel {e.Channel}");

            if (e.Channel == Bot.PrimaryChannelName)
            {
                Bot.PrimaryChannelName = TwitchClient.JoinedChannels.FirstOrDefault()?.Channel ?? null;
                Console.WriteLine($"Primary channel updated to {Bot.PrimaryChannelName}");
            }

        }

        public override void Execute(IEnumerable<String> arguments)
        {
            if (arguments.Count() == 0) { return; }

            if (!TwitchClient.IsConnected)
            {
                TwitchClient.Connect();
            }

            String channelName = arguments.FirstOrDefault();

            // If the user inputs any second argument, it will join that channel and not leave the existing channel.

            TwitchClient.LeaveChannel(channelName);
        }

        public override string GetHelp()
        {
            return @"
Leave a twitch channel.

usage: leave <channel name>

    <channel name>                      Name of the twitch channel to leave.
";
        }
    }
}
