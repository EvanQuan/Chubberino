using Chubberino.Database.Contexts;
using Chubberino.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client.Events;

namespace Chubberino.Client.Commands.Settings.UserCommands
{
    public sealed class Join : UserCommand
    {
        public const Int32 MaximumChannelsToJoin = 100;

        public IApplicationContext Context { get; }

        public Join(IApplicationContext context, ITwitchClientManager client, IConsole console) : base(client, console)
        {
            Context = context;

            TwitchClientManager.Client.OnJoinedChannel += TwitchClient_OnJoinedChannel;

            Enable = twitchClient => twitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
            Disable = twitchClient => twitchClient.OnMessageReceived -= TwitchClient_OnMessageReceived;

            IsEnabled = TwitchClientManager.IsBot;
        }

        public void TwitchClient_OnJoinedChannel(Object sender, OnJoinedChannelArgs e)
        {
            Console.WriteLine($"Joined channel {e.Channel}");
        }

        private void TwitchClient_OnMessageReceived(Object sender, OnMessageReceivedArgs e)
        {
            if (!TryValidateCommand(e, out var words)) { return; }

            if (!words.Any() || !words.First().Equals(e.ChatMessage.Username, StringComparison.OrdinalIgnoreCase))
            {
                TwitchClientManager.Client.SpoolMessage(e.ChatMessage.Channel, $"{e.ChatMessage.DisplayName} I cannot join other users' channels for you.");
                return;
            }

            // Users can only add their own channel
            var channelFound = Context.StartupChannels.FirstOrDefault(x => x.UserID == e.ChatMessage.UserId);

            String outputMessage;
            if (channelFound == null)
            {
                if (Context.StartupChannels.Count() >= MaximumChannelsToJoin)
                {
                    outputMessage = $"{e.ChatMessage.DisplayName} I have reached the maximum number of channels to join and could not join your channel.";
                }
                else
                {
                    // Add new channel.
                    Context.StartupChannels.Add(new StartupChannel()
                    {
                        UserID = e.ChatMessage.UserId,
                        DisplayName = e.ChatMessage.DisplayName
                    });

                    outputMessage = $"{e.ChatMessage.DisplayName} I have now joined your channel.";
                }
            }
            else
            {
                // Update the display name
                channelFound.DisplayName = e.ChatMessage.DisplayName;

                if (channelFound.DisplayName == e.ChatMessage.DisplayName)
                {
                    outputMessage = $"{e.ChatMessage.DisplayName} I have already joined your channel.";
                }
                else
                {
                    outputMessage = $"{e.ChatMessage.DisplayName} I have already joined your channel. Updated user name.";
                }

                // Update the display name
                channelFound.DisplayName = e.ChatMessage.DisplayName;
            }


            Context.SaveChanges();

            TwitchClientManager.Client.JoinChannel(e.ChatMessage.Username);

            TwitchClientManager.Client.SpoolMessage(e.ChatMessage.Channel, outputMessage);
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

            TwitchClientManager.Client.EnsureJoinedToChannel(channelName);
        }

        public override string GetHelp()
        {
            return @"
Join a twitch channel.

usage: join <channel name>

    <channel name>                      Name of the twitch channel to join.
";
        }
    }
}
