using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Chubberino.Common.Extensions;
using Chubberino.Common.Services;
using Chubberino.Infrastructure.Client;
using Chubberino.Infrastructure.Client.TwitchClients;
using Chubberino.Infrastructure.Commands.Settings;
using TwitchLib.Client.Events;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Bots.Common.Commands.Settings;

public sealed class Cookie : Setting
{
    private IRepeater Repeater { get; }
    private IDateTimeService DateTime { get; }
    private Boolean Responded { get; set; }

    public String Channel { get; private set; }

    public DateTime LastCookieTime { get; private set; }

    public override String Status => base.Status
        + $"\n\tChannel: {Channel}"
        + $"\n\tLast cookie: {LastCookieTime}";

    public Cookie(
        ITwitchClientManager client,
        IRepeater repeater,
        TextWriter writer,
        IDateTimeService dateTime)
        : base(client, writer)
    {
        Repeater = repeater;
        DateTime = dateTime;
        Repeater.Action = SpoolRepeatMessages;
        Repeater.Interval = TimeSpan.FromHours(1);
        Channel = "thepositivebot";
    }

    public override void Register(ITwitchClient client)
    {
        client.OnMessageReceived += TwitchClient_OnMessageReceived;
    }

    public override void Unregister(ITwitchClient client)
    {
        client.OnMessageReceived -= TwitchClient_OnMessageReceived;
    }

    private void TwitchClient_OnMessageReceived(Object sender, OnMessageReceivedArgs e)
    {
        if (e.ChatMessage.UserId == TwitchUserIDs.ThePositiveBot)
        {
            if (e.ChatMessage.Message.Contains(TwitchClientManager.Name.Value, StringComparison.OrdinalIgnoreCase))
            {
                Responded = true;
            }
        }
    }

    private void SpoolRepeatMessages()
    {
        Responded = false;
        while (!Responded)
        {
            TwitchClientManager.SpoolMessage(Channel, "!cookie");
            SpinWait.SpinUntil(() => Responded, TimeSpan.FromSeconds(5));
        }
        Responded = false;
        while (!Responded)
        {
            TwitchClientManager.SpoolMessage(Channel, "!cdr");
            SpinWait.SpinUntil(() => Responded, TimeSpan.FromSeconds(5));
        }
        Responded = false;
        while (!Responded)
        {
            TwitchClientManager.SpoolMessage(Channel, "!cookie");
            SpinWait.SpinUntil(() => Responded, TimeSpan.FromSeconds(5));
        }
        LastCookieTime = DateTime.Now;
    }

    public override void Execute(IEnumerable<String> arguments)
    {
        base.Execute(arguments);

        if (IsEnabled)
        {
            TwitchClientManager.EnsureJoinedToChannel(Channel);
        }

        Repeater.IsRunning = IsEnabled;
    }

    public override Boolean Set(String property, IEnumerable<String> arguments)
    {
        switch (property?.ToLower())
        {
            case "c":
            case "channel":
                if (arguments.TryGetFirst(out String channel))
                {
                    Channel = channel;
                    return true;
                }
                return false;
        }
        return false;
    }
}
