using Chubberino.Client.Abstractions;
using Chubberino.Client.Commands.Settings;
using System.IO;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings
{
    /// <summary>
    /// Extends <see cref="Setting"/> to be instantiated.
    /// </summary>
    public sealed class SettingExtender : Setting
    {
        public SettingExtender(IExtendedClient client, TextWriter console)
            : base(client, console)
        {
        }
    }
}
