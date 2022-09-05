using System.IO;
using Chubberino.Infrastructure.Client.TwitchClients;
using Chubberino.Infrastructure.Commands.Settings;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings;

/// <summary>
/// Extends <see cref="Setting"/> to be instantiated.
/// </summary>
public sealed class SettingExtender : Setting
{
    public SettingExtender(ITwitchClientManager client, TextWriter writer)
        : base(client, writer)
    {
    }
}
