using System;
using System.IO;
using Chubberino.Bots.Channel.Translations;
using Chubberino.Client.Commands.Settings.UserCommands;
using Chubberino.Infrastructure.Client.TwitchClients;
using Chubberino.Infrastructure.Commands.Settings.UserCommands;
using Jering.Javascript.NodeJS;

namespace Chubberino.Bots.Channel.Commands;

public sealed class Translate : UserCommand
{
    private INodeJSService NodeService { get; }

    public Translate(ITwitchClientManager client, TextWriter writer, INodeJSService nodeService) : base(client, writer)
    {
        NodeService = nodeService;
    }

    public override void Invoke(Object sender, OnUserCommandReceivedArgs e)
    {
        String translatedText = NodeService.InvokeFromStringAsync<String>(moduleString: JavaScript.Translate, args: e.Words).Result;

        if (translatedText is not null)
        {
            TwitchClientManager.SpoolMessage(translatedText);
        }
    }
}
