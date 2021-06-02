using Chubberino.Client.Commands.Settings.UserCommands;
using Chubberino.Database.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitchLib.Client.Events;

namespace Chubberino.Client.Commands
{
    public sealed class Leave : UserCommand
    {
        public IApplicationContextFactory ContextFactory { get; }

        public Leave(IApplicationContextFactory contextFactory, ITwitchClientManager client, IConsole console) : base(client, console)
        {
            TwitchClientManager.Client.OnLeftChannel += TwitchClient_OnLeftChannel;
            ContextFactory = contextFactory;

            Enable = twitchClient => twitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
            Disable = TwitchClient => TwitchClient.OnMessageReceived -= TwitchClient_OnMessageReceived;

            IsEnabled = TwitchClientManager.IsBot;
        }

        private async void TwitchClient_OnMessageReceived(Object sender, OnMessageReceivedArgs e)
        {
            if (!TryValidateCommand(e, out var words)) { return; }

            if (!words.Any() || !words.First().Equals(e.ChatMessage.Username, StringComparison.OrdinalIgnoreCase))
            {
                TwitchClientManager.SpoolMessageAsMe(e.ChatMessage.Channel, $"{e.ChatMessage.DisplayName} I cannot leave other users' channels for you.");
                return;
            }

            var context = ContextFactory.GetContext();

            // Users can only leave their own channel.
            var channelFound = context.StartupChannels.FirstOrDefault(x => x.UserID == e.ChatMessage.UserId);

            Task<Int32> task = null;

            if (channelFound is not null)
            {
                context.StartupChannels.Remove(channelFound);
                task = context.SaveChangesAsync();
            }

            TwitchClientManager.Client.LeaveChannel(e.ChatMessage.Username);

            TwitchClientManager.SpoolMessageAsMe(e.ChatMessage.Channel, $"{e.ChatMessage.DisplayName} I have left your channel");

            if (task is not null)
            {
                await task;
            }
        }

        private void TwitchClient_OnLeftChannel(Object sender, OnLeftChannelArgs e)
        {
            Console.WriteLine($"Left channel {e.Channel}");

            if (e.Channel == TwitchClientManager.PrimaryChannelName)
            {
                TwitchClientManager.PrimaryChannelName = TwitchClientManager.Client.JoinedChannels?[0].Channel ?? null;
                Console.WriteLine($"Primary channel updated to {TwitchClientManager.PrimaryChannelName}");
            }

        }

        public override void Execute(IEnumerable<String> arguments)
        {
            if (!arguments.Any()) { return; }

            if (!TwitchClientManager.Client.IsConnected)
            {
                TwitchClientManager.Client.Connect();
            }

            String channelName = arguments.FirstOrDefault();

            // If the user inputs any second argument, it will join that channel and not leave the existing channel.

            TwitchClientManager.Client.LeaveChannel(channelName);
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
