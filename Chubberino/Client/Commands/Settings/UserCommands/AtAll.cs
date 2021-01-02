using Chubberino.Client.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TwitchLib.Api;
using TwitchLib.Api.Core.Models.Undocumented.Chatters;
using TwitchLib.Api.Interfaces;
using TwitchLib.Client.Events;

namespace Chubberino.Client.Commands.Settings.UserCommands
{
    public sealed class AtAll : UserCommand
    {
        private List<String> UsersInChannel { get; set; } = new List<String>();

        private ITwitchAPI Api { get; }
        public IBot Bot { get; }

        public AtAll(IExtendedClient client, TextWriter console, ITwitchAPI api, IBot bot) : base(client, console)
        {
            IsEnabled = true;
            Api = api;
            Bot = bot;
        }

        public override void Execute(IEnumerable<String> arguments)
        {
            List<ChatterFormatted> chatters = Api.Undocumented.GetChattersAsync(Bot.ChannelName).Result;

            foreach (var user in chatters)
            {
                TwitchClient.SpoolMessage(user.Username + " " + String.Join(' ', arguments));
            };
        }
    }
}
