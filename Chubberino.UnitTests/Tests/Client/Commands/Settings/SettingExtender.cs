using Chubberino.Client;
using Chubberino.Client.Commands.Settings;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings
{
    /// <summary>
    /// Extends <see cref="Setting"/> to be instantiated.
    /// </summary>
    public sealed class SettingExtender : Setting
    {
        public SettingExtender(ITwitchClientManager client, IConsole console)
            : base(client, console)
        {
        }
    }
}
