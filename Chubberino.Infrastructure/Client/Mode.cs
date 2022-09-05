using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Chubberino.Infrastructure.Client.TwitchClients;
using Chubberino.Infrastructure.Commands;

namespace Chubberino.Infrastructure.Client;

public sealed class Mode : Command
{
    private IBot Bot { get; }
    public IModeratorClientOptions ModeratorClientOptions { get; }
    public IRegularClientOptions RegularClientOptions { get; }

    public Mode(
        ITwitchClientManager client,
        TextWriter writer,
        IBot bot,
        IModeratorClientOptions moderatorClientOptions,
        IRegularClientOptions regularClientOptions)
        : base(client, writer)
    {
        Bot = bot;
        ModeratorClientOptions = moderatorClientOptions;
        RegularClientOptions = regularClientOptions;
    }

    public override void Execute(IEnumerable<String> arguments)
    {
        switch (arguments?.FirstOrDefault())
        {
            case "m":
            case "mod":
            case "moderator":
                Bot.Refresh(ModeratorClientOptions);
                Bot.IsModerator = true;
                break;
            case "n":
            case "normal":
                Bot.Refresh(RegularClientOptions);
                Bot.IsModerator = false;
                break;

        }
    }
}
