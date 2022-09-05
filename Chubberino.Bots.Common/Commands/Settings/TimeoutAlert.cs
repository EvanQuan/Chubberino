using System;
using System.IO;
using Chubberino.Infrastructure.Client.TwitchClients;
using Chubberino.Infrastructure.Commands.Settings;
using TwitchLib.Client.Events;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Bots.Common.Commands.Settings;

public sealed class TimeoutAlert : Setting
{
    public TimeoutAlert(ITwitchClientManager client, TextWriter writer)
        : base(client, writer)
    {
    }

    public override void Register(ITwitchClient client)
    {
        client.OnUserTimedout += TwitchClient_OnUserTimedout;
    }

    public override void Unregister(ITwitchClient client)
    {
        client.OnUserTimedout -= TwitchClient_OnUserTimedout;
    }

    public void TwitchClient_OnUserTimedout(Object sender, OnUserTimedoutArgs e)
    {
        TwitchClientManager.SpoolMessage($"WideHardo FREE MY MAN {e.UserTimeout.Username.ToUpper()}");
    }
}
