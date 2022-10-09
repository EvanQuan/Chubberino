using System.IO;
using Chubberino.Infrastructure.Client;
using Chubberino.Infrastructure.Client.TwitchClients;
using Chubberino.Infrastructure.Commands;

namespace Chubberino.Bots.Common.Commands;

public sealed class Refresh : Command
{
    public IBot Bot { get; }

    public Refresh(ITwitchClientManager client, TextWriter writer, IBot bot)
        : base(client, writer)
    {
        Bot = bot;
    }


    public override void Execute(IEnumerable<String> arguments)
    {
        Bot.Refresh();
    }
}
