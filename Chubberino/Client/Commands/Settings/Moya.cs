using Chubberino.Client.Abstractions;
using System.IO;

namespace Chubberino.Client.Commands.Settings
{
    public sealed class Moya : Setting
    {
        public Moya(IExtendedClient client, TextWriter console)
            : base(client, console)
        {

        }
    }
}
