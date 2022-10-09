using System.IO;
using Chubberino.Infrastructure.Client.TwitchClients;
using Chubberino.Infrastructure.Commands.Settings;
using TwitchLib.Client.Events;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Bots.Common.Commands.Settings;

public sealed class Log : Setting
{
    public Log(ITwitchClientManager client, TextWriter writer)
        : base(client, writer)
    {
    }

    public override void Register(ITwitchClient client)
    {
        client.OnLog += TwitchClient_OnLog;
    }

    public override void Unregister(ITwitchClient client)
    {
        client.OnLog -= TwitchClient_OnLog;
    }

    public void TwitchClient_OnLog(Object sender, OnLogArgs e)
    {
        Writer.WriteLine($"{e.DateTime}: {e.BotUsername} - {e.Data}");
    }
}
