using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Chubberino.Client.Commands.Settings.UserCommands;
using Chubberino.Database.Contexts;
using Chubberino.Database.Models;
using Chubberino.Infrastructure.Client.TwitchClients;
using Chubberino.Infrastructure.Commands.Settings.UserCommands;

namespace Chubberino.Bots.Channel.Commands;

public sealed class Join : UserCommand
{
    public const Int32 MaximumChannelsToJoin = 100;

    public IApplicationContextFactory ContextFactory { get; }

    public Join(IApplicationContextFactory contextFactory, ITwitchClientManager client, TextWriter writer) : base(client, writer)
    {
        ContextFactory = contextFactory;
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

        TwitchClientManager.EnsureJoinedToChannel(channelName);
    }

    public override String GetHelp()
    {
        return @"
Join a twitch channel.

usage: join <channel name>

    <channel name>                      Name of the twitch channel to join.
";
    }

    public async override void Invoke(Object sender, OnUserCommandReceivedArgs e)
    {
        if (!e.Words.Any() || !e.Words.First().Equals(e.ChatMessage.Username, StringComparison.OrdinalIgnoreCase))
        {
            TwitchClientManager.SpoolMessageAsMe(e.ChatMessage.Channel, $"{e.ChatMessage.DisplayName} I cannot join other users' channels for you.");
            return;
        }

        using var context = ContextFactory.GetContext();

        // Users can only add their own channel
        var channelFound = context.StartupChannels.FirstOrDefault(x => x.UserID == e.ChatMessage.UserId);

        String outputMessage;
        if (channelFound == null)
        {
            if (context.StartupChannels.Count() >= MaximumChannelsToJoin)
            {
                outputMessage = $"{e.ChatMessage.DisplayName} I have reached the maximum number of channels to join and could not join your channel.";
            }
            else
            {
                // Add new channel.
                context.StartupChannels.Add(new StartupChannel()
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


        var task = context.SaveChangesAsync();

        TwitchClientManager.Client.JoinChannel(e.ChatMessage.Username);

        TwitchClientManager.SpoolMessageAsMe(e.ChatMessage.Channel, outputMessage);

        await task;
    }
}
