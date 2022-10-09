using System.IO;
using Chubberino.Infrastructure.Client.TwitchClients;
using Chubberino.Infrastructure.Commands;

namespace Chubberino.Bots.Common.Commands;

public sealed class DisableAll : Command
{
    public ICommandRepository Commands { get; }

    public DisableAll(ITwitchClientManager client, ICommandRepository commands, TextWriter writer)
        : base(client, writer)
    {
        Commands = commands;
    }


    public override void Execute(IEnumerable<String> arguments)
    {
        if ('u' == (arguments.FirstOrDefault()?[0] ?? default))
        {
            Commands.DisableAllUserCommands();
            Writer.WriteLine("Disabled all user commands.");
        }
        else
        {
            Commands.DisableAllSettings();
            Writer.WriteLine("Disabled all settings.");
        }

    }

    public override String GetHelp()
    {
        return @"
Disables all settings.

usage: disableall [type]

    [type]  default - All settings
            u - All user commands
";
    }
}
