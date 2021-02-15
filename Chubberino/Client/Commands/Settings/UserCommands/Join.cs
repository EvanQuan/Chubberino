using Chubberino.Client.Abstractions;
using Chubberino.Database.Contexts;
using Chubberino.Database.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TwitchLib.Client.Events;

namespace Chubberino.Client.Commands.Settings.UserCommands
{
    public sealed class Join : UserCommand
    {
        public ApplicationContext Context { get; }

        public Join(ApplicationContext context, IExtendedClient client, TextWriter console) : base(client, console)
        {
            Context = context;

            TwitchClient.OnJoinedChannel += TwitchClient_OnJoinedChannel;

            Enable = twitchClient => twitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
            Disable = twitchClient => twitchClient.OnMessageReceived -= TwitchClient_OnMessageReceived;

            IsEnabled = true;
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
                TwitchClient.SpoolMessage(e.ChatMessage.Channel, $"{e.ChatMessage.DisplayName} I cannot join other users' channels for you.");
                return;
            }

            // Users can only add their own channel
            var channelFound = Context.StartupChannels.FirstOrDefault(x => x.UserID == e.ChatMessage.UserId);

            String outputMessage;
            if (channelFound == null)
            {
                // Add new channel.
                Context.StartupChannels.Add(new StartupChannel()
                {
                    UserID = e.ChatMessage.UserId,
                    DisplayName = e.ChatMessage.DisplayName
                });

                outputMessage = $"{e.ChatMessage.DisplayName} I have now joined your channel.";
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

            TwitchClient.JoinChannel(e.ChatMessage.Username);

            TwitchClient.SpoolMessage(e.ChatMessage.Channel, outputMessage);
        }

        public override void Execute(IEnumerable<String> arguments)
        {
            if (!arguments.Any()) { return; }

            if (!TwitchClient.IsConnected)
            {
                TwitchClient.Connect();
            }

            String channelName = arguments.FirstOrDefault();

            // If the user inputs any second argument, it will join that channel and not leave the existing channel.

            TwitchClient.EnsureJoinedToChannel(channelName);
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
